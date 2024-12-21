using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MDKCurs
{
    public partial class Orders : Form
    {
        private string connectionString = @"Data Source=ADCLG1;Initial catalog=SKLAD;Integrated Security=True";
        private SqlConnection connection;

        public Orders()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            StyleForm(); // Применяем стили к форме
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            LoadOrders();
            // Устанавливаем формат даты для dateTimePicker1
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        private void LoadOrders()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Admin")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT ID, CreationDate, Status, CustomerID FROM Orders";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dtg.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime date = dateTimePicker1.Value;
            string status = textBox2.Text;
            int customerID;
            if (!int.TryParse(textBox3.Text, out customerID))
            {
                MessageBox.Show("Невозможно преобразовать идентификатор заказчика в число.");
                return;
            }

            string query = "INSERT INTO Orders (CreationDate, Status, CustomerID) VALUES (@CreationDate, @Status, @CustomerID)";
            ExecuteNonQuery(query, ("@CreationDate", date), ("@Status", status), ("@CustomerID", customerID));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dtg.SelectedRows.Count > 0)
            {
                int orderID = Convert.ToInt32(dtg.SelectedRows[0].Cells["ID"].Value);
                DateTime date = dateTimePicker1.Value;
                string status = textBox2.Text;
                int customerID = Convert.ToInt32(textBox3.Text);

                string query = "UPDATE Orders SET CreationDate = @CreationDate, Status = @Status, CustomerID = @CustomerID WHERE ID = @OrderID";
                ExecuteNonQuery(query, ("@CreationDate", date), ("@Status", status), ("@CustomerID", customerID), ("@OrderID", orderID));
            }
            else
            {
                MessageBox.Show("Выберите заказ для обновления.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dtg.SelectedRows.Count > 0)
            {
                int orderID = Convert.ToInt32(dtg.SelectedRows[0].Cells["ID"].Value);

                string query = "DELETE FROM Orders WHERE ID = @OrderID";
                ExecuteNonQuery(query, ("@OrderID", orderID));
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления.");
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
                    LoadOrders(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadOrders();
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void dtg_SelectionChanged(object sender, EventArgs e)
        {
            if (dtg.SelectedRows.Count > 0)
            {
                string dateString = dtg.SelectedRows[0].Cells["CreationDate"].Value.ToString();
                DateTime date;

                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    dateTimePicker1.Value = date;
                }

                textBox2.Text = dtg.SelectedRows[0].Cells["Status"].Value.ToString();
                textBox3.Text = dtg.SelectedRows[0].Cells["CustomerID"].Value.ToString();
            }
        }

        private void Orders_FormClosed(object sender, FormClosedEventArgs e)
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
            foreach (var textBox in new TextBox[] { textBox2, textBox3 })
            {
                textBox.Font = new Font("Segoe UI", 12); // Шрифт для текстовых полей
                textBox.BackColor = Color.WhiteSmoke; // Светлый фон
                textBox.ForeColor = Color.Black; // Черный текст
            }

            // Стилизуем DataGridView
            dtg.BackgroundColor = Color.White; // Фон таблицы
            dtg.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 204); // Цвет строк
            dtg.DefaultCellStyle.ForeColor = Color.Black; // Черный текст
        }
    }
}
