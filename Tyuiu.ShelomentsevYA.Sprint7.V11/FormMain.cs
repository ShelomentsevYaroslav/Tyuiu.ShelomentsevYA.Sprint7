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
        private readonly DataService dataService_SYA = new(); // слой, отвечающий за CSV и работу с файлами
        private readonly BindingSource bindingSource_SYA = new(); // посредник между datatable и datagridviev, чтобы грид автоматически обновялся при изменении данных

        private DataTable table_SYA = new(); // основная таблица данных, хранящая содержимое CSV в памяти
        private readonly Dictionary<string, string> csvFiles_SYA = new(); // словать отображаемого имени пути к CSV, нужен для переключения между файлами
        private readonly Dictionary<string, bool> sortAscending_SYA = new(); // словарь направления сортировки по кажжому столбцу 

        private string currentPath_SYA = string.Empty; // путь к текущему открытому csv
        private bool isEditMode_SYA = false; // флажок режима редактирования

        public FormMain()
        {
            InitializeComponent(); // создание элементов интерфейса 

            dataGridViewEmployees_SYA.AutoGenerateColumns = true; // грид сам создает колонки на основе дататейбл
            dataGridViewEmployees_SYA.DataSource = bindingSource_SYA; // привязка к изменению данныъ

            dataGridViewEmployees_SYA.ColumnHeaderMouseClick +=
                DataGridViewEmployees_SYA_ColumnHeaderMouseClick; // подписка грида на событие клика по заголовку столбца, винформ вызывает метод сортировки 

            dataGridViewEmployees_SYA.DataBindingComplete +=
                DataGridViewEmployees_SYA_DataBindingComplete;

            ConfigureToolStrip(); // внешний вид панели кнопок
            ApplyIcons(); // назначение иконок кнопкам и меню


            Text = "Отдел кадров — Вариант 11";
            MinimumSize = new Size(1000, 600);
        }


        // ================= TOOLSTRIP =================

        private void ConfigureToolStrip()
        {
            toolStripMain_SYA.GripStyle = ToolStripGripStyle.Hidden; // скрываем ручку для перетаскивания
            toolStripMain_SYA.AutoSize = false; // запрет авторазмера и ручной контроль
            toolStripMain_SYA.Height = 48;
            toolStripMain_SYA.Padding = new Padding(4, 4, 4, 4);
            toolStripMain_SYA.ImageScalingSize = new Size(24, 24); 


            // проходка по всем элементам панели
            foreach (ToolStripItem item in toolStripMain_SYA.Items)
            {
                if (item is not ToolStripButton btn) // только кнопки
                    continue;

                btn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText; // отображаем текст и картинку
                btn.TextImageRelation = TextImageRelation.ImageBeforeText; // иконки до текста

                btn.ImageAlign = ContentAlignment.MiddleLeft; // просто выравнивание текста и иконок
                btn.TextAlign = ContentAlignment.MiddleCenter;

                btn.AutoSize = false; // фиксированный размер кнопок
                btn.Size = new Size(120, 38);
                btn.Margin = new Padding(2, 1, 2, 1);
                btn.Padding = new Padding(6, 0, 6, 0);

                btn.ToolTipText = btn.Text switch
                {
                    "Добавить" => "Добавить новую строку в таблицу",
                    "Изменить" => "Включить или выключить режим редактирования",
                    "Удалить" => "Удалить выбранные строки",
                    "Статистика" => "Показать статистику по окладам",
                    "График" => "Построить диаграмму по данным",
                    "Обновить" => "Перезагрузить текущий CSV-файл",
                    "Фильтр" => "Открыть расширенную фильтрацию",
                    _ => btn.Text
                };


                // отключение автомасштабирование картинок системой 
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
                // в зависсимости от текста кнопки подбираем иконку
                btn.Image = btn.Text switch
                {
                    "Добавить" => LoadIcon("add.png"),
                    "Изменить" => LoadIcon("pencil.png"),
                    "Удалить" => LoadIcon("delete.png"),
                    "Статистика" => LoadIcon("chart_bar.png"),
                    "График" => LoadIcon("chart_line.png"),
                    "Обновить" => LoadIcon("arrow_refresh.png"),
                    "Фильтр" => LoadIcon("magnifier.png"),
                    _ => null
                };
            }
            // иконки для пунктов в меню
            toolStripMenuItemOpenCsv_SYA.Image = LoadIcon("page_white_excel.png");
            toolStripMenuItemOpenFolder_SYA.Image = LoadIcon("folder_table.png");
            toolStripMenuItemSaveCsv_SYA.Image = LoadIcon("disk.png");
            toolStripMenuItemExit_SYA.Image = LoadIcon("door_out.png");

            toolStripMenuItemUserGuide_SYA.Image = LoadIcon("help.png");
            toolStripMenuItemAbout_SYA.Image = LoadIcon("information.png");
        }

        private Image LoadIcon(string name)
        {
            string path = Path.Combine( // здесь формируем путь папка запуска + иконки + имя файлы
                AppDomain.CurrentDomain.BaseDirectory,
                "Icons",
                name
            );
            // если файл есть - загружаем картинку, иначе 0
            return File.Exists(path) ? Image.FromFile(path) : null;
        }

        // ================= FILE =================

        private void OpenCsv_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new() // диалог выбора файлы
            {
                Filter = "CSV files (*.csv)|*.csv"
            };

            if (dlg.ShowDialog() != DialogResult.OK) // отмена
                return;

            csvFiles_SYA.Clear(); // очистка предыдущих данных
            toolStripComboBoxTables_SYA.Items.Clear();

            string path = dlg.FileName; // путь к выбранному файлу
            string key = Path.GetFileNameWithoutExtension(path); // имя файла без расширения - ключ для комбобокс

            csvFiles_SYA[key] = path; // сохраняем соответствие имя -> путь
            toolStripComboBoxTables_SYA.Items.Add(key); // добавляем комбобокс
            toolStripComboBoxTables_SYA.SelectedIndex = 0;
            // загружаем таблицу
            LoadTable(path);
        }

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dlg = new(); // диалог выбора папки
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            csvFiles_SYA.Clear(); // очищаем старые данные - словарь и комбобокс
            toolStripComboBoxTables_SYA.Items.Clear();
            // проходим и получаем все csv фалы в выбранной папке
            foreach (var file in Directory.GetFiles(dlg.SelectedPath, "*.csv"))
            {
                string key = Path.GetFileNameWithoutExtension(file); // имя файла которое будет показано пользователю
                csvFiles_SYA[key] = file; // соответствие имя -> полный путь
                toolStripComboBoxTables_SYA.Items.Add(key); // добавляем имя файла в комбобокс
            }

            if (toolStripComboBoxTables_SYA.Items.Count > 0) // проверка на наличие файлов (автоматически выбираем первый если хотя бы один)
                toolStripComboBoxTables_SYA.SelectedIndex = 0;
        }

        private void Tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBoxTables_SYA.SelectedItem is not string key) // проверка что выбранный элемент строка, т.к комбобокс может быть пустым 
                return;

            if (csvFiles_SYA.TryGetValue(key, out var path)) // по имени получаемся получить путь 
                LoadTable(path); // загружаем csv в datatable
        }

        private void SaveCsv_Click(object sender, EventArgs e)
        {
            if (table_SYA.Rows.Count == 0) // таблица пустая - ничего не сохраняем
                return;

            using SaveFileDialog dlg = new() // диалог сохранения файла
            {
                Filter = "CSV files (*.csv)|*.csv", // разрешаем сохранять только csv
                FileName = Path.GetFileName(currentPath_SYA) // подставляем имя текущего файла по умолчанию
            };

            if (dlg.ShowDialog() != DialogResult.OK) // отмена 
                return;

            dataService_SYA.SaveCsv(table_SYA, dlg.FileName); // сохпаеяем таблицу через слой датасервис
            currentPath_SYA = dlg.FileName; // обновляем путь текущего файла
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            if (File.Exists(currentPath_SYA)) // проверка, если файл существует - перечитываем его с диска
                LoadTable(currentPath_SYA);
        }

        // ================= LOAD =================

        private void LoadTable(string path)
        {
            UseWaitCursor = true; // курсор ожидания 

            try
            {
                table_SYA = SafeLoadCsv(path); // загружаем csv в new datatable
                bindingSource_SYA.DataSource = table_SYA; // привязка таблица и автоматическое обновление грида

                currentPath_SYA = path; // запоминаем путь 
                isEditMode_SYA = false; // сбрасываем решим редактирования
                dataGridViewEmployees_SYA.ReadOnly = true;
            }
            finally // выполнится всегда, даже если файл кривой
            {
                UpdateFeatureAvailability(); // обновляем доступность кнопок и возвращаем курсор 
                UseWaitCursor = false;
            }
        }

        private DataTable SafeLoadCsv(string path)
        {
            var table = new DataTable(); // пустая таблица
            var lines = File.ReadAllLines(path); // считаем все строки файла

            if (lines.Length == 0) // файл пуст - возвращаем пустоту
                return table;

            var headers = lines[0].Split(';'); // первая строка - заголовки колонок
            foreach (var h in headers) // добавялем колонки в дататейбл
                table.Columns.Add(h.Trim());
            // обрабатываем строки с данными
            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(';');
                var row = table.NewRow(); // создаем новую строку таблицы
                // заполняем ячейки
                for (int c = 0; c < table.Columns.Count; c++)
                    row[c] = c < values.Length ? values[c] : DBNull.Value;
                // добавляем строку в таблицу
                table.Rows.Add(row);
            }

            return table;
        }

        // ================= CRUD =================

        private void Add_Click(object sender, EventArgs e)
        {
            table_SYA.Rows.Add(table_SYA.NewRow()); // добавляем пустую строку
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            isEditMode_SYA = !isEditMode_SYA; // переключаем режим редактирования
            dataGridViewEmployees_SYA.ReadOnly = !isEditMode_SYA; // разрешаем или запрещаем ввод в таблице
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewEmployees_SYA.SelectedRows) // проходим по выбранным строкам грида
                if (row.DataBoundItem is DataRowView v) // проверка типа, защита от null, безопасное приведение типа. грид привязан к тейбл через биндингсурс, для корректного удаления датаров и через датароввию - делит
                    v.Row.Delete(); // помечаем строку удаленной 
        }

        // ================= SEARCH =================

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (table_SYA == null || table_SYA.Columns.Count == 0) // таблицы нет искать нечего
                return;

            if (sender is not ToolStripTextBox tb) // проверка что исчтоник текстбокс
                return;

            string value = tb.Text;
            // строка пустая - сбрасываем фильтр
            if (string.IsNullOrWhiteSpace(value))
            {
                table_SYA.DefaultView.RowFilter = string.Empty;
                return;
            }
            // экранирование - защита от падения rowfilter
            value = value
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("]", "[]]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
            // формирование фильтра, поиска по текстовым колонкам
            var filters = table_SYA.Columns
                .Cast<DataColumn>()
                .Where(c => c.DataType == typeof(string))
                .Select(c => $"[{c.ColumnName}] LIKE '%{value}%'")
                .ToList();
            // фильтров нет - возвращаем по умолчанию
            if (filters.Count == 0)
            {
                table_SYA.DefaultView.RowFilter = string.Empty;
                return;
            }
            // применение фильтра
            try
            {
                table_SYA.DefaultView.RowFilter = string.Join(" OR ", filters);
            }
            catch
            {
                table_SYA.DefaultView.RowFilter = string.Empty;
            }
        }
        private void Filter_Click(object sender, EventArgs e)
        {
            using var f = new FormFilter_SYA(table_SYA); // создаем форму расширенного фильтра, передаем в тейбл
            if (f.ShowDialog() == DialogResult.OK) // открываем форму модально, код ниже выполняет после её закрытия
            {
                table_SYA.DefaultView.RowFilter = f.ResultFilter; // получаем из форму строку ровфильтр и применяем её к дефолтвью таблицы
            }

        }

        private void ClearSearch_Click(object sender, EventArgs e)
        {
            toolStripTextBoxSearch_SYA.Text = string.Empty; // очищаем поле
            table_SYA.DefaultView.RowFilter = string.Empty; // сбрасываем фильтрацию
        }

        // ================= INFO =================

        private void Statistics_Click(object sender, EventArgs e)
        {
            if (!CanBuildStatistics(out var reason)) // проверяем можно ли строить статистику, если нельзя показываем причину
            {
                MessageBox.Show(reason, "Статистика");
                return;
            }
            // создаем форму статистику, передаём в тейбл тольок для чтения
            using var form = new FormStatistics_SYA(table_SYA);
            form.ShowDialog(); // показываем модально
        }

        private void Chart_Click(object sender, EventArgs e)
        {
            if (!CanBuildStatistics(out var reason)) // использует ту же проверку что и для статистики
            {
                MessageBox.Show(reason, "Диаграмма");
                return;
            }
             
            using var form = new FormChart_SYA(table_SYA); // создаем форму диаграмм
            form.ShowDialog();
        }

        private void About_Click(object sender, EventArgs e)
        {
            using var f = new FormAbout_SYA(); // информационная форма, не требует данных
            f.ShowDialog();
        }

        private void UserGuide_Click(object sender, EventArgs e)
        {
            using var f = new FormUserGuide_SYA(); // форма со справкой
            f.ShowDialog();
        }


        private void Exit_Click(object sender, EventArgs e) => Close(); // закрывает главную форму

        // ================= HELPERS =================

        private void UpdateFeatureAvailability() // включение или отключение кнопок
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

        private bool CanBuildStatistics(out string reason) // единая проверка данных
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

        // ================= SORTING =================

        private void DataGridViewEmployees_SYA_ColumnHeaderMouseClick(
            object sender,
            DataGridViewCellMouseEventArgs e)
        {
            if (table_SYA == null || table_SYA.Rows.Count == 0) // если таблицы нет или она пустая - выходим
                return;

            string columnName =
                dataGridViewEmployees_SYA.Columns[e.ColumnIndex].Name; // определяем имя столбца по индексу

            bool asc = !sortAscending_SYA.ContainsKey(columnName) // если столбец сортируется впервые - asc или если сортировался - меняем направление
                       || !sortAscending_SYA[columnName];

            sortAscending_SYA[columnName] = asc; // сохраняем направление и формируем строку 
            string direction = asc ? "ASC" : "DESC";

            if (IsNumericColumn(table_SYA, columnName)) // числовая или текстовая сортировка 
            {
                string sortColumn = EnsureNumericSortColumn(columnName); //скрытая колонка
                table_SYA.DefaultView.Sort = $"[{sortColumn}] {direction}";
            }
            else
            {
                table_SYA.DefaultView.Sort = $"[{columnName}] {direction}";
            }

            UpdateSortGlyph(columnName, asc);
        }


        private bool IsNumericColumn(DataTable table, string columnName) // проверка числовая ли колонка
        {
            bool hasAnyValue = false;

            foreach (DataRow row in table.Rows)
            {
                var value = row[columnName];
                if (value == null || value == DBNull.Value)
                    continue;

                string s = value.ToString()?.Trim();
                if (string.IsNullOrEmpty(s))
                    continue;

                hasAnyValue = true;

                if (!double.TryParse(s, out _))
                    return false;
            }

            return hasAnyValue;
        }


        private string EnsureNumericSortColumn(string columnName) // скрытая колонка для сортировка
        {
            string numericColumnName = $"__NUM_{columnName}";

            if (!table_SYA.Columns.Contains(numericColumnName))
            {
                table_SYA.Columns.Add(numericColumnName, typeof(double));
            }

            foreach (DataRow row in table_SYA.Rows)
            {
                var value = row[columnName];

                if (value == null || value == DBNull.Value)
                {
                    row[numericColumnName] = double.MinValue;
                    continue;
                }

                string s = value.ToString()?.Trim();

                if (double.TryParse(s, out double number))
                    row[numericColumnName] = number;
                else
                    row[numericColumnName] = double.MinValue;
            }

            return numericColumnName;
        }


        private void UpdateSortGlyph(string sortedColumn, bool asc) // стрелочки сортировки
        {
            foreach (DataGridViewColumn col in dataGridViewEmployees_SYA.Columns)
            {
                col.HeaderCell.SortGlyphDirection =
                    col.Name == sortedColumn
                        ? (asc ? SortOrder.Ascending : SortOrder.Descending)
                        : SortOrder.None;
            }
        }

        private void DataGridViewEmployees_SYA_DataBindingComplete(
    object sender,
    DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn col in dataGridViewEmployees_SYA.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic; // отключаем стандартную сортировку

                if (col.Name.StartsWith("__NUM_")) // скрываем служебные числовые колонки
                {
                    col.Visible = false;
                }
            }
        }
    }
}
