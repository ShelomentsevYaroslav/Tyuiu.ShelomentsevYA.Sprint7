namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    partial class FormUserGuide_SYA
    {
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.RichTextBox richTextBoxGuide_SYA;
        private System.Windows.Forms.Button buttonClose_SYA;

        private void InitializeComponent()
        {
            richTextBoxGuide_SYA = new();
            buttonClose_SYA = new();

            SuspendLayout();

            richTextBoxGuide_SYA.Dock = DockStyle.Fill;
            richTextBoxGuide_SYA.ReadOnly = true;
            richTextBoxGuide_SYA.Font = new System.Drawing.Font("Segoe UI", 10F);

            buttonClose_SYA.Text = "Закрыть";
            buttonClose_SYA.Dock = DockStyle.Bottom;
            buttonClose_SYA.Height = 40;
            buttonClose_SYA.Click += (s, e) => Close();

            Controls.Add(richTextBoxGuide_SYA);
            Controls.Add(buttonClose_SYA);

            Text = "Руководство пользователя";
            ClientSize = new System.Drawing.Size(700, 500);
            StartPosition = FormStartPosition.CenterScreen;

            ResumeLayout(false);
        }
    }
}
