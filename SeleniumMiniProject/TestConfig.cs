namespace SeleniumLoginTests.Config
{
    public static class TestConfig
    {
        public static string BaseUrl => "http://localhost:32769/login?next=%2F";
        public static string LoginUrl => $"{BaseUrl}/login";

    }
}