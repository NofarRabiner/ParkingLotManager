using NLog;
using SeleniumTests.Pages;
using SeleniumLoginTests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTests.Tests.Validations
{
    public class ValidationTests : BaseTest
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
        [Test]
        public void SuccessfulLogin_VerifyHeaderContainExpectedValue()
        {
            logger.Info("Starting successful login test");

            _loginPage.GoTo();
            var (username, password) = Users.ValidUser;
            _loginPage.Login(username, password);
            string dashboardHeaderText = _dashboardPage.GetHeaderText();

            Assert.IsTrue(dashboardHeaderText.Contains("חניות פעילות נוכחיות"), "Dashboard header text should contain 'חניות פעילות נוכחיות'.");

        }
        [Test]
        public void EmptyPassword_passwordErrorJSFieldValidation()
        {
            logger.Info("Starting EmptyPassword_passwordErrorJSFieldValidation Test");
            _loginPage.GoTo();
            var (username, password) = Users.EmptyPassword;

            _loginPage.Login(username, password);
            string message = _loginPage.GetLoginValidationErrorMessage();
            Assert.IsTrue(message.Contains("Please fill out this field"), "Error message not shown as expected.");
        }
        [Test]
        public void SQLInjectionTest_Validation()
        {
            logger.Info("SQLInjectionTest_Validation");
            _loginPage.GoTo();
            var (username, password) = Users.SqlInjection;

            _loginPage.Login(username, password);
            string url =driver.Url;
            Assert.IsTrue(url.Contains("login"), "SQL injection test fail");

        }

    }
}
