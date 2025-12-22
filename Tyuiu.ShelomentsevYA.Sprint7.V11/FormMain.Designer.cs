namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.MenuStrip menuStripMain_SYA;
        private System.Windows.Forms.ToolStrip toolStripMain_SYA;
        private System.Windows.Forms.DataGridView dataGridViewEmployees_SYA;
        private System.Windows.Forms.ToolTip toolTipMain_SYA;

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenCsv_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveCsv_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUserGuide_SYA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout_SYA;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            menuStripMain_SYA = new MenuStrip();
            toolStripMain_SYA = new ToolStrip();
            dataGridViewEmployees_SYA = new DataGridView();
            toolTipMain_SYA = new ToolTip(components);

            toolStripMenuItemFile_SYA = new ToolStripMenuItem();
            toolStripMenuItemOpenCsv_SYA = new ToolStripMenuItem();
            toolStripMenuItemSaveCsv_SYA = new ToolStripMenuItem();
            toolStripMenuItemExit_SYA = new ToolStripMenuItem();
            toolStripMenuItemHelp_SYA = new ToolStripMenuItem();
            toolStripMenuItemUserGuide_SYA = new ToolStripMenuItem();
            toolStripMenuItemAbout_SYA = new ToolStripMenuItem();

            ((System.ComponentModel.ISupportInitialize)dataGridViewEmployees_SYA).BeginInit();
            SuspendLayout();

            // ===== MENU =====
            menuStripMain_SYA.Items.AddRange(new ToolStripItem[]
            {
                toolStripMenuItemFile_SYA,
                toolStripMenuItemHelp_SYA
            });

            toolStripMenuItemFile_SYA.Text = "Файл";
            toolStripMenuItemFile_SYA.DropDownItems.AddRange(new ToolStripItem[]
            {
                toolStripMenuItemOpenCsv_SYA,
                toolStripMenuItemSaveCsv_SYA,
                new ToolStripSeparator(),
                toolStripMenuItemExit_SYA
            });

            toolStripMenuItemOpenCsv_SYA.Text = "Открыть CSV";
            toolStripMenuItemOpenCsv_SYA.Click += OpenCsv_Click;

            toolStripMenuItemSaveCsv_SYA.Text = "Сохранить CSV";
            toolStripMenuItemSaveCsv_SYA.Click += SaveCsv_Click;

            toolStripMenuItemExit_SYA.Text = "Выход";
            toolStripMenuItemExit_SYA.Click += Exit_Click;

            toolStripMenuItemHelp_SYA.Text = "Справка";
            toolStripMenuItemHelp_SYA.DropDownItems.AddRange(new ToolStripItem[]
            {
                toolStripMenuItemUserGuide_SYA,
                toolStripMenuItemAbout_SYA
            });

            toolStripMenuItemUserGuide_SYA.Text = "Руководство пользователя";
            toolStripMenuItemUserGuide_SYA.Click += UserGuide_Click;

            toolStripMenuItemAbout_SYA.Text = "О программе";
            toolStripMenuItemAbout_SYA.Click += About_Click;

            // ===== TOOLSTRIP BUTTONS =====
            var buttonSearch_SYA = new ToolStripButton("Поиск");
            buttonSearch_SYA.Click += Search_Click;

            var buttonAdd_SYA = new ToolStripButton("Добавить");
            buttonAdd_SYA.Click += Add_Click;

            var buttonEdit_SYA = new ToolStripButton("Изменить");
            buttonEdit_SYA.Click += Edit_Click;

            var buttonDelete_SYA = new ToolStripButton("Удалить");
            buttonDelete_SYA.Click += Delete_Click;

            var buttonStatistics_SYA = new ToolStripButton("Статистика");
            buttonStatistics_SYA.Click += Statistics_Click;

            var buttonChart_SYA = new ToolStripButton("График");
            buttonChart_SYA.Click += Chart_Click;

            var buttonRefresh_SYA = new ToolStripButton("Обновить");
            buttonRefresh_SYA.Click += Refresh_Click;

            toolStripMain_SYA.Items.AddRange(new ToolStripItem[]
            {
                buttonSearch_SYA,
                new ToolStripSeparator(),
                buttonAdd_SYA,
                buttonEdit_SYA,
                buttonDelete_SYA,
                new ToolStripSeparator(),
                buttonStatistics_SYA,
                buttonChart_SYA,
                new ToolStripSeparator(),
                buttonRefresh_SYA
            });

            // ===== GRID =====
            dataGridViewEmployees_SYA.Dock = DockStyle.Fill;
            dataGridViewEmployees_SYA.ReadOnly = true;
            dataGridViewEmployees_SYA.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewEmployees_SYA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewEmployees_SYA.RowHeadersVisible = false;

            // ===== FORM =====
            Controls.Add(dataGridViewEmployees_SYA);
            Controls.Add(toolStripMain_SYA);
            Controls.Add(menuStripMain_SYA);

            MainMenuStrip = menuStripMain_SYA;
            Text = "Отдел кадров — Вариант 11";
            ClientSize = new System.Drawing.Size(1000, 600);
            StartPosition = FormStartPosition.CenterScreen;

            ((System.ComponentModel.ISupportInitialize)dataGridViewEmployees_SYA).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
