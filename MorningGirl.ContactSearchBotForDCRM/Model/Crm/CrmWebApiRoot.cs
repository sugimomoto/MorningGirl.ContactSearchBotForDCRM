using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MorningGirl.ContactSearchBotForDCRM.Model.Crm
{
    class CrmWebApiRoot<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public string odatacontext { get; set; }

        /// <summary>
        /// 複数レコード取得時の格納用
        /// </summary>
        public T[] value { get; set; }
    }
}