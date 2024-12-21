using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MDKCurs
{
    public partial class ProductTypes : Form
    {
        private string connectionString = @"Data Source=ADCLG1;Initial catalog=SKLAD;Integrated Security=True";
        private SqlConnection connection;

        public ProductTypes()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            StyleForm(); // Применяем стили к форме
        }

        private void ProductTypes_Load(object sender, EventArgs e)
        {
            // Отладка: выводим роль пользователя в консоль (или MessageBox)
            string currentRole = UserManager.CurrentUser.Role;
            MessageBox.Show($"Текущая роль пользователя: {currentRole}");  // Отладка: проверим роль

            // Скрываем кнопки только если роль "user" и после загрузки данных
            if (currentRole == "user")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            LoadProductTypes();
        }

        private void LoadProductTypes()
        {
            string query = "SELECT Type_ID, Name FROM ProductTypes";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();

            try
            {
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка загрузки типов продуктов из базы данных: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string typeName = textBox1.Text.Trim();
            if (!string.IsNullOrWhiteSpace(typeName))
            {
                string query = "INSERT INTO ProductTypes (Name) VALUES (@Name)";
                ExecuteNonQuery(query, ("@Name", typeName));
                LoadProductTypes();
                MessageBox.Show("Категория успешно добавлена.");
            }
            else
            {
                MessageBox.Show("Введите название категории.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productTypeID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Type_ID"].Value);
                string typeName = textBox1.Text.Trim();

                string query = "UPDATE ProductTypes SET Name = @Name WHERE Type_ID = @ProductTypeID";
                ExecuteNonQuery(query, ("@Name", typeName), ("@ProductTypeID", productTypeID));
                LoadProductTypes();
                MessageBox.Show("Категория успешно обновлена.");
            }
            else
            {
                MessageBox.Show("Выберите тип продукта для обновления.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productTypeID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Type_ID"].Value);

                string query = "DELETE FROM ProductTypes WHERE Type_ID = @ProductTypeID";
                ExecuteNonQuery(query, ("@ProductTypeID", productTypeID));
                LoadProductTypes();
                MessageBox.Show("Категория успешно удалена.");
            }
            else
            {
                MessageBox.Show("Выберите тип продукта для удаления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadProductTypes();
            textBox1.Text = "";
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["Name"].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Main.Instance.Show();
        }

        private void ProductTypes_FormClosed(object sender, FormClosedEventArgs e)
        {
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
            foreach (var textBox in new TextBox[] { textBox1 })
            {
                textBox.Font = new Font("Segoe UI", 12); // Шрифт для текстового поля
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
