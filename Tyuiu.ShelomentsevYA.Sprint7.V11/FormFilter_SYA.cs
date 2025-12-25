using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    public partial class FormFilter_SYA : Form
    {
        private readonly DataTable table;
        public string ResultFilter { get; private set; } = "";

        public FormFilter_SYA(DataTable table)
        {
            InitializeComponent();
            this.table = table;
            BuildFilterUI();
        }

        private void BuildFilterUI()
        {
            panelFilters.Controls.Clear();
            int y = 10;

            foreach (DataColumn col in table.Columns)
            {
                Label lbl = new Label
                {
                    Text = col.ColumnName,
                    Left = 10,
                    Top = y + 4,
                    Width = 150
                };

                if (IsNumericColumn(col))
                {
                    TextBox from = new TextBox { Left = 170, Top = y, Width = 80, Tag = col };
                    TextBox to = new TextBox { Left = 260, Top = y, Width = 80, Tag = col };

                    panelFilters.Controls.Add(lbl);
                    panelFilters.Controls.Add(from);
                    panelFilters.Controls.Add(to);
                }
                else
                {
                    TextBox tb = new TextBox
                    {
                        Left = 170,
                        Top = y,
                        Width = 170,
                        Tag = col
                    };

                    panelFilters.Controls.Add(lbl);
                    panelFilters.Controls.Add(tb);
                }

                y += 30;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            foreach (Control c in panelFilters.Controls)
            {
                if (c is TextBox tb && tb.Tag is DataColumn col)
                {
                    string value = tb.Text.Trim();
                    if (string.IsNullOrEmpty(value))
                        continue;

                    if (IsNumericColumn(col))
                    {
                        // от / до
                        if (tb.Left < 200 && double.TryParse(value, out double from))
                        {
                            Append(sb, $"[{col.ColumnName}] >= {from}");
                        }
                        else if (tb.Left > 200 && double.TryParse(value, out double to))
                        {
                            Append(sb, $"[{col.ColumnName}] <= {to}");
                        }
                    }
                    else
                    {
                        value = value.Replace("'", "''");
                        Append(sb, $"[{col.ColumnName}] LIKE '%{value}%'");
                    }
                }
            }

            ResultFilter = sb.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ResultFilter = "";
            DialogResult = DialogResult.OK;
            Close();
        }

        private static void Append(StringBuilder sb, string expr)
        {
            if (sb.Length > 0)
                sb.Append(" AND ");
            sb.Append(expr);
        }

        private static bool IsNumericColumn(DataColumn col)
        {
            foreach (DataRow r in col.Table.Rows)
            {
                if (r[col] == DBNull.Value) continue;
                if (!double.TryParse(r[col].ToString(), out _))
                    return false;
            }
            return true;
        }
    }
}
