using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tyuiu.ShelomentsevYA.Sprint7.V11.Lib;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    public partial class FormMain : Form
    {
        private readonly DataService dataService_SYA = new();
        private readonly BindingSource bindingSource_SYA = new();

        private DataTable table_SYA = new();
        private readonly Dictionary<string, string> csvFiles_SYA = new();

        private string currentPath_SYA = string.Empty;
        private bool isEditMode_SYA = false;

        public FormMain()
        {
            InitializeComponent();

            dataGridViewEmployees_SYA.AutoGenerateColumns = true;
            dataGridViewEmployees_SYA.DataSource = bindingSource_SYA;

            ConfigureToolStrip();
            ApplyIcons();

            Text = "Отдел кадров — Вариант 11";
            MinimumSize = new Size(1000, 600);
        }

        // ================= TOOLSTRIP =================

        private void ConfigureToolStrip()
        {
            toolStripMain_SYA.GripStyle = ToolStripGripStyle.Hidden;
            toolStripMain_SYA.AutoSize = false;
            toolStripMain_SYA.Height = 42;
            toolStripMain_SYA.Padding = new Padding(4, 2, 4, 2);
            toolStripMain_SYA.ImageScalingSize = new Size(16, 16);

            foreach (ToolStripItem item in toolStripMain_SYA.Items)
            {
                if (item is not ToolStripButton btn)
                    continue;

                btn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;

                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextAlign = ContentAlignment.MiddleCenter;

                btn.AutoSize = false;
                btn.Size = new Size(110, 32);
                btn.Margin = new Padding(2);
                btn.Padding = new Padding(8, 0, 8, 0);

                btn.ImageScaling = ToolStripItemImageScaling.None;
                btn.ImageTransparentColor = Color.Magenta;
            }
        }

        private void ApplyIcons()
        {
            foreach (ToolStripItem item in toolStripMain_SYA.Items)
            {
                if (item is not ToolStripButton btn)
                    continue;

                btn.Image = btn.Text switch
                {
                    "Добавить" => LoadIcon("add.png"),
                    "Изменить" => LoadIcon("pencil.png"),
                    "Удалить" => LoadIcon("delete.png"),
                    "Статистика" => LoadIcon("chart_bar.png"),
                    "График" => LoadIcon("chart_line.png"),
                    "Обновить" => LoadIcon("arrow_refresh.png"),
                    _ => null
                };
            }

            toolStripMenuItemOpenCsv_SYA.Image = LoadIcon("page_white_excel.png");
            toolStripMenuItemOpenFolder_SYA.Image = LoadIcon("folder_table.png");
            toolStripMenuItemSaveCsv_SYA.Image = LoadIcon("disk.png");
            toolStripMenuItemExit_SYA.Image = LoadIcon("door_out.png");

            toolStripMenuItemUserGuide_SYA.Image = LoadIcon("help.png");
            toolStripMenuItemAbout_SYA.Image = LoadIcon("information.png");
        }

        private Image LoadIcon(string name)
        {
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Icons",
                name
            );

            return File.Exists(path) ? Image.FromFile(path) : null;
        }

        // ================= FILE =================

        private void OpenCsv_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new()
            {
                Filter = "CSV files (*.csv)|*.csv"
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            csvFiles_SYA.Clear();
            toolStripComboBoxTables_SYA.Items.Clear();

            string path = dlg.FileName;
            string key = Path.GetFileNameWithoutExtension(path);

            csvFiles_SYA[key] = path;
            toolStripComboBoxTables_SYA.Items.Add(key);
            toolStripComboBoxTables_SYA.SelectedIndex = 0;

            LoadTable(path);
        }

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dlg = new();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            csvFiles_SYA.Clear();
            toolStripComboBoxTables_SYA.Items.Clear();

            foreach (var file in Directory.GetFiles(dlg.SelectedPath, "*.csv"))
            {
                string key = Path.GetFileNameWithoutExtension(file);
                csvFiles_SYA[key] = file;
                toolStripComboBoxTables_SYA.Items.Add(key);
            }

            if (toolStripComboBoxTables_SYA.Items.Count > 0)
                toolStripComboBoxTables_SYA.SelectedIndex = 0;
        }

        private void Tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBoxTables_SYA.SelectedItem is not string key)
                return;

            if (csvFiles_SYA.TryGetValue(key, out var path))
                LoadTable(path);
        }

        private void SaveCsv_Click(object sender, EventArgs e)
        {
            if (table_SYA.Rows.Count == 0)
                return;

            using SaveFileDialog dlg = new()
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = Path.GetFileName(currentPath_SYA)
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            dataService_SYA.SaveCsv(table_SYA, dlg.FileName);
            currentPath_SYA = dlg.FileName;
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            if (File.Exists(currentPath_SYA))
                LoadTable(currentPath_SYA);
        }

        // ================= LOAD =================

        private void LoadTable(string path)
        {
            UseWaitCursor = true;

            try
            {
                table_SYA = SafeLoadCsv(path);
                bindingSource_SYA.DataSource = table_SYA;

                currentPath_SYA = path;
                isEditMode_SYA = false;
                dataGridViewEmployees_SYA.ReadOnly = true;
            }
            finally
            {
                UpdateFeatureAvailability();
                UseWaitCursor = false;
            }
        }

        private DataTable SafeLoadCsv(string path)
        {
            var table = new DataTable();
            var lines = File.ReadAllLines(path);

            if (lines.Length == 0)
                return table;

            var headers = lines[0].Split(';');
            foreach (var h in headers)
                table.Columns.Add(h.Trim());

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(';');
                var row = table.NewRow();

                for (int c = 0; c < table.Columns.Count; c++)
                    row[c] = c < values.Length ? values[c] : DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }

        // ================= CRUD =================

        private void Add_Click(object sender, EventArgs e)
        {
            table_SYA.Rows.Add(table_SYA.NewRow());
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            isEditMode_SYA = !isEditMode_SYA;
            dataGridViewEmployees_SYA.ReadOnly = !isEditMode_SYA;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewEmployees_SYA.SelectedRows)
                if (row.DataBoundItem is DataRowView v)
                    v.Row.Delete();
        }

        // ================= SEARCH =================

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (table_SYA == null || table_SYA.Columns.Count == 0)
                return;

            if (sender is not ToolStripTextBox tb)
                return;

            string value = tb.Text;

            if (string.IsNullOrWhiteSpace(value))
            {
                table_SYA.DefaultView.RowFilter = string.Empty;
                return;
            }

            value = value
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("]", "[]]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");

            var filters = table_SYA.Columns
                .Cast<DataColumn>()
                .Where(c => c.DataType == typeof(string))
                .Select(c => $"[{c.ColumnName}] LIKE '%{value}%'")
                .ToList();

            if (filters.Count == 0)
            {
                table_SYA.DefaultView.RowFilter = string.Empty;
                return;
            }

            try
            {
                table_SYA.DefaultView.RowFilter = string.Join(" OR ", filters);
            }
            catch
            {
                table_SYA.DefaultView.RowFilter = string.Empty;
            }
        }


        private void ClearSearch_Click(object sender, EventArgs e)
        {
            toolStripTextBoxSearch_SYA.Text = string.Empty;
            table_SYA.DefaultView.RowFilter = string.Empty;
        }

        // ================= INFO =================

        private void Statistics_Click(object sender, EventArgs e)
        {
            if (!CanBuildStatistics(out var reason))
            {
                MessageBox.Show(reason, "Статистика");
                return;
            }

            using var form = new FormStatistics_SYA(table_SYA);
            form.ShowDialog();
        }

        private void Chart_Click(object sender, EventArgs e)
        {
            if (!CanBuildStatistics(out var reason))
            {
                MessageBox.Show(reason, "Диаграмма");
                return;
            }

            using var form = new FormChart_SYA(table_SYA);
            form.ShowDialog();
        }

        private void About_Click(object sender, EventArgs e)
        {
            using var f = new FormAbout_SYA();
            f.ShowDialog();
        }

        private void UserGuide_Click(object sender, EventArgs e)
        {
            using var f = new FormUserGuide_SYA();
            f.ShowDialog();
        }


        private void Exit_Click(object sender, EventArgs e) => Close();

        // ================= HELPERS =================

        private void UpdateFeatureAvailability()
        {
            bool enabled =
                table_SYA.Columns.Contains("Оклад") &&
                table_SYA.AsEnumerable()
                    .Any(r => double.TryParse(r["Оклад"]?.ToString(), out _));

            foreach (ToolStripItem item in toolStripMain_SYA.Items)
                if (item is ToolStripButton btn &&
                    (btn.Text == "Статистика" || btn.Text == "График"))
                    btn.Enabled = enabled;
        }

        private bool CanBuildStatistics(out string reason)
        {
            reason = string.Empty;

            if (table_SYA.Rows.Count == 0)
            {
                reason = "Таблица пустая";
                return false;
            }

            if (!table_SYA.Columns.Contains("Оклад"))
            {
                reason = "Нет колонки «Оклад»";
                return false;
            }

            if (!table_SYA.AsEnumerable()
                .Any(r => double.TryParse(r["Оклад"]?.ToString(), out _)))
            {
                reason = "Колонка «Оклад» не содержит чисел";
                return false;
            }

            return true;
        }
    }
}
