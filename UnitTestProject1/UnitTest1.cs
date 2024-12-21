using MDKCurs;
using Microsoft.VisualStudio.TestTools.UnitTesting;  // Для MSTest

namespace MDKCursTests
{
    [TestClass]
    public class UserTests
    {
        private User _user;

        // Инициализация перед каждым тестом
        [TestInitialize]
        public void Setup()
        {
            _user = new User("testUser", "admin");
        }

        // 1. Проверка конструктора, чтобы установить имя и роль
        [TestMethod]
        public void Constructor_ShouldSetUsernameAndRole()
        {
            Assert.AreEqual("testUser", _user.Username);
            Assert.AreEqual("admin", _user.Role);
        }

        // 2. Проверка изменения имени пользователя
        [TestMethod]
        public void SetUsername_ShouldUpdateUsername()
        {
            _user.Username = "newUser";
            Assert.AreEqual("newUser", _user.Username);
        }

        // 3. Проверка изменения роли пользователя
        [TestMethod]
        public void SetRole_ShouldUpdateRole()
        {
            _user.Role = "user";
            Assert.AreEqual("user", _user.Role);
        }

        // 4. Проверка метода IsAdmin для роли "admin"
        [TestMethod]
        public void IsAdmin_ShouldReturnTrueForAdminRole()
        {
            Assert.IsTrue(_user.IsAdmin());
        }

        // 5. Проверка метода IsAdmin для роли "user"
        [TestMethod]
        public void IsAdmin_ShouldReturnFalseForNonAdminRole()
        {
            _user.Role = "user";
            Assert.IsFalse(_user.IsAdmin());
        }

        // 6. Проверка смены имени на пустое значение
        [TestMethod]
        public void ChangeUsername_ShouldNotUpdateUsername_WhenNewUsernameIsNull()
        {
            _user.ChangeUsername(null);
            Assert.AreEqual("testUser", _user.Username);
        }

        // 7. Проверка смены имени на пустую строку
        [TestMethod]
        public void ChangeUsername_ShouldNotUpdateUsername_WhenNewUsernameIsEmpty()
        {
            _user.ChangeUsername("");
            Assert.AreEqual("testUser", _user.Username);
        }

        // 8. Проверка смены имени на валидное значение
        [TestMethod]
        public void ChangeUsername_ShouldUpdateUsername_WhenNewUsernameIsValid()
        {
            _user.ChangeUsername("validUsername");
            Assert.AreEqual("validUsername", _user.Username);
        }

        // 9. Проверка, что метод IsAdmin возвращает false, если роль не "admin"
        [TestMethod]
        public void IsAdmin_ShouldReturnFalse_WhenRoleIsDifferent()
        {
            _user.Role = "guest";
            Assert.IsFalse(_user.IsAdmin());
        }

        // 10. Проверка конструктора, чтобы не присваивалась пустая роль
        [TestMethod]
        public void Constructor_ShouldNotAllowEmptyRole()
        {
            var userWithEmptyRole = new User("newUser", "");
            Assert.AreEqual("", userWithEmptyRole.Role);
        }
    }
}
