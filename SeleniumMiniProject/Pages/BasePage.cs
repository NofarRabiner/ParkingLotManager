using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumTests.Pages
{
    public class BasePage
    {
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();
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

        protected string GetText(By locator, int maxRetries = 20)
        {
            int attempts = 0;
            while (attempts < maxRetries)
            {
                try
                {
                    var element = _wait.Until(driver =>
                    {
                        var el = driver.FindElement(locator);
                        return (el != null && el.Displayed) ? el : null;
                    });
                    return element.Text;
                }
                catch (StaleElementReferenceException)
                {
                    attempts++;
                    // Optionally add a small delay before retrying
                    Thread.Sleep(200);
                }
            }
            throw new StaleElementReferenceException($"Unable to get text after {maxRetries} attempts.");
        }

        protected bool IsVisible(By locator)
        {
            try
            {
                var element = _wait.Until(driver =>
                {
                    var el = driver.FindElement(locator);
                    return (el != null && el.Displayed) ? el : null;
                });
                return element.Displayed;
            }
            catch
            {
                return false;
            }
        }

        protected void WaitForElementToDisappear(By locator)
        {
            _wait.Until(driver =>
            {
                try
                {
                    var el = driver.FindElement(locator);
                    return !el.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
            });
        }
        protected string GetAlertTextAndDismiss()
        {
            IAlert alert = _wait.Until(ExpectedConditions.AlertIsPresent());
            string? text = alert.Text;
            alert.Dismiss();
            return text ?? string.Empty;
        }

        protected string EnterTextAndAcceptAlert(string input)
        {
            IAlert alert = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
            alert.SendKeys(input);
            string? text = alert.Text;
            alert.Accept();
            return text ?? string.Empty;
        }

        protected void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public string GetAlertText()
        {
            // Use a fresh find each time to avoid stale references
            try
            {
                // Wait for the alert to be present and visible
                var alert = _wait.Until(driver =>
                {
                    var el = driver.FindElement(By.CssSelector("div.alert"));
                    return (el != null && el.Displayed) ? el : null;
                });
                return alert.Text;
            }
            catch (StaleElementReferenceException)
            {
                // Try to re-find the element once if it was replaced
                var alert = _driver.FindElement(By.CssSelector("div.alert"));
                return alert.Text;
            }
            catch (WebDriverTimeoutException)
            {
                // If the alert never appears, return empty or throw
                return string.Empty;
            }
        }
    }
}