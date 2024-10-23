namespace AppBlazor.Services
{
    /// <summary>
    /// Production service implementation that will be replaced in the test project.
    /// </summary>
    public class WelcomeMessageService : IWelcomeMessageService
    {
        /// <summary>
        /// Get the production welcome message.
        /// </summary>
        public string GetMessage()
        {
            return "Welcome to your new app in Production.";
        }
    }
}
