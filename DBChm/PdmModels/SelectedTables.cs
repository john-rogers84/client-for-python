using DocTools.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCHM.PdmModels
{
    /// <summary>
    /// 关键词过滤
    /// </summary>
    [Serializable]
    public class SelectedTables
    {
        public List<TableDto> tables { get; set; } = new List<TableDto>();

        //var viewDict = new NameValueCollection();
        //var procDict = new NameValueCollection();

        public Dictionary<string, string> viewDict { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> procDict { get; set; } = new Dictionary<string, string>();


        public void Clear()
        {
            this.tables.Clear();
            this.viewDict.Clear();
            this.procDict.Clear();
        }
    }
}
