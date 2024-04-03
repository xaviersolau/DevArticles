using Microsoft.Extensions.Localization;
using SoloX.BlazorJsonLocalization.Attributes;

namespace MyWebApp.Client.Localizer
{
    /// <summary>
    /// The component class.
    /// In this example it is an empty class but instead you can use directly a razor component.
    /// </summary>
    public class ClientGlobal
    {
    }

    /// <summary>
    /// Here, we use the Localizer attribute to tell the LocalizationGen tool that we need the Json file
    /// stored in the wwwroot folder and that we want the fr-FR and en-UK files in addition of the default
    /// Json file.
    /// </summary>
    [Localizer("wwwroot", ["fr-FR", "en-UK"])]
    public interface IClientGlobalStringLocalizer : IStringLocalizer<ClientGlobal>
    {
        /// <summary>
        /// Let's define properties to access the localized values.
        /// The optional Translate attribute allows to define a default translation for the localized entry.
        /// </summary>

        [Translate("Counter")]
        string CounterPageTitle { get; }

        [Translate("Counter")]
        string CounterTitle { get; }

        [Translate("Click me")]
        string ClickMe {  get; }

        /// <summary>
        /// Here is an example where we define a localized entry with a method instead of a property. This is
        /// to provide a way to use arguments in your localized message.
        /// </summary>
        /// <param name="currentCount">The argument we want to use in the message.</param>
        /// <returns>The localized value with the argument(s) serialized within the message.</returns>
        [Translate("Current count: {0}")]
        string CurrentCount(int currentCount);
    }
}
