using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    public partial class FormStatistics_SYA : Form
    {
        private const string SalaryColumn = "Оклад";

        private readonly DataTable sourceTable_SYA;
        private DataTable resultTable_SYA = new();

        public FormStatistics_SYA(DataTable table)
        {
            InitializeComponent();

            sourceTable_SYA = table ?? new DataTable();

            comboBoxMode_SYA.SelectedIndex = 0;
            BuildStatistics();
        }

        // ================= BUILD =================

        private void BuildStatistics()
        {
            if (comboBoxMode_SYA.SelectedItem is not string mode)
                return;

            resultTable_SYA = mode switch
            {
                "Общая статистика" => BuildTotalStatistics(),
                "По подразделениям" => BuildGroupedStatistics("Подразделение"),
                "По должностям" => BuildGroupedStatistics("Должность"),
                "По образованию" => BuildGroupedStatistics("Образование"),
                _ => new DataTable()
            };

            dataGridViewStats_SYA.DataSource = resultTable_SYA;
        }

        // ================= TOTAL =================

        private DataTable BuildTotalStatistics()
        {
            DataTable t = new();
            t.Columns.Add("Показатель");
            t.Columns.Add("Значение");

            var salaries = GetSalaryValues();

            t.Rows.Add("Всего сотрудников", sourceTable_SYA.Rows.Count);
            t.Rows.Add("Средний оклад", salaries.Any() ? salaries.Average().ToString("N0") : "—");
            t.Rows.Add("Минимальный оклад", salaries.Any() ? salaries.Min().ToString("N0") : "—");
            t.Rows.Add("Максимальный оклад", salaries.Any() ? salaries.Max().ToString("N0") : "—");

            return t;
        }

        // ================= GROUPED =================

        private DataTable BuildGroupedStatistics(string groupColumn)
        {
            DataTable t = new();
            t.Columns.Add(groupColumn);
            t.Columns.Add("Сотрудников");
            t.Columns.Add("Средний оклад");
            t.Columns.Add("ФОТ");

            if (!sourceTable_SYA.Columns.Contains(groupColumn) ||
                !sourceTable_SYA.Columns.Contains(SalaryColumn))
                return t;

            var groups = sourceTable_SYA.AsEnumerable()
                .GroupBy(r => r[groupColumn]?.ToString() ?? "—");

            foreach (var g in groups)
            {
                var salaries = g
                    .Select(r => TryGetSalary(r))
                    .Where(v => v.HasValue)
                    .Select(v => v!.Value)
                    .ToList();

                t.Rows.Add(
                    g.Key,
                    g.Count(),
                    salaries.Any() ? salaries.Average().ToString("N0") : "—",
                    salaries.Any() ? salaries.Sum().ToString("N0") : "—"
                );
            }

            return t;
        }

        // ================= HELPERS =================

        private double? TryGetSalary(DataRow row)
        {
            return double.TryParse(row[SalaryColumn]?.ToString(), out var v)
                ? v
                : null;
        }

        private System.Collections.Generic.List<double> GetSalaryValues()
        {
            if (!sourceTable_SYA.Columns.Contains(SalaryColumn))
                return new();

            return sourceTable_SYA.AsEnumerable()
                .Select(TryGetSalary)
                .Where(v => v.HasValue)
                .Select(v => v!.Value)
                .ToList();
        }

        // ================= EVENTS =================

        private void comboBoxMode_SYA_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildStatistics();
        }

        private void buttonExportCsv_SYA_Click(object sender, EventArgs e)
        {
            Export("csv");
        }

        private void buttonExportTxt_SYA_Click(object sender, EventArgs e)
        {
            Export("txt");
        }

        // ================= EXPORT =================

        private void Export(string ext)
        {
            if (resultTable_SYA.Rows.Count == 0)
                return;

            using SaveFileDialog dlg = new()
            {
                Filter = $"{ext.ToUpper()} files (*.{ext})|*.{ext}",
                FileName = $"statistics.{ext}"
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            var sb = new StringBuilder();

            sb.AppendLine(string.Join(";",
                resultTable_SYA.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

            foreach (DataRow r in resultTable_SYA.Rows)
                sb.AppendLine(string.Join(";", r.ItemArray));

            File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
        }
    }
}
