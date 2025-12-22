using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    public class FormChart_SYA : Form
    {
        private readonly DataTable table_SYA;

        public FormChart_SYA(DataTable table)
        {
            table_SYA = table;

            Text = "Гистограмма окладов сотрудников";
            ClientSize = new Size(900, 500);
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;

            Paint += FormChart_SYA_Paint;
        }

        private void FormChart_SYA_Paint(object sender, PaintEventArgs e)
        {
            if (table_SYA == null || table_SYA.Rows.Count == 0)
                return;

            Graphics g = e.Graphics;

            var data = table_SYA.AsEnumerable()
                .Select(r => new
                {
                    Name = r["Фамилия"].ToString(),
                    Salary = double.TryParse(r["Оклад"].ToString(), out double v) ? v : 0
                })
                .Where(x => x.Salary > 0)
                .ToList();

            if (data.Count == 0)
                return;

            int margin = 60;
            int chartHeight = ClientSize.Height - margin * 2;
            int chartWidth = ClientSize.Width - margin * 2;

            double maxSalary = data.Max(x => x.Salary);
            int barWidth = chartWidth / data.Count;

            Pen axisPen = Pens.Black;
            Brush barBrush = Brushes.SteelBlue;
            Font labelFont = new Font("Segoe UI", 9);

            // Оси
            g.DrawLine(axisPen, margin, margin, margin, margin + chartHeight);
            g.DrawLine(axisPen, margin, margin + chartHeight, margin + chartWidth, margin + chartHeight);

            for (int i = 0; i < data.Count; i++)
            {
                int barHeight = (int)(chartHeight * data[i].Salary / maxSalary);

                int x = margin + i * barWidth + 5;
                int y = margin + chartHeight - barHeight;

                g.FillRectangle(barBrush, x, y, barWidth - 10, barHeight);
                g.DrawRectangle(Pens.Black, x, y, barWidth - 10, barHeight);

                g.DrawString(
                    data[i].Name,
                    labelFont,
                    Brushes.Black,
                    x,
                    margin + chartHeight + 5
                );
            }
        }
    }
}
