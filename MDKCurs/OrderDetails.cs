using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MDKCurs
{
    public partial class OrderDetails : Form
    {
        private string connectionString = @"Data Source=ADCLG1;Initial catalog=SKLAD;Integrated Security=True";
        private SqlConnection connection;

        public OrderDetails()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadOrderDetails();
            StyleForm();
        }

        private void LoadOrderDetails()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Admin")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }


            string query = "SELECT ID, OrderID, ProductID, Quantity, Amount FROM OrderDetails";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int orderID = Convert.ToInt32(textBox1.Text);
            int productID = Convert.ToInt32(textBox2.Text);
            int quantity = Convert.ToInt32(textBox3.Text);
            decimal amount = Convert.ToDecimal(textBox4.Text);

            string query = "INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Amount) VALUES (@OrderID, @ProductID, @Quantity, @Amount)";
            ExecuteNonQuery(query, ("@OrderID", orderID), ("@ProductID", productID), ("@Quantity", quantity), ("@Amount", amount));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int detailID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                int orderID = Convert.ToInt32(textBox1.Text);
                int productID = Convert.ToInt32(textBox2.Text);
                int quantity = Convert.ToInt32(textBox3.Text);
                decimal amount = Convert.ToDecimal(textBox4.Text);


                string query = "UPDATE OrderDetails SET OrderID = @OrderID, ProductID = @ProductID, Quantity = @Quantity, Amount = @Amount WHERE ID = @DetailID";
                ExecuteNonQuery(query, ("@OrderID", orderID), ("@ProductID", productID), ("@Quantity", quantity), ("@Amount", amount), ("@DetailID", detailID));
            }
            else
            {
                MessageBox.Show("Выберите деталь заказа для обновления.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int detailID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);


                string query = "DELETE FROM OrderDetails WHERE ID = @DetailID";
                ExecuteNonQuery(query, ("@DetailID", detailID));
            }
            else
            {
                MessageBox.Show("Выберите деталь заказа для удаления.");
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
                    LoadOrderDetails();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadOrderDetails();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["OrderID"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["ProductID"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["Quantity"].Value.ToString();
                textBox4.Text = dataGridView1.SelectedRows[0].Cells["Amount"].Value.ToString();
            }
        }

        private void OrderDetails_FormClosing(object sender, FormClosingEventArgs e)
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


            foreach (var button in new Button[] { button1, button2, button3, button4, button5 })
            {
                button.BackColor = Color.FromArgb(255, 153, 51);
                button.ForeColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            }


            foreach (var textBox in new TextBox[] { textBox1, textBox2, textBox3, textBox4 })
            {
                textBox.Font = new Font("Segoe UI", 12);
                textBox.BackColor = Color.WhiteSmoke;
                textBox.ForeColor = Color.Black;
            }


            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 204);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void OrderDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
