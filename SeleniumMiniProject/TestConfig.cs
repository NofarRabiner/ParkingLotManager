namespace SeleniumLoginTests.Config
{
    public static class TestConfig
    {
        public static string BaseUrl => "http://localhost:5000/login?next=%2F";
        public static string LoginUrl => $"{BaseUrl}/login";

    }
}
