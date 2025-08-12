using NLog;
using NUnit.Framework;
using OpenQA.Selenium; // Add this for NoSuchElementException
using OpenQA.Selenium.Support.UI;
using SeleniumLoginTests.Config;
using SeleniumLoginTests.Drivers;
using SeleniumLoginTests.TestData;
using SeleniumTests.Pages;
using System;

namespace SeleniumTests.Tests
{
    public class LoginTests : BaseTest
    {
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;


        [SetUp]
        public new void SetUp()
        {
            logger.Info("Setting up LoginTests environment");
            _loginPage = new LoginPage(driver);
            _dashboardPage = new DashboardPage(driver);// Ensure _jsPage is initialized
        }

        [TearDown]
        public new void TearDown()
        {
            logger.Info("Tearing down LoginTests environment");
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;

        }
 


 /*       [Test]
        public void FailedLogin_ShouldShowErrorMessage()
        {
            logger.Info("Starting failed login test");

            _loginPage.GoTo();
            _loginPage.EnterUsername("invalidUser");
            _loginPage.EnterPassword("wrongPass");
            _loginPage.ClickLogin();

            string message = _loginPage.GetFlashMessage();
            logger.Info($"Error message:  {_loginPage.GetErrorMessage()}, Expected: {"Your username is invalid!"} ");
            Assert.IsTrue(message.Contains("Your username is invalid!"), "Error message not shown as expected.");
        }


        [Test]
        public void InvalidLogin_ShouldShowError()
        {
            logger.Info("Starting Invalid login test");
            _loginPage.GoTo();
            var (username, password) = Users.InvalidUser;

            _loginPage.Login(username, password);
            string message = _loginPage.GetFlashMessage();
            logger.Info($"Error message:  {_loginPage.GetErrorMessage()}, Expected: {"Your username is invalid!"} ");
            Assert.IsTrue(_loginPage.GetErrorMessage().Contains("Your username is invalid!"), "Error message not shown as expected.");
        }

        [Test]
        public void EmptyUsername_ShouldShowError()
        {
            logger.Info("Starting Empty username test");
            _loginPage.GoTo();
            var (username, password) = Users.EmptyUsername;

            _loginPage.Login(username, password);

            string message = _loginPage.GetFlashMessage();
            logger.Info($"Error message:  {_loginPage.GetErrorMessage()}, Expected: {"Your username is invalid!"} ");
            Assert.IsTrue(_loginPage.GetErrorMessage().Contains("Your username is invalid!"), "Error message not shown as expected.");
        }

        [Test]
        public void EmptyPassword_ShouldShowError()
        {
            logger.Info("Starting Empty password test");
            _loginPage.GoTo();
            var (username, password) = Users.EmptyPassword;

            _loginPage.Login(username, password);
            string message = _loginPage.GetErrorMessage();
            logger.Info($"Error message:  {_loginPage.GetErrorMessage()}, Expected: {"Your username is invalid!"} ");
            Assert.IsTrue(_loginPage.GetErrorMessage().Contains("Your username is invalid!"), "Error message not shown as expected.");
        }

        [Test]
        public void SqlInjection_ShouldShowError()
        {
            logger.Info("Starting SQL Injection test");
            _loginPage.GoTo();
            var (username, password) = Users.SqlInjection;

            _loginPage.Login(username, password);
            string message = _loginPage.GetErrorMessage();
            logger.Info($"Error message:  {_loginPage.GetErrorMessage()}, Expected: Your username is invalid! ");
            Assert.IsTrue(_loginPage.GetErrorMessage().Contains("Your username is invalid!"), "Error message not shown as expected.");
        }

        [Test]
        public void LongInput_ShouldShowErrorOrLimit()
        {
            logger.Info("Starting Long input test");
            _loginPage.GoTo();
            var (username, password) = Users.LongInput;

            _loginPage.Login(username, password);

            string message = _loginPage.GetErrorMessage();
            logger.Info($"Error message:  {_loginPage.GetErrorMessage()}, Expected: Your username is invalid! ");
            Assert.IsTrue(_loginPage.GetErrorMessage().Contains("Your username is invalid"));
        }
        [Test]
        public void ClickJsAlertButton()
        {
            logger.Info("Starting JS Alert button click test");
            _jsPage.GoTo();

            string alertText = _jsPage.ClickConfirmAndDismiss();
            logger.Info($"Alert test: {alertText}");
            Assert.That(alertText, Is.EqualTo("I am a JS Confirm"));
        }*/
    }
}
