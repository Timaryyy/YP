using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MDKCurs
{
    public partial class Users : Form
    {
        private string connectionString = @"Data Source=ADCLG1;Initial catalog=SKLAD;Integrated Security=True";
        private SqlConnection connection;

        public Users()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadUsers();
            StyleForm();
        }

        private void LoadUsers()
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

            // Запрос для загрузки данных пользователей
            string query = "SELECT User_ID AS ID, Name AS Login, Password, Role FROM Users"; // Убедитесь, что здесь указаны правильные имена столбцов
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем значения из текстовых полей
            string login = textBox1.Text.Trim();
            string password = textBox3.Text.Trim();
            string role = textBox6.Text.Trim();

            // Проверяем, заполнены ли все поля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                // Выводим сообщение об ошибке
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Прерываем выполнение метода
            }

            // Формируем запрос для добавления пользователя
            string query = "INSERT INTO Users (Name, Password, Role) VALUES (@Login, @Password, @Role)";
            ExecuteNonQuery(query, ("@Login", login), ("@Password", password), ("@Role", role));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int userID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                string login = textBox1.Text;
                string password = textBox3.Text;
                string role = textBox6.Text;

                string query = "UPDATE Users SET Name = @Login, Password = @Password, Role = @Role WHERE User_ID = @UserID";
                ExecuteNonQuery(query, ("@Login", login), ("@Password", password), ("@Role", role), ("@UserID", userID));
            }
            else
            {
                MessageBox.Show("Выберите пользователя для обновления.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int userID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                string query = "DELETE FROM Users WHERE User_ID = @UserID";
                ExecuteNonQuery(query, ("@UserID", userID));
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadUsers();
            textBox1.Text = "";
            textBox3.Text = "";
            textBox6.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["Login"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["Password"].Value.ToString();
                textBox6.Text = dataGridView1.SelectedRows[0].Cells["Role"].Value.ToString();
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
                    LoadUsers(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void Users_FormClosed(object sender, FormClosedEventArgs e)
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
            foreach (var textBox in new TextBox[] { textBox1, textBox3, textBox6 })
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
