using System.Data;
using System.Text;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11.Lib
{
    public class DataService
    {
        public DataTable LoadCsv(string path)
        {
            var table = new DataTable();

            var lines = File.ReadAllLines(path, Encoding.UTF8);
            if (lines.Length == 0) return table;

            var headers = lines[0].Split(';');
            foreach (var h in headers)
                table.Columns.Add(h);

            for (int i = 1; i < lines.Length; i++)
                table.Rows.Add(lines[i].Split(';'));

            return table;
        }

        public void SaveCsv(DataTable table, string path)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Join(";", table.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

            foreach (DataRow row in table.Rows)
                sb.AppendLine(string.Join(";", row.ItemArray));

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        public (int count, double min, double max, double avg) GetSalaryStatistics(DataTable table)
        {
            var salaries = table.AsEnumerable()
                .Select(r => double.TryParse(r["Оклад"].ToString(), out double v) ? v : 0)
                .Where(v => v > 0)
                .ToList();

            if (salaries.Count == 0)
                return (0, 0, 0, 0);

            return (
                salaries.Count,
                salaries.Min(),
                salaries.Max(),
                salaries.Average()
            );
        }

    }
}
