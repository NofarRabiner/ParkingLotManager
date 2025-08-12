using AngleSharp.Browser.Dom;
using AventStack.ExtentReports.Gherkin.Model;
using NLog;
using OpenQA.Selenium;
using SeleniumLoginTests.TestData;
using SeleniumTests.Models;
using SeleniumTests.Pages;

namespace SeleniumTests.Tests.Functional
{
    public class ParkingFlowTests : BaseTest
    {
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;
        private HistoryPage _historyPage;
        private const string MY_CAR = "26858401";


        [SetUp]
        public new void SetUp()
        {
            logger.Info("Setting up ParkingFlowTests");
            _loginPage = new LoginPage(driver);
            _dashboardPage = new DashboardPage(driver);
            _historyPage = new HistoryPage(driver);  // Initialize HistoryPage
        }

        [TearDown]
        public new void TearDown()
        {
            logger.Info("Tearing down ParkingFlowTests");
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;

        }

        [Test]
        public void CompletedParkingSession_ShouldAppearInHistory()
        {
            logger.Info("Start CompletedParkingSession_ShouldAppearInHistory test");
            ParkingHistoryRecords carNewRecord = new ParkingHistoryRecords();
            _loginPage.GoTo();
            var (username, password) = Users.ValidUser;
            _loginPage.Login(username, password);
            string dashboardHeaderText = _dashboardPage.GetHeaderText();
            _dashboardPage.StartParkingProcess(carNewRecord);
            _dashboardPage.ScrollDown();
            _dashboardPage.EndParking(carNewRecord);
            _dashboardPage.ClickHistoryTab();

            Thread.Sleep(2000); // Wait for the history page to load
            List<List<string>> historyTableData = _historyPage.GetHistoryTableData();
            List<ParkingHistoryRecords> historyRecordsData = _historyPage.GetParkingHistoryRecords(historyTableData);

            bool isRegistered = _historyPage.IsTheVehicleRegisteredInTheHistoryTable(historyRecordsData, carNewRecord);
            Assert.IsTrue(isRegistered, "The vehicle should be registered in the history table as expected");
        }

        [Test]
        public void Prevent_DuplicateParking_ForSameVehicle_ByDifferentDrivers()
        {
            logger.Info("start Prevent_DuplicateParking_ForSameVehicle_ByDifferentDrivers test");
            ParkingHistoryRecords carRecord = new ParkingHistoryRecords();
            _loginPage.GoTo();
            var (username, password) = Users.ValidUser;
            _loginPage.Login(username, password);
            _dashboardPage.StartParkingProcess(carRecord);
            _dashboardPage.TryParkingWithTheSameCar(carRecord.CarPlate);

            _dashboardPage.ScrollDown();
            //_dashboardPage.EndParking(carNewRecord);
            //_dashboardPage.ClickHistoryTab();
            // Attempt to start parking again with the same car
            //_dashboardPage.StartParkingProcess(carNewRecord);
            //string alertText = _dashboardPage.GetAlertText();
           // Assert.IsTrue(alertText.Contains("You already have an active parking session for this vehicle."), "Alert message should indicate that the vehicle is already parked.");

        }



        protected void Click(By locator)
        {
            var element = WaitForElementClickable(locator);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Try to click via JavaScript as a fallback
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
            }
        }
        // Add this method to the ParkingFlowTests class to fix CS0103
        protected IWebElement WaitForElementClickable(By locator, int timeoutInSeconds = 10)
        {
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }
    }
}

