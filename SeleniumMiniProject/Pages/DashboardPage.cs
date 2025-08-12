using OpenQA.Selenium;
using SeleniumTests.Models;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Interfaces;

namespace SeleniumTests.Pages
{
    internal class DashboardPage : BasePage, IDashboardPage
    {
        private readonly By header = By.ClassName("dashboard-header");

        private readonly By carPlate = By.Id("car_plate");

        private readonly By historyTab = By.CssSelector("a[href='/history']");

        private readonly By slot = By.Id("slot");

        private readonly By startParkingButton = By.Id("submit");

        private readonly By endParkingButton = By.CssSelector("button.btn.btn-danger.btn-sm[type='submit']");

        private readonly By alert = By.CssSelector("div.alert");
        // IWebElement alertElement = driver.FindElement(By.CssSelector(".alert.alert-info"));

        public DashboardPage(IWebDriver driver) : base(driver)
        {
            base._driver = driver;
        }
        public void StartParkingProcess(ParkingHistoryRecords record)
        {
            string carNumber = getRandomCarNumber();
            record.CarPlate = carNumber;
            TypeCarNumber(carNumber);
            string slotStr = getRandomSlot();
            record.Slot = slotStr;
            TypeSlot(slotStr);
            ScrollDown();

            ClickStartParking();
            record.StartTime = DateTime.Now;
        }

        public bool IsDuplicateParkingAlertPresent(int timeoutSeconds = 5)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                var alert = wait.Until(driver =>
                {
                    var elements = driver.FindElements(By.CssSelector("div.alert.alert-warning"));
                    foreach (var el in elements)
                    {
                        try
                        {
                            if (el.Displayed && el.Text.Contains("Duplicate parking prevented: this car is already parked."))
                                return el;
                        }
                        catch (StaleElementReferenceException)
                        {
                            // Ignore and continue to next element
                        }
                    }
                    return null;
                });
                //var alert = wait.Until(driver =>
                //{
                //    var el = driver.FindElements(By.CssSelector("div.alert.alert-warning"));
                //    return el.FirstOrDefault(e => e.Displayed && e.Text.Contains("Duplicate parking prevented: this car is already parked."));
                //});
                return alert != null;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void TryParkingWithTheSameCar(string carNumber)
        {
            TypeCarNumber(carNumber);
            string slotStr = getRandomSlot();
            TypeSlot(slotStr);
            ScrollDown();
            ClickStartParking();
            // Assert that the duplicate parking alert is present
            Assert.IsTrue(IsDuplicateParkingAlertPresent(), "Duplicate parking alert found");
        }
        private string getRandomCarNumber()
        {
            Random random = new Random();
            string carNumber = random.Next(10000000, 99999999).ToString();
            return carNumber;
        }
        private string getRandomSlot(int length = 3)
        {
            const string chars = "0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

        }

        public string GetHeaderText() => GetText(header);

        public string GetAlertText() => GetText(alert);

        public void TypeCarNumber(string carNum) => Type(carPlate, carNum);
        public void TypeSlot(string slotStr) => Type(slot, slotStr);

        public void ScrollDown() => ScrollDownToEndOfPage();
        public void ClickStartParking() => Click(startParkingButton);

        public void ClickHistoryTab() => Click(historyTab);
        public void EndParking(ParkingHistoryRecords record)
        {
            record.EndTime = DateTime.Now;
            var buttons = _driver.FindElements(endParkingButton);
            if (buttons == null || buttons.Count == 0)
                throw new Exception("No end parking button found.");

            var lastButton = buttons.Last();

            // Scroll into view
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lastButton);

            // Wait until clickable
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(lastButton));

            try
            {
                lastButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Fallback: click via JS if intercepted
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", lastButton);
            }
        }


    }
}
