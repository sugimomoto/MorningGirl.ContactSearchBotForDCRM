using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MorningGirl.ContactSearchBotForDCRM.Model.Crm
{
    [Serializable]
    public class CrmWebApiConnection
    {
        /// <summary>
        /// Azure AD から取得したAccess Token
        /// </summary>
        private string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private string ServiceUri { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="token"></param>
        /// <param name="serviceUri"></param>
        public CrmWebApiConnection(string token, string serviceUri)
        {
            this.Token = token;
            this.ServiceUri = serviceUri + "api/data/v8.2/";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recordId"></param>
        /// <param name="entitiesName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<T> Retrieve<T>(Guid recordId, string entitiesName, string query)
        {
            var requestUrl = $"{ServiceUri}/{entitiesName}({recordId.ToString()}){query}";

            var result = this.Send(HttpMethod.Get, requestUrl);

            var content = await result.Result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitiesName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<T> RetrieveMultiple<T>(string entitiesName, string query)
        {
            var requestUrl = $"{ServiceUri}{entitiesName}{query}";

            var result = await this.Send(HttpMethod.Get, requestUrl);

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="targetEntity"></param>
        /// <param name="createJsonContent"></param>
        /// <returns></returns>
        public async Task<string> Create(string targetEntity, string createJsonContent)
        {
            throw new NullReferenceException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="targetEntity"></param>
        /// <param name="createJsonContent"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Update(string targetEntity, string targetGuid, string jsonContent)
        {
            throw new NullReferenceException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Send(HttpMethod method, string requestUrl, string content = null)
        {
            var request = new HttpRequestMessage(method, requestUrl);

            if (content != null)
                request.Content = new StringContent(content);

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 2, 0);
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"*\"");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", this.Token);

                return await client.SendAsync(request);
            }
        }
    }
}