using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MDKCurs
{
    public partial class Customers : Form
    {
        private string connectionString = @"Data Source=ADCLG1;Initial catalog=SKLAD;Integrated Security=True";
        private SqlConnection connection;

        public Customers()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadCustomers();
            StyleForm();
        }

        private void LoadCustomers()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Admin")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }
            else
            {
                button1.Show();
                button2.Show();
                button3.Show();
            }

            string query = "SELECT Customer_ID AS ID, Name, Address, Phone FROM Customers";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["Name"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["Address"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["Phone"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string address = textBox2.Text;
            string phone = textBox3.Text;

            string query = "INSERT INTO Customers (Name, Address, Phone) VALUES (@Name, @Address, @Phone)";
            ExecuteNonQuery(query, ("@Name", name), ("@Address", address), ("@Phone", phone));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int customerID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                string name = textBox1.Text;
                string address = textBox2.Text;
                string phone = textBox3.Text;

                string query = "UPDATE Customers SET Name = @Name, Address = @Address, Phone = @Phone WHERE Customer_ID = @CustomerID";
                ExecuteNonQuery(query, ("@Name", name), ("@Address", address), ("@Phone", phone), ("@CustomerID", customerID));
            }
            else
            {
                MessageBox.Show("Выберите клиента для обновления.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int customerID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                string query = "DELETE FROM Customers WHERE Customer_ID = @CustomerID";
                ExecuteNonQuery(query, ("@CustomerID", customerID));
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления.");
            }
        }

        private void ExecuteNonQuery(string query, params (string, object)[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                }

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Операция успешно выполнена.");
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadCustomers();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void Reviews_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Main.Instance.Show();
        }

        private void StyleForm()
        {

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 204, 153);

            // Стилизуем кнопки
            foreach (var button in new Button[] { button1, button2, button3, button4, button5 })
            {
                button.BackColor = Color.FromArgb(255, 153, 51);
                button.ForeColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            }

            // Стилизуем текстовые поля
            foreach (var textBox in new TextBox[] { textBox1, textBox2, textBox3 })
            {
                textBox.Font = new Font("Segoe UI", 12);
                textBox.BackColor = Color.WhiteSmoke;
                textBox.ForeColor = Color.Black;
            }

            // Стилизуем DataGridView
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 204);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }
    }
}