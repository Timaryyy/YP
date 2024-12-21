using System;
using System.Windows.Forms;
using System.Drawing;

namespace MDKCurs
{
    public partial class Main : Form
    {
        public static Main Instance { get; private set; }

        public Main()
        {
            InitializeComponent();
            Instance = this;
        }

        
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

   
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        private void покупателиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customers category = new Customers();
            this.Hide();
            category.ShowDialog();
        }

        private void деталиЗаказовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderDetails category = new OrderDetails();
            this.Hide();
            category.ShowDialog();
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Orders category = new Orders();
            this.Hide();
            category.ShowDialog();
        }


        private void товарыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products category = new Products();
            this.Hide();
            category.ShowDialog();
        }


        private void типыПродуктовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductTypes category = new ProductTypes();
            this.Hide();
            category.ShowDialog();
        }


        private void поставщикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Suppliers category = new Suppliers();
            this.Hide();
            category.ShowDialog();
        }


        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users category = new Users();
            this.Hide();
            category.ShowDialog();
        }


        private void Main_Load(object sender, EventArgs e)
        {

            string currentRole = UserManager.CurrentUser.Role;


            MessageBox.Show($"Текущая роль пользователя: {currentRole}");


            if (currentRole != "Admin")
            {
                пользователиToolStripMenuItem.Enabled = false;
            }


            StyleMenuStrip();
        }


        private void StyleMenuStrip()
        {

            menuStrip1.BackColor = Color.FromArgb(255, 183, 77);
            menuStrip1.ForeColor = Color.White;


            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                item.ForeColor = Color.White;
                item.BackColor = Color.FromArgb(255, 162, 49);
                item.Padding = new Padding(12, 6, 12, 6);

                item.MouseEnter += (s, e) => { item.BackColor = Color.FromArgb(255, 141, 40); }; 
                item.MouseLeave += (s, e) => { item.BackColor = Color.FromArgb(255, 162, 49); };
            }


            foreach (ToolStripMenuItem subItem in menuStrip1.Items)
            {
                subItem.DropDown.BackColor = Color.FromArgb(255, 141, 40);
                subItem.DropDown.ForeColor = Color.White;
                subItem.DropDown.Font = new Font("Segoe UI", 14);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
