using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MorningGirl.ContactSearchBotForDCRM.Model.Crm
{
    /// <summary>
    /// 取引先担当者格納
    /// </summary>
    public class Contacts
    {
        [JsonProperty("fullname")]
        public string fullname { get; set; }

        [JsonProperty("_parentcustomerid_value@OData.Community.Display.V1.FormattedValue")]
        public string _parentcustomerid_valueODataCommunityDisplayV1FormattedValue { get; set; }

        [JsonProperty("_parentcustomerid_value@Microsoft.Dynamics.CRM.associatednavigationproperty")]
        public string _parentcustomerid_valueMicrosoftDynamicsCRMassociatednavigationproperty { get; set; }

        [JsonProperty("_parentcustomerid_value@Microsoft.Dynamics.CRM.lookuplogicalname")]
        public string _parentcustomerid_valueMicrosoftDynamicsCRMlookuplogicalname { get; set; }

        [JsonProperty("_parentcustomerid_value")]
        public string _parentcustomerid_value { get; set; }

        [JsonProperty("telephone1")]
        public string telephone1 { get; set; }

        [JsonProperty("emailaddress1")]
        public string emailaddress1 { get; set; }

        [JsonProperty("contactid")]
        public string contactid { get; set; }

        [JsonProperty("lastname")]
        public string lastname { get; set; }

        [JsonProperty("firstname")]
        public string firstname { get; set; }

        [JsonProperty("yomifullname")]
        public string yomifullname { get; set; }

        [JsonProperty("yomilastname")]
        public object yomilastname { get; set; }

        [JsonProperty("yomifirstname")]
        public object yomifirstname { get; set; }

        [JsonProperty("statuscode@OData.Community.Display.V1.FormattedValue")]
        public string statuscodeODataCommunityDisplayV1FormattedValue { get; set; }

        [JsonProperty("statuscode")]
        public int statuscode { get; set; }
    }
}