using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MorningGirl.ContactSearchBotForDCRM.Model.Crm
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ContactSearch
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="entityName"></param>
        public ContactSearch(string searchText)
        {
            this.InputSearchText = searchText;
        }

        /// <summary>
        /// 検索対象の名前
        /// </summary>
        public string InputSearchText { get; set; }

        /// <summary>
        /// 取得対象の列名
        /// </summary>
        public string[] InpurtSelectText =
        {
            "fullname", "_parentcustomerid_value", "telephone1", "emailaddress1", "ownerid", "contactid", "lastname",
            "firstname", "yomifullname", "yomilastname", "yomifirstname", "statuscode"
        };
    
        /// <summary>
        /// スプリット用Char
        /// 検索文字列分解用
        /// </summary>
        private char[] SpritText = {' ','　'};

        /// <summary>
        /// 検索対象列の取得
        /// </summary>
        private string SelectQuery => $"$select={string.Join(",", InpurtSelectText)}";

        /// <summary>
        /// 検索文字列をString配列に分解
        /// </summary>
        private string[] SearchTexts => InputSearchText.Split(SpritText);

        /// <summary>
        /// 並び順
        /// </summary>
        private string OrderQuery = "$orderby=fullname asc";
            
        /// <summary>
        /// Like検索用 or 条件のQueryを作成
        /// </summary>
        private string LikeFilterQuery {
            get
            {
                var searchList = new List<string>();

                foreach (var column in SearchColumns)
                    foreach (var text in SearchTexts)
                        searchList.Add($"contains({column},'{text}')");

                return $"$filter={string.Join(" or ", searchList)}";
            }
        }

        /// <summary>
        /// 対象のエンティティ名
        /// </summary>
        public string EntityName = "contacts";

        /// <summary>
        /// 
        /// </summary>
        public string OptionQuery = "$top=10";
        
        /// <summary>
        /// 検索対象項目名
        /// </summary>
        private string[] SearchColumns = {"fullname", "yomifullname" };

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Query => $"?{SelectQuery}&{LikeFilterQuery}&{OrderQuery}&{OptionQuery}";
    }
}