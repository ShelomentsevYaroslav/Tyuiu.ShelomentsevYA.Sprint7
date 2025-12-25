using System.Windows.Forms;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    partial class FormFilter_SYA
    {
        private Panel panelFilters;
        private Button buttonApply;
        private Button buttonReset;

        private void InitializeComponent()
        {
            panelFilters = new Panel();
            buttonApply = new Button();
            buttonReset = new Button();

            SuspendLayout();

            Text = "Фильтр";
            ClientSize = new System.Drawing.Size(400, 450);
            StartPosition = FormStartPosition.CenterParent;

            panelFilters.Dock = DockStyle.Top;
            panelFilters.Height = 360;
            panelFilters.AutoScroll = true;

            buttonApply.Text = "Применить";
            buttonApply.SetBounds(90, 380, 100, 30);
            buttonApply.Click += buttonApply_Click;

            buttonReset.Text = "Сбросить";
            buttonReset.SetBounds(210, 380, 100, 30);
            buttonReset.Click += buttonReset_Click;

            Controls.Add(panelFilters);
            Controls.Add(buttonApply);
            Controls.Add(buttonReset);

            ResumeLayout(false);
        }
    }
}
