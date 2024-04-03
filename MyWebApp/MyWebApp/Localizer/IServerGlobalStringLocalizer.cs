using Microsoft.Extensions.Localization;
using SoloX.BlazorJsonLocalization.Attributes;

namespace MyWebApp.Localizer
{
    /// <summary>
    /// The component class.
    /// In this example it is an empty class but instead you can use directly a razor component.
    /// </summary>
    public class ServerGlobal
    {
    }

    /// <summary>
    /// Here, we use the Localizer attribute to tell the LocalizationGen tool that we need the Json file
    /// stored in the wwwroot folder and that we want the fr-FR and en-UK files in addition of the default
    /// Json file.
    /// </summary>
    [Localizer("wwwroot", ["fr-FR", "en-UK"])]
    public interface IServerGlobalStringLocalizer : IStringLocalizer<ServerGlobal>
    {
        /// <summary>
        /// Let's define properties to access the localized values.
        /// The optional Translate attribute allows to define a default translation for the localized entry.
        /// </summary>

        [Translate("Home")]
        string HomePageTitle { get; }

        [Translate("Hello, world!")]
        string HelloTitle { get; }

        [Translate("Welcome to your new app.")]
        string HelloBody { get; }
    }
}
