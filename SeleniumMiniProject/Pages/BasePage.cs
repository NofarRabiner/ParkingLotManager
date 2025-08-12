using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests.Pages
{
    public class BasePage
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected IWebDriver _driver;
        protected WebDriverWait _wait;

        public BasePage(IWebDriver driver)
        {
            logger.Info("Initializing BasePage");
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        protected IWebElement WaitForElementVisible(By locator, int seconds = 10)
        {
            return new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        protected IWebElement WaitForElementClickable(By locator, int seconds = 10)
        {
            return new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }
        protected void SwitchToIframe(By locator)
        {
            IWebElement iframe = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
            _driver.SwitchTo().Frame(iframe);
        }

        protected void SwitchToDefaultContent()
        {
            _driver.SwitchTo().DefaultContent();
        }

        protected void Click(By locator)
        {
            int maxRetries = 5;
            int attempts = 0;
            Exception lastException = null;
            while (attempts < maxRetries)
            {
                try
                {
                    var element = WaitForElementClickable(locator);
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
                    // Optionally, add a small wait to ensure any animations or overlays finish
                    Thread.Sleep(500); // or use WebDriverWait for overlays to disappear
                    element.Click();
                    return;
                }
                catch (StaleElementReferenceException ex)
                {
                    attempts++;
                    lastException = ex;
                    Thread.Sleep(200);
                }
            }
            throw lastException ?? new StaleElementReferenceException($"Unable to click after {maxRetries} attempts.");
        }
        protected void ScrollDownToEndOfPage()      
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

        }

        protected void ScrollToElement(By locator)
        {
            try
            {
                var element = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            }
            catch (WebDriverTimeoutException ex)
            {
                logger.Error(ex, $"Element with locator '{locator}' was not found or not visible for scrolling.");
                throw;
            }
        }
   /*    public string GetLoginValidationErrorMessage()
        {
            IWebElement passwordField = _driver.FindElement(By.Id("password")); // Replace with actual ID or locator
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            bool isValid = (bool)js.ExecuteScript("return arguments[0].checkValidity();", passwordField);
            string message = (string)js.ExecuteScript("return arguments[0].validationMessage;", passwordField);
            logger.Error("Validation message: " + message);
            return message;

        }*/

        protected void ClickUsingJs(By locator)
        {
            var element = _wait.Until(driver => driver.FindElement(locator));
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].click();", element);
        }

        protected void Type(By locator, string text)
        {
            int maxRetries = 5;
            int attempts = 0;
            Exception lastException = null;
            while (attempts < maxRetries)
            {
                try
                {
                    var element = _wait.Until(driver =>
                    {
                        var el = driver.FindElement(locator);
                        return (el != null && el.Displayed) ? el : null;
                    });
                    element.Clear();
                    element.SendKeys(text);
                    return;
                }
                catch (StaleElementReferenceException ex)
                {
                    attempts++;
                    lastException = ex;
                    Thread.Sleep(200);
                }
            }
            throw lastException ?? new StaleElementReferenceException($"Unable to type after {maxRetries} attempts.");
        }

        protected string GetText(By locator)
        {
            var element = _wait.Until(driver =>
            {
                var el = driver.FindElement(locator);
                return (el != null && el.Displayed) ? el : null;
            });
            return element.Text;
        }
    }
}