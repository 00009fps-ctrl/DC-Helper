using System;
using System.Drawing;
using System.Windows.Forms;

namespace DC_Button_Finder
{
    public class TestForm : Form
    {
        private Button btnTest;
        private Label lblTest;

        public TestForm()
        {
            InitializeComponent();

            // Принудительное создание дескриптора
            _ = this.Handle;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 300);
            this.Text = "Тестовая форма";
            this.BackColor = Color.LightBlue;
        }

        private void InitializeComponent()
        {
            btnTest = new Button();
            lblTest = new Label();

            // btnTest
            btnTest.Text = "Нажми меня";
            btnTest.Location = new Point(150, 150);
            btnTest.Size = new Size(100, 30);
            btnTest.Click += (s, e) => MessageBox.Show("Работает!");

            // lblTest
            lblTest.Text = "Тестовая форма для диагностики";
            lblTest.Location = new Point(100, 50);
            lblTest.Size = new Size(200, 30);
            lblTest.Font = new Font("Arial", 10, FontStyle.Bold);

            // TestForm
            this.Controls.Add(btnTest);
            this.Controls.Add(lblTest);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Activate();
            this.BringToFront();
        }
    }
}