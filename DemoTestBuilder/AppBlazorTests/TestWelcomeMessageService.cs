using AppBlazor.Services;

namespace AppBlazorTests
{
    /// <summary>
    /// Test service implementation that will replace the one in the application.
    /// </summary>
    public class TestWelcomeMessageService : IWelcomeMessageService
    {
        /// <summary>
        /// Get the test welcome message.
        /// </summary>
        public string GetMessage()
        {
            return "<p data-test=\"test-message\">Welcome to your Test.</p>";
        }
    }
}
