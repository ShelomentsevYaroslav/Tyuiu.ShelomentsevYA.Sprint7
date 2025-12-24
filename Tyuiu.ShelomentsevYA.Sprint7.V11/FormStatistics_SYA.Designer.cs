namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    partial class FormStatistics_SYA
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox comboBoxMode_SYA;
        private System.Windows.Forms.DataGridView dataGridViewStats_SYA;
        private System.Windows.Forms.Button buttonExportCsv_SYA;
        private System.Windows.Forms.Button buttonExportTxt_SYA;
        private System.Windows.Forms.Label labelMode_SYA;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            comboBoxMode_SYA = new();
            dataGridViewStats_SYA = new();
            buttonExportCsv_SYA = new();
            buttonExportTxt_SYA = new();
            labelMode_SYA = new();

            SuspendLayout();

            // ===== LABEL =====
            labelMode_SYA.Text = "Тип статистики:";
            labelMode_SYA.Location = new System.Drawing.Point(12, 15);
            labelMode_SYA.AutoSize = true;

            // ===== COMBO MODE =====
            comboBoxMode_SYA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxMode_SYA.Items.AddRange(new object[]
            {
                "Общая статистика",
                "По подразделениям",
                "По должностям",
                "По образованию"
            });
            comboBoxMode_SYA.Location = new System.Drawing.Point(130, 12);
            comboBoxMode_SYA.Width = 260;
            comboBoxMode_SYA.SelectedIndexChanged += comboBoxMode_SYA_SelectedIndexChanged;

            // ===== BUTTONS =====
            buttonExportCsv_SYA.Text = "Экспорт CSV";
            buttonExportCsv_SYA.Location = new System.Drawing.Point(420, 10);
            buttonExportCsv_SYA.Size = new System.Drawing.Size(120, 30);
            buttonExportCsv_SYA.Click += buttonExportCsv_SYA_Click;

            buttonExportTxt_SYA.Text = "Экспорт TXT";
            buttonExportTxt_SYA.Location = new System.Drawing.Point(550, 10);
            buttonExportTxt_SYA.Size = new System.Drawing.Size(120, 30);
            buttonExportTxt_SYA.Click += buttonExportTxt_SYA_Click;

            // ===== GRID =====
            dataGridViewStats_SYA.Dock = System.Windows.Forms.DockStyle.Bottom;
            dataGridViewStats_SYA.Height = 430;
            dataGridViewStats_SYA.ReadOnly = true;
            dataGridViewStats_SYA.RowHeadersVisible = false;
            dataGridViewStats_SYA.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewStats_SYA.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // ===== FORM =====
            ClientSize = new System.Drawing.Size(800, 500);
            Controls.Add(labelMode_SYA);
            Controls.Add(comboBoxMode_SYA);
            Controls.Add(buttonExportCsv_SYA);
            Controls.Add(buttonExportTxt_SYA);
            Controls.Add(dataGridViewStats_SYA);

            Text = "Статистика";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
