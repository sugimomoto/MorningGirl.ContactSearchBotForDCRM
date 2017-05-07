using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using MorningGirl.ContactSearchBotForDCRM.Dialogs;
using MorningGirl.ContactSearchBotForDCRM.Model.Crm;

namespace MorningGirl.ContactSearchBotForDCRM
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity == null)
                return new HttpResponseMessage(HttpStatusCode.Accepted);

            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    var clientId = ConfigurationManager.AppSettings["clientId"];
                    var clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                    var serverUri = ConfigurationManager.AppSettings["serverUri"];
                    var tenantId = ConfigurationManager.AppSettings["tenantId"];
                    var AzureAd = new AzureAdConnect(serverUri, tenantId, clientId, clientSecret);

                    var token = await AzureAd.GetAccessToken();

                    await Conversation.SendAsync(activity, () => new CrmSearchDialog(token,serverUri));
                    break;
                
                // 開始時のメッセージ処理
                case ActivityTypes.ConversationUpdate:
                    if(!IsNewConnection(activity))
                        break;

                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    await connector.Conversations.ReplyToActivityAsync(
                        activity.CreateReply($"こんにちは。Dynamics CRM Contact Search サービスです。検索したい担当者の名前、または会社名を入力してください。")
                        );
                    
                    break;

                default:
                    Trace.TraceError($"Unknown activity type igored: {activity.GetActivityType()}");
                    break;
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// 新規ユーザーかどうか判定
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private bool IsNewConnection(Activity activity)
        {
            IConversationUpdateActivity update = activity;
            if (!update.MembersAdded.Any())
                return false;

            if (!update.MembersAdded.Contains(activity.Recipient))
                return false;

            return true;
        }
    }
}