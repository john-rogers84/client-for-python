using ComponentFactory.Krypton.Toolkit;
using DocTools;
using DocTools.DBDoc;
using DocTools.Dtos;
using MJTop.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;


namespace DBCHM
{
    /// <summary>
    /// 实现列名称的搜索
    /// </summary>
    public partial class MainForm
    {
        private ToolTip m_Tooltip = new ToolTip();

       
        /// <summary>
        /// 按列名称搜索
        /// </summary>
        private void InitTree_SearchColumn()
        {
            if (DBUtils.Instance?.Info == null)
            {
                return;
            }

            lblTip.Text = string.Empty;
            treeDB.Nodes.Clear();

            if (!string.IsNullOrWhiteSpace(DBUtils.Instance?.Info?.DBName))
            {
                var ver = ((System.Reflection.AssemblyFileVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)?.FirstOrDefault())?.Version;
                if (ver == null)
                {
                    ver = Assembly.GetExecutingAssembly().GetName()?.Version?.ToString();
                }
                this.Text = DBUtils.Instance?.Info?.DBName + "(" + DBUtils.Instance.DBType.ToString() + ") - " + "DBCHM v" + ver;
            }

            BtnSaveGridData.Enabled = true;

            if (DBUtils.Instance.DBType == MJTop.Data.DBType.SQLite)
            {
                SetMsg(DBUtils.Instance.DBType + "数据库不支持批注功能！", false);
                BtnSaveGridData.Enabled = false;
            }

            //定义填充子节点方法
            Action<TreeNode, List<string>> fillTreeNodeFunc = (TreeNode parentNode, List<string> nodeNameList) =>
            {
                //去重,排序
                nodeNameList = nodeNameList.Distinct().OrderBy(p => p).ToList();
                //加入table
                foreach (var item in nodeNameList)
                {
                    string tbName = item;
                    TreeNode node = new TreeNode(tbName, 1, 1);
                    node.Name = tbName;
                    if (this._selectedTables.tables.Any(u => String.Equals(u.TableName, tbName, StringComparison.OrdinalIgnoreCase)))
                    {
                        node.Checked = true;
                        //ckCount++;
                    }
                    parentNode.Nodes.Add(node);
                }

            };

            //表中含有列名称的表名称
            if (DBUtils.Instance.Info.TableNames.Count > 0)
            {
                TreeNode tnTable = new TreeNode("表", 0, 0) { Name = "table" };
                int ckCount = 0;

                #region 搜索关键字匹配列名称
                if (SearchWords.Any())
                {
                    List<string> listColumnContainTable = new List<string>();

                    foreach (var word in SearchWords)
                    {

                        var currentTableList = DBUtils.Instance.Info.TableInfoDict.Where(p => p.Value.Colnumns.Any(pf => pf.ColumnName.Equals(word))).Select(p => p.Key).Distinct();

                        if (currentTableList.Any())
                        {
                            listColumnContainTable.AddRange(currentTableList);
                        }

                    }

                    fillTreeNodeFunc(tnTable, listColumnContainTable);
                }
                else
                {
                    fillTreeNodeFunc(tnTable, DBUtils.Instance.Info.TableNames);
                }

                #endregion


                tnTable.Checked = tnTable.Nodes.Count == ckCount;
                treeDB.Nodes.Add(tnTable);


            }

            //视图中的列名称需要分析sql语句，暂未实现
            

            foreach (TreeNode node in treeDB.Nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    treeDB.SelectedNode = node.Nodes[0];
                    node.Expand();
                    break;
                }
            }

            this.TongJi();
        }

        /// <summary>
        /// 列搜索选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbIsColumn_CheckedChanged(object sender, EventArgs e)
        {
            ikMethod method;
            if (cbIsColumn.Checked)
            {
               
                method = new ikMethod(InitTree_SearchColumn);
                this.tlTip.SetToolTip(this.TxtSearchWords, "多个关键字搜索用英文逗号(,)隔开！按列名称搜索");
            }
            else
            {

                method = new ikMethod(InitTree);
                this.tlTip.SetToolTip(this.TxtSearchWords, "多个关键字搜索用英文逗号(,)隔开！按表名称搜索");
            }
            this.Invoke(method);
        }

    }
}
