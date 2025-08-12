
namespace SeleniumLoginTests.Interfaces
{
    public interface ILoginPage
    {
        public interface ILoginPage
        {
            void GoTo();
            void NavigateTo(string url);
            void Login(string username, string password);
            string GetErrorMessage();
            void EnterUsername(string username);
            void EnterPassword(string password);
            void ClickLogin();
            string GetFlashMessage();
        }
    }
}
