using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataGridViewDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Button btn1 = new Button();

        private void Form1_Load(object sender, EventArgs e)
        {
            // 为DataGridView赋值的两种方法
            // 方法一：DataGridView.DataSource = DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("IP");
            dt.Columns.Add("Option");
            dt.Columns.Add("button");
            for (int i = 10; i < 20; i++)
                dt.Rows.Add($"192.168.1.{i}", $"Option{i - 10}", $"button{i - 10}");

            dataGridView1.DataSource = dt;

            //// 方法二：DataGridView.DataSource = List
            //var list = new List<Object>();
            //list.Add(new { IP = "192.168.1.10", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.11", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.12", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.13", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.14", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.15", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.16", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.17", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.18", Option = "null", button = "null" });
            //list.Add(new { IP = "192.168.1.19", Option = "null", button = "null" });
            //dataGridView1.DataSource = list;

            DataGridViewCheckBoxColumn newColumn1 = new DataGridViewCheckBoxColumn();
            newColumn1.HeaderText = "选择";
            dataGridView1.Columns.Insert(3, newColumn1);

            DataGridViewButtonColumn newColumn2 = new DataGridViewButtonColumn();
            newColumn2.HeaderText = "控件";
            dataGridView1.Columns.Insert(4, newColumn2);

            dt.Columns.Add("action");

            // 默认选择0,0位置
            dataGridView1.Rows[0].Cells[0].Value = true;

            btn1 = new Button
            {
                Name = "btnRun",
                Text = "Run",
                Visible = true,
                Location = new Point(550, 80),
                Size = new Size(80, 50),
                Parent = this,
            };
            btn1.Click += Btn1_Click;
            dataGridView1.Controls.Add(btn1);
        }

        // 弹窗显示CheckBox已选择的数据行
        private void Btn1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string otherValue = dataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString();
                if (otherValue == "True")
                    MessageBox.Show(dataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString());
            }
        }

        // 点击Button时弹窗显示IP地址
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns[1].Index)
            {
                MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
        }

        // 将当前单元格中的更改提交到数据缓存，但不结束编辑模式，及时获得其状态是选中还是未选中    
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    int colIndex = dataGridView1.CurrentCell.ColumnIndex;
                    if (colIndex == 0) //第一列
                    {
                        string _selectValue = dataGridView1.CurrentCell.EditedFormattedValue.ToString();
                        if (_selectValue == "True")
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (i != rowIndex)
                                {
                                    string otherValue = dataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString();
                                    if (otherValue == "True")
                                    {
                                        ((DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0]).Value = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception){}
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // 对第5列相同单元格进行合并 
            if (e.ColumnIndex == 5 && e.RowIndex != -1)
            {
                using
                (
                Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
                backColorBrush = new SolidBrush(e.CellStyle.BackColor)
                )
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        // 清除单元格 
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                        // 画 Grid 边线（仅画单元格的底边线和右边线） 
                        // 如果下一行和当前行的数据不同，则在当前的单元格画一条底边线 
                        if (e.RowIndex < dataGridView1.Rows.Count - 1 &&
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() !=
                        e.Value.ToString())
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left + 2,
                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);
                        //画最后一条记录的底线 
                        if (e.RowIndex == dataGridView1.Rows.Count - 1)
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left + 2,
                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);
                        // 画右边线 
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                        e.CellBounds.Top, e.CellBounds.Right - 1,
                        e.CellBounds.Bottom);

                        // 画左边线 
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                        e.CellBounds.Top, e.CellBounds.Left,
                        e.CellBounds.Bottom);

                        // 画（填写）单元格内容，相同的内容的单元格只填写第一个 
                        if (e.Value != null)
                        {
                            if (e.RowIndex > 0 &&
                            dataGridView1.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString() ==
                            e.Value.ToString())
                            {
                                //e.Graphics.DrawRectangle(Pens.Red, 0, 0, 100, 100);
                            }
                            else
                            {
                                //e.Graphics.DrawString((String)e.Value, e.CellStyle.Font,
                                //Brushes.Black, e.CellBounds.X + 2,
                                //e.CellBounds.Y + 5, StringFormat.GenericDefault);
                            }
                        }
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
