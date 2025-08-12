namespace SeleniumLoginTests.TestData
{
    public static class Users
    {
        public static (string username, string password) ValidUser => ("admin", "password");
        public static (string username, string password) InvalidUser => ("invalidUser", "wrongPassword");
        public static (string username, string password) EmptyUsername => ("", "somePassword");
        public static (string username, string password) EmptyPassword => ("validUser", "");
        public static (string username, string password) SqlInjection => ("' OR 1=1 --", "password");
        public static (string username, string password) LongInput => (new string('a', 500), "password");
    }
}