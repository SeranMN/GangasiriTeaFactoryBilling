using System;

namespace GangasiriTeaFactoryBilling
{
    public static class Session
    {
        public static int UserID { get; private set; }
        public static string Username { get; private set; }
        public static string Role { get; private set; }
        public static bool IsLoggedIn { get; private set; }

        public static void Login(int userId, string username, string role)
        {
            UserID = userId;
            Username = username;
            Role = role;
            IsLoggedIn = true;
        }

        public static void Logout()
        {
            UserID = 0;
            Username = null;
            Role = null;
            IsLoggedIn = false;
        }

        public static bool IsAdmin()
        {
            return IsLoggedIn && Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}
