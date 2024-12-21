using MDKCurs;
using System;

namespace MDKCurs
{
    internal class UserManager
    {
        public static User CurrentUser { get; private set; }

        // Метод для установки текущего пользователя
        public static void SetCurrentUser(string username, string role)
        {
            CurrentUser = new User(username, role);
        }
    }
}

