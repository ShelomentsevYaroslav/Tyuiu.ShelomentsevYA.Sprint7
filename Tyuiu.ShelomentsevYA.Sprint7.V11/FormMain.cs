using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tyuiu.ShelomentsevYA.Sprint7.V11.Lib;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    public partial class FormMain : Form
    {
        private readonly DataService dataService_SYA = new DataService();

        private DataTable table_SYA = new DataTable();
        private string currentPath_SYA = string.Empty;
        private bool isEditMode_SYA = false;

        public FormMain()
        {
            InitializeComponent();

            Name = "formMain_SYA";
            Text = "Отдел кадров — Вариант 11";
            MinimumSize = new Size(1000, 600);
        }

        // ====== FILE ======

        private void OpenCsv_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                currentPath_SYA = dlg.FileName;
                table_SYA = dataService_SYA.LoadCsv(currentPath_SYA);
                dataGridViewEmployees_SYA.DataSource = table_SYA;
                dataGridViewEmployees_SYA.ReadOnly = true;
                isEditMode_SYA = false;
            }
        }

        private void SaveCsv_Click(object sender, EventArgs e)
        {
            if (table_SYA.Rows.Count == 0) return;

            using SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = Path.GetFileName(currentPath_SYA)
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dataService_SYA.SaveCsv(table_SYA, dlg.FileName);
                currentPath_SYA = dlg.FileName;
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            if (!File.Exists(currentPath_SYA)) return;

            table_SYA = dataService_SYA.LoadCsv(currentPath_SYA);
            table_SYA.DefaultView.RowFilter = string.Empty;
            dataGridViewEmployees_SYA.DataSource = table_SYA;
        }

        // ====== CRUD ======

        private void Add_Click(object sender, EventArgs e)
        {
            if (table_SYA.Columns.Count == 0) return;

            table_SYA.Rows.Add(table_SYA.NewRow());
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            if (table_SYA.Rows.Count == 0) return;

            isEditMode_SYA = !isEditMode_SYA;
            dataGridViewEmployees_SYA.ReadOnly = !isEditMode_SYA;

            MessageBox.Show(
                isEditMode_SYA ? "Режим редактирования включён" : "Режим редактирования выключён",
                "Редактирование",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewEmployees_SYA.SelectedRows)
            {
                if (row.IsNewRow) continue;

                if (row.DataBoundItem is DataRowView view)
                    view.Row.Delete();
            }
        }

        // ====== SEARCH / FILTER ======

        private void Search_Click(object sender, EventArgs e)
        {
            if (table_SYA.Rows.Count == 0) return;

            string value = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите фамилию:",
                "Поиск по фамилии",
                "");

            if (string.IsNullOrWhiteSpace(value))
                table_SYA.DefaultView.RowFilter = string.Empty;
            else
                table_SYA.DefaultView.RowFilter = $"Фамилия LIKE '%{value}%'";
        }

        // ====== INFO ======

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Отдел кадров\nВариант 11\nШеломенцев Я.А.\nТИУ, 2025",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void UserGuide_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "1. Откройте CSV\n" +
                "2. Используйте Поиск / Изменение\n" +
                "3. Сохраните изменения",
                "Руководство пользователя",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Statistics_Click(object sender, EventArgs e)
        {
            if (table_SYA.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Нет данных для расчёта статистики.",
                    "Статистика",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var stats = dataService_SYA.GetSalaryStatistics(table_SYA);

            MessageBox.Show(
                $"Статистика по окладам:\n\n" +
                $"Количество сотрудников: {stats.count}\n" +
                $"Минимальный оклад: {stats.min:F2}\n" +
                $"Максимальный оклад: {stats.max:F2}\n" +
                $"Средний оклад: {stats.avg:F2}",
                "Статистика",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        private void Chart_Click(object sender, EventArgs e)
        {
            if (table_SYA.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Нет данных для построения графика.",
                    "График",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using FormChart_SYA form = new FormChart_SYA(table_SYA);
            form.ShowDialog();
        }

    }
}
