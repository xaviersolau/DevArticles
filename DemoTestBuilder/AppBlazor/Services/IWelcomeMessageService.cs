﻿namespace AppBlazor.Services
{
    /// <summary>
    /// Service interface that will be injected and used in the application Home page.
    /// </summary>
    public interface IWelcomeMessageService
    {
        /// <summary>
        /// Get the production welcome message.
        /// </summary>
        string GetMessage();
    }
}
