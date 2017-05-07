using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MorningGirl.ContactSearchBotForDCRM.Model.Crm;

namespace MorningGirl.ContactSearchBotForDCRM.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CrmSearchDialog : IDialog<ContactSearch>
    {
        public CrmWebApiConnection CrmConnection { get; set; }

        public ContactSearch ContactSearch;

        public string ServerUri { get; set; }

        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CrmSearchDialog(string token, string serverUri)
        {
            Token = token;
            ServerUri = serverUri;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(SearchResultMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task SearchResultMessage(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            this.CrmConnection = new CrmWebApiConnection(Token, ServerUri);

            var activity = await result;
            
            ContactSearch = new ContactSearch(activity.Text);

            var contactList =
                await CrmConnection.RetrieveMultiple<CrmWebApiRoot<Contacts>>(ContactSearch.EntityName, ContactSearch.Query);

            await context.PostAsync($"取引先担当者を{contactList.value.Length}件取得しました。上位から結果を表示します。");

            var replay = context.MakeMessage();
            replay.Attachments = new List<Attachment>();

            replay.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            foreach (var contact in contactList.value)
            {

                var actions = new List<CardAction>();
                actions.Add(new CardAction
                {
                    Title = $"取引先企業を表示",
                    Value = $"https://sugi38.crm7.dynamics.com/main.aspx?etc=1&id=%7b{contact._parentcustomerid_value}%7d&pagetype=entityrecord",
                    Type = ActionTypes.OpenUrl
                });

                actions.Add(new CardAction
                {
                    Title = $"取引先担当者を表示",
                    Value = $"https://sugi38.crm7.dynamics.com/main.aspx?etc=2&id=%7b{contact.contactid}%7d&pagetype=entityrecord",
                    Type = ActionTypes.OpenUrl
                });

                replay.Attachments.Add(
                    new HeroCard
                    {
                        Title = $"名前：{contact.fullname}({contact.yomifullname})",
                        Subtitle = $"会社名：{contact._parentcustomerid_valueODataCommunityDisplayV1FormattedValue}",
                        Text = $"メールアドレス：{contact.emailaddress1}",
                        Buttons = actions
                    }.ToAttachment()
                    );
            }

            await context.PostAsync(replay);
        }
    }
}