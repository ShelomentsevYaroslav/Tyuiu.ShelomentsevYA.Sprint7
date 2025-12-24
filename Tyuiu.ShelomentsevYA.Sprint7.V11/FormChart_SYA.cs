using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    public sealed class FormChart_SYA : Form
    {
        internal sealed class DeptStat_SYA
        {
            public string Name { get; init; } = "—";
            public int Count { get; init; }
            public double AvgSalary { get; init; }
            public double SumSalary { get; init; }
        }

        private readonly List<DeptStat_SYA> stats_SYA;

        // ===== CONST UI =====
        private const int MarginOuter = 34;
        private const int Gap = 26;

        public FormChart_SYA(DataTable table)
        {
            Text = "Аналитика по подразделениям";
            WindowState = FormWindowState.Maximized;
            BackColor = Color.White;
            DoubleBuffered = true;

            stats_SYA = BuildStatistics(table);
        }

        // ===== DATA =====

        private static List<DeptStat_SYA> BuildStatistics(DataTable? table)
        {
            if (table == null)
                return new();

            if (!table.Columns.Contains("Подразделение") ||
                !table.Columns.Contains("Оклад"))
                return new();

            return table.AsEnumerable()
                .Select(r =>
                {
                    bool ok = double.TryParse(r["Оклад"]?.ToString(), out double salary);
                    return new { Row = r, Ok = ok, Salary = salary };
                })
                .Where(x => x.Ok)
                .GroupBy(x => x.Row["Подразделение"]?.ToString() ?? "—")
                .Select(g => new DeptStat_SYA
                {
                    Name = g.Key,
                    Count = g.Count(),
                    AvgSalary = g.Average(x => x.Salary),
                    SumSalary = g.Sum(x => x.Salary)
                })
                .OrderByDescending(x => x.AvgSalary)
                .ToList();
        }

        // ===== PAINT =====

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (stats_SYA.Count == 0)
            {
                DrawEmptyMessage(e.Graphics);
                return;
            }

            var layout = CalculateLayout(ClientSize);

            DrawBarChart(e.Graphics, layout.AvgRect, stats_SYA, s => s.AvgSalary,
                "Средний оклад по подразделениям");

            DrawBarChart(e.Graphics, layout.CountRect, stats_SYA, s => s.Count,
                "Количество сотрудников");

            DrawBarChart(e.Graphics, layout.SumRect, stats_SYA, s => s.SumSalary,
                "Фонд оплаты труда");
        }

        // ===== LAYOUT =====

        private static (Rectangle AvgRect, Rectangle CountRect, Rectangle SumRect)
            CalculateLayout(Size size)
        {
            int topRowHeight = (size.Height - MarginOuter * 3 - Gap) / 2;

            Rectangle avg = new(
                MarginOuter,
                MarginOuter + 18,
                (size.Width - MarginOuter * 2 - Gap) / 2,
                topRowHeight
            );

            Rectangle count = new(
                avg.Right + Gap,
                avg.Top,
                avg.Width,
                avg.Height
            );

            Rectangle sum = new(
                MarginOuter,
                avg.Bottom + MarginOuter,
                size.Width - MarginOuter * 2,
                topRowHeight
            );

            return (avg, count, sum);
        }

        // ===== DRAW =====

        private static void DrawEmptyMessage(Graphics g)
        {
            using Font f = new("Segoe UI", 14, FontStyle.Italic);
            g.DrawString(
                "Недостаточно данных для построения диаграммы",
                f,
                Brushes.Gray,
                40,
                40
            );
        }

        private static void DrawBarChart(
            Graphics g,
            Rectangle area,
            List<DeptStat_SYA> data,
            Func<DeptStat_SYA, double> selector,
            string title)
        {
            using Font titleFont = new("Segoe UI", 11, FontStyle.Bold);
            using Font labelFont = new("Segoe UI", 8);
            using Font valueFont = new("Segoe UI", 7);
            using Brush barBrush = new SolidBrush(Color.FromArgb(90, 140, 200));

            g.DrawString(title, titleFont, Brushes.Black, area.Left, area.Top - 26);

            double maxValue = data.Max(selector);
            if (maxValue <= 0) return;

            const int innerTop = 10;
            const int innerBottom = 58;

            int plotLeft = area.Left + 4;
            int plotRight = area.Right - 4;
            int plotTop = area.Top + innerTop;
            int plotBottom = area.Bottom - innerBottom;

            int plotWidth = plotRight - plotLeft;
            int plotHeight = plotBottom - plotTop;
            if (plotWidth <= 0 || plotHeight <= 0) return;

            int n = data.Count;
            int barSpacing = 10;
            int barWidth = Math.Max(18, plotWidth / n - barSpacing);

            for (int i = 0; i < n; i++)
            {
                DrawBar(
                    g,
                    data[i],
                    selector,
                    maxValue,
                    plotLeft,
                    plotBottom,
                    plotHeight,
                    barWidth,
                    barSpacing,
                    i,
                    barBrush,
                    labelFont,
                    valueFont
                );
            }
        }

        private void InitializeComponent()
        {

        }

        private static void DrawBar(
            Graphics g,
            DeptStat_SYA item,
            Func<DeptStat_SYA, double> selector,
            double maxValue,
            int plotLeft,
            int plotBottom,
            int plotHeight,
            int barWidth,
            int spacing,
            int index,
            Brush brush,
            Font labelFont,
            Font valueFont)
        {
            double value = selector(item);
            int barHeight = (int)(value / maxValue * plotHeight);

            int x = plotLeft + index * (barWidth + spacing);
            int y = plotBottom - barHeight;

            g.FillRectangle(brush, x, y, barWidth, barHeight);
            g.DrawRectangle(Pens.Black, x, y, barWidth, barHeight);

            g.DrawString(
                value.ToString("N0"),
                valueFont,
                Brushes.Black,
                x + barWidth / 2,
                y - 12,
                new StringFormat { Alignment = StringAlignment.Center }
            );

            g.TranslateTransform(x + barWidth / 2, plotBottom + 6);
            g.RotateTransform(-35);
            g.DrawString(item.Name, labelFont, Brushes.Black, 0, 0,
                new StringFormat { Alignment = StringAlignment.Far });
            g.ResetTransform();
        }
    }
}
