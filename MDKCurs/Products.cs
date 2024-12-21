using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MDKCurs
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadProducts();
            StyleForm(); // Применяем стили к форме
        }

        private string connectionString = @"Data Source=ADCLG1;Initial catalog=SKLAD;Integrated Security=True";
        private SqlConnection connection;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["Name"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["Description"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["Price"].Value.ToString();
                textBox4.Text = dataGridView1.SelectedRows[0].Cells["Quantity"].Value.ToString();
                textBox5.Text = dataGridView1.SelectedRows[0].Cells["ProductTypeID"].Value.ToString();
            }
        }

        private void LoadProducts()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Admin")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT ID, Name, Description, Price, Quantity, ProductTypeID FROM Products";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string description = textBox2.Text;
            decimal price = Convert.ToDecimal(textBox3.Text);
            int quantity = Convert.ToInt32(textBox4.Text);
            int productTypeID = Convert.ToInt32(textBox5.Text);

            string query = "INSERT INTO Products (Name, Description, Price, Quantity, ProductTypeID) VALUES (@Name, @Description, @Price, @Quantity, @ProductTypeID)";
            ExecuteNonQuery(query, ("@Name", name), ("@Description", description), ("@Price", price), ("@Quantity", quantity), ("@ProductTypeID", productTypeID));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                string name = textBox1.Text;
                string description = textBox2.Text;
                decimal price = Convert.ToDecimal(textBox3.Text);
                int quantity = Convert.ToInt32(textBox4.Text);
                int productTypeID = Convert.ToInt32(textBox5.Text);

                string query = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, Quantity = @Quantity, ProductTypeID = @ProductTypeID WHERE ID = @ProductID";
                ExecuteNonQuery(query, ("@Name", name), ("@Description", description), ("@Price", price), ("@Quantity", quantity), ("@ProductTypeID", productTypeID), ("@ProductID", productID));
            }
            else
            {
                MessageBox.Show("Выберите продукт для обновления.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                string query = "DELETE FROM Products WHERE ID = @ProductID";
                ExecuteNonQuery(query, ("@ProductID", productID));
            }
            else
            {
                MessageBox.Show("Выберите продукт для удаления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadProducts();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void Products_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.Instance.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Main.Instance.Show();
        }

        // Метод для стилизации формы и элементов
        private void StyleForm()
        {
            // Устанавливаем стиль для окна
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // Форма не изменяется
            this.StartPosition = FormStartPosition.CenterScreen; // Центрируем форму
            this.BackColor = Color.FromArgb(255, 204, 153); // Оранжевый фон

            // Стилизуем кнопки
            foreach (var button in new Button[] { button1, button2, button3, button4, button5 })
            {
                button.BackColor = Color.FromArgb(255, 153, 51); // Оранжевый фон для кнопок
                button.ForeColor = Color.White; // Белый текст на кнопках
                button.FlatStyle = FlatStyle.Flat; // Без границы у кнопок
                button.Font = new Font("Segoe UI", 12, FontStyle.Bold); // Шрифт кнопок
            }

            // Стилизуем текстовые поля
            foreach (var textBox in new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5 })
            {
                textBox.Font = new Font("Segoe UI", 12); // Шрифт для текстовых полей
                textBox.BackColor = Color.WhiteSmoke; // Светлый фон
                textBox.ForeColor = Color.Black; // Черный текст
            }

            // Стилизуем DataGridView
            dataGridView1.BackgroundColor = Color.White; // Фон таблицы
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 204); // Цвет строк
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black; // Черный текст
        }
    }
}
