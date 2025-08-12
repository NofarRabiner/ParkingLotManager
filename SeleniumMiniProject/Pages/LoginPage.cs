using NLog;
using OpenQA.Selenium;
using SeleniumLoginTests.Config;
using SeleniumLoginTests.Interfaces;

namespace SeleniumTests.Pages
{
    public class LoginPage : BasePage, ILoginPage
    {
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public LoginPage(IWebDriver driver) : base(driver)
        {
            base._driver = driver;
        }

        private readonly By loginButton = By.CssSelector("button[type='submit']");

        private readonly By userNameInput = By.Id("username");

        private readonly By passwordInput = By.Id("password");

        public void GoTo()
        {
            logger.Info($"Navigating to the login page: {TestConfig.LoginUrl}");    
            NavigateTo(TestConfig.LoginUrl);        
        }

        public new void NavigateTo(string url)
        {
            logger.Info($"Navigating to URL: {url}");
            _driver.Navigate().GoToUrl(url);
        }

        public void Login(string username, string password)
        {
            logger.Info($"Logging in with username: {username}, password: {password}");
            Type(userNameInput, username);
            Type(passwordInput, password);
            Click(loginButton);
        }

        public string GetLoginValidationErrorMessage()
        {
            IWebElement passwordField = _driver.FindElement(By.Id("password")); 
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            bool isValid = (bool)js.ExecuteScript("return arguments[0].checkValidity();", passwordField);
            string message = (string)js.ExecuteScript("return arguments[0].validationMessage;", passwordField);
            logger.Error("Validation message: " + message);
            return message;

        }
    }
}