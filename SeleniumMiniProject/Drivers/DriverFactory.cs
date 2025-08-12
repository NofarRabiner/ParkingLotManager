using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumLoginTests.Drivers
{
    public class DriverFactory
    {
        public static IWebDriver CreateDriver()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            return new ChromeDriver(options);
        }
    }
}