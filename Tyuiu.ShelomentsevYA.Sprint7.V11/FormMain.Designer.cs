namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.MenuStrip menuStripMain_SYA;
        private System.Windows.Forms.ToolStrip toolStripMain_SYA;
        private System.Windows.Forms.DataGridView dataGridViewEmployees_SYA;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxTables_SYA;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSearch_SYA;

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenCsv_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenFolder_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveCsv_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit_SYA;

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUserGuide_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout_SYA;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            menuStripMain_SYA = new();
            toolStripMain_SYA = new();
            dataGridViewEmployees_SYA = new();
            toolStripComboBoxTables_SYA = new();
            toolStripTextBoxSearch_SYA = new();
            toolStripComboBoxTables_SYA.SelectedIndexChanged += Tables_SelectedIndexChanged;


            toolStripMenuItemFile_SYA = new();
            toolStripMenuItemOpenCsv_SYA = new();
            toolStripMenuItemOpenFolder_SYA = new();
            toolStripMenuItemSaveCsv_SYA = new();
            toolStripMenuItemExit_SYA = new();

            toolStripMenuItemHelp_SYA = new();
            toolStripMenuItemUserGuide_SYA = new();
            toolStripMenuItemAbout_SYA = new();

            // ===== MENU =====
            menuStripMain_SYA.Items.AddRange(new[]
            {
                toolStripMenuItemFile_SYA,
                toolStripMenuItemHelp_SYA
            });

            toolStripMenuItemFile_SYA.Text = "Файл";
            toolStripMenuItemFile_SYA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                toolStripMenuItemOpenCsv_SYA,
                toolStripMenuItemOpenFolder_SYA,
                toolStripMenuItemSaveCsv_SYA,
                new System.Windows.Forms.ToolStripSeparator(),
                toolStripMenuItemExit_SYA
            });

            toolStripMenuItemOpenCsv_SYA.Text = "Открыть CSV";
            toolStripMenuItemOpenCsv_SYA.Click += OpenCsv_Click;

            toolStripMenuItemOpenFolder_SYA.Text = "Открыть папку CSV";
            toolStripMenuItemOpenFolder_SYA.Click += OpenFolder_Click;

            toolStripMenuItemSaveCsv_SYA.Text = "Сохранить CSV";
            toolStripMenuItemSaveCsv_SYA.Click += SaveCsv_Click;

            toolStripMenuItemExit_SYA.Text = "Выход";
            toolStripMenuItemExit_SYA.Click += Exit_Click;

            toolStripMenuItemHelp_SYA.Text = "Справка";
            toolStripMenuItemHelp_SYA.DropDownItems.AddRange(new[]
            {
                toolStripMenuItemUserGuide_SYA,
                toolStripMenuItemAbout_SYA
            });

            toolStripMenuItemUserGuide_SYA.Text = "Руководство пользователя";
            toolStripMenuItemUserGuide_SYA.Click += UserGuide_Click;

            toolStripMenuItemAbout_SYA.Text = "О программе";
            toolStripMenuItemAbout_SYA.Click += About_Click;

            // ===== TOOLSTRIP =====
            toolStripMain_SYA.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripMain_SYA.AutoSize = false;
            toolStripMain_SYA.Height = 46;

            System.Windows.Forms.ToolStripButton Btn(string text, System.EventHandler h)
            {
                var b = new System.Windows.Forms.ToolStripButton(text);
                b.AutoSize = false;
                b.Size = new System.Drawing.Size(100, 32);
                b.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
                b.Click += h;
                return b;
            }

            toolStripTextBoxSearch_SYA.AutoSize = false;
            toolStripTextBoxSearch_SYA.Width = 220;
            toolStripTextBoxSearch_SYA.TextChanged += Search_TextChanged;

            var clearBtn = Btn("✕", ClearSearch_Click);
            clearBtn.Size = new System.Drawing.Size(40, 32);

            toolStripMain_SYA.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                toolStripComboBoxTables_SYA,
                new System.Windows.Forms.ToolStripSeparator(),
                Btn("Добавить", Add_Click),
                Btn("Изменить", Edit_Click),
                Btn("Удалить", Delete_Click),
                new System.Windows.Forms.ToolStripSeparator(),
                Btn("Статистика", Statistics_Click),
                Btn("График", Chart_Click),
                new System.Windows.Forms.ToolStripSeparator(),
                Btn("Обновить", Refresh_Click),
                new System.Windows.Forms.ToolStripSeparator(),
                new System.Windows.Forms.ToolStripLabel("Поиск:"),
                toolStripTextBoxSearch_SYA,
                clearBtn
            });

            // ===== GRID =====
            dataGridViewEmployees_SYA.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewEmployees_SYA.ReadOnly = true;
            dataGridViewEmployees_SYA.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridViewEmployees_SYA.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewEmployees_SYA.RowHeadersVisible = false;

            // ===== FORM =====
            Controls.Add(dataGridViewEmployees_SYA);
            Controls.Add(toolStripMain_SYA);
            Controls.Add(menuStripMain_SYA);

            MainMenuStrip = menuStripMain_SYA;
            ClientSize = new System.Drawing.Size(1000, 600);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Отдел кадров — Вариант 11";
        }
    }
}
