using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumLoginTests.Drivers;
using System;

namespace SeleniumTests.Tests
{
    // BaseTest is now abstract and uses virtual methods for extensibility
    public abstract class BaseTest
    {
        protected IWebDriver driver;

        [SetUp]
        public virtual void SetUp()
        {
            driver = DriverFactory.CreateDriver();
        }

        [TearDown]
        public virtual void TearDown()
        {
            HandleUnexpectedAlert();
            DisposeDriver();
        }

        // Handles unexpected alerts in a robust way
        protected void HandleUnexpectedAlert()
        {
            if (driver == null) return;
            try
            {
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(2));
                wait.Until(drv =>
                {
                    try
                    {
                        IAlert alert = drv.SwitchTo().Alert();
                        alert.Accept();
                        return true;
                    }
                    catch (NoAlertPresentException)
                    {
                        return true;
                    }
                });
            }
            catch (WebDriverException)
            {
                // Log or ignore teardown exceptions
            }
        }

        // Ensures proper disposal of the driver
        protected void DisposeDriver()
        {
            if (driver != null)
            {
                try
                {
                    driver.Quit(); // Properly closes all browser windows and ends session
                }
                catch (WebDriverException)
                {
                    // Log or ignore exceptions during quit
                }
                finally
                {
                    driver.Dispose(); // Explicitly dispose the driver
                    driver = default!; // Use default! to suppress CS8625 for non-nullable reference type
                }
            }
        }
    }
}