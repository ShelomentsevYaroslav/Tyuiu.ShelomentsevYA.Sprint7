using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11.Lib
{
    public class DataService
    {
        // ================= CSV =================

        public DataTable LoadCsv(string path)
        {
            var table = new DataTable();

            if (!File.Exists(path))
                return table;

            var lines = File.ReadAllLines(path);
            if (lines.Length == 0)
                return table;

            char delimiter = DetectDelimiter(lines[0]);

            // заголовки
            var headers = lines[0].Split(delimiter);
            foreach (var h in headers)
                table.Columns.Add(h.Trim());

            // строки
            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(delimiter);
                var row = table.NewRow();

                for (int c = 0; c < table.Columns.Count; c++)
                    row[c] = c < values.Length && !string.IsNullOrWhiteSpace(values[c])
                        ? values[c]
                        : DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }

        public void SaveCsv(DataTable table, string path)
        {
            if (table == null || table.Columns.Count == 0)
                return;

            var sb = new StringBuilder();

            sb.AppendLine(string.Join(";", table.Columns
                .Cast<DataColumn>()
                .Select(c => c.ColumnName)));

            foreach (DataRow row in table.Rows)
            {
                var values = row.ItemArray
                    .Select(v => v == DBNull.Value || v == null
                        ? ""
                        : v.ToString().Replace(";", ","));

                sb.AppendLine(string.Join(";", values));
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private static char DetectDelimiter(string line)
        {
            int semicolon = line.Count(c => c == ';');
            int comma = line.Count(c => c == ',');
            return semicolon >= comma ? ';' : ',';
        }

        // ================= VALIDATION =================

        public static bool IsNumericColumn(DataColumn column)
        {
            foreach (DataRow row in column.Table.Rows)
            {
                if (row[column] == DBNull.Value)
                    continue;

                if (!double.TryParse(row[column].ToString(), out _))
                    return false;
            }
            return true;
        }

        public static bool IsCategoricalColumn(DataColumn column)
        {
            return column.DataType == typeof(string);
        }

        // ================= SIMPLE STATISTICS =================

        public (int Count, double Min, double Max, double Average)
            GetSalaryStatistics(DataTable table, string salaryColumn = "Оклад")
        {
            if (table == null || !table.Columns.Contains(salaryColumn))
                return (0, 0, 0, 0);

            var values = table.AsEnumerable()
                .Select(r => double.TryParse(r[salaryColumn]?.ToString(), out var v) ? (double?)v : null)
                .Where(v => v.HasValue)
                .Select(v => v.Value)
                .ToList();

            if (values.Count == 0)
                return (0, 0, 0, 0);

            return (
                values.Count,
                values.Min(),
                values.Max(),
                Math.Round(values.Average(), 2)
            );
        }

        // ================= EXPORT =================

        public void ExportTable(DataTable table, string path)
        {
            if (table == null || table.Columns.Count == 0)
                return;

            var sb = new StringBuilder();

            sb.AppendLine(string.Join(";", table.Columns
                .Cast<DataColumn>()
                .Select(c => c.ColumnName)));

            foreach (DataRow row in table.Rows)
                sb.AppendLine(string.Join(";", row.ItemArray.Select(v => v?.ToString() ?? "")));

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
