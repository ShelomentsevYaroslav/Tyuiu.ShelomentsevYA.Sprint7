namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    partial class FormAbout_SYA
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.PictureBox pictureBoxPhoto_SYA;
        private System.Windows.Forms.Label labelInfo_SYA;
        private System.Windows.Forms.Button buttonClose_SYA;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tableLayoutPanelMain = new();
            pictureBoxPhoto_SYA = new();
            labelInfo_SYA = new();
            buttonClose_SYA = new();

            SuspendLayout();

            // ===== TABLE LAYOUT =====
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.ColumnCount = 2;
            tableLayoutPanelMain.RowCount = 2;

            tableLayoutPanelMain.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200));
            tableLayoutPanelMain.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            tableLayoutPanelMain.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50));

            // ===== PHOTO =====
            pictureBoxPhoto_SYA.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxPhoto_SYA.Margin = new System.Windows.Forms.Padding(15);
            pictureBoxPhoto_SYA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBoxPhoto_SYA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // ===== TEXT =====
            labelInfo_SYA.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInfo_SYA.Font = new System.Drawing.Font("Segoe UI", 10F);
            labelInfo_SYA.Padding = new System.Windows.Forms.Padding(10);
            labelInfo_SYA.AutoSize = false;
            labelInfo_SYA.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            labelInfo_SYA.Text =
@"Отдел кадров — Вариант 11

Автор:
Шеломенцев Ярослав Александрович

Направление:
Информационные системы и технологии
в геологии и нефтегазовой отрасли

Учебное заведение:
Тюменский индустриальный университет

Год:
2025";

            // ===== BUTTON =====
            buttonClose_SYA.Text = "Закрыть";
            buttonClose_SYA.Dock = System.Windows.Forms.DockStyle.Right;
            buttonClose_SYA.Width = 120;
            buttonClose_SYA.Margin = new System.Windows.Forms.Padding(10);
            buttonClose_SYA.Click += (s, e) => Close();

            // ===== ADD CONTROLS =====
            tableLayoutPanelMain.Controls.Add(pictureBoxPhoto_SYA, 0, 0);
            tableLayoutPanelMain.Controls.Add(labelInfo_SYA, 1, 0);
            tableLayoutPanelMain.Controls.Add(buttonClose_SYA, 1, 1);

            Controls.Add(tableLayoutPanelMain);

            // ===== FORM =====
            Text = "О программе";
            ClientSize = new System.Drawing.Size(700, 350);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            MinimumSize = new System.Drawing.Size(600, 300);

            ResumeLayout(false);
        }
    }
}
