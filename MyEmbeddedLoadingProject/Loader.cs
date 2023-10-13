using Microsoft.Extensions.FileProviders;
using System.Globalization;
using System.Reflection;
using System.Text.Json;

namespace MyEmbeddedLoadingProject
{
    public class Loader
    {
        private readonly Assembly assembly;

        /// <summary>
        /// Setup Loader with the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        public Loader(Assembly assembly)
        {
            this.assembly = assembly;
        }

        /// <summary>
        /// Load and display all values from the given localization embedded file.
        /// </summary>
        /// <returns>The asynchronous task result.</returns>
        public Task LoadAndDisplay(string fileName)
        {
            // Try to get the cultureInfo from the file name to know witch satellite assembly needs to be loaded.
            var cultureInfo = TryGetCultureInfo(fileName);

            // Make sure the resource file name is defined without its culture name.
            var baseResourceFileName = GetBaseResourceName(fileName, cultureInfo);

            // Get the main assembly or the satellite assembly depending if there is a culture name specified.
            var assemblyToLoad = cultureInfo == null ? this.assembly : this.assembly.GetSatelliteAssembly(cultureInfo);

            // Get the EmbeddedFileProvider instance with the assembly.
            var fileProvider = new EmbeddedFileProvider(assemblyToLoad, this.assembly.GetName().Name);

            // Get the file info matching the given fileName argument.
            var globalFileInfo = fileProvider.GetFileInfo(baseResourceFileName);

            // Then we can just call the method that take the fileInfo.
            return LoadAndDisplayFromFileInfo(globalFileInfo);
        }

        private CultureInfo? TryGetCultureInfo(string fileName)
        {
            // Find the CultureInfo matching the file name suffix.
            var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            return allCultures.FirstOrDefault(ci => fileName.EndsWith($".{ci.Name}.json"));
        }

        private string GetBaseResourceName(string fileName, CultureInfo? cultureInfo)
        {
            // Remove the culture name from the file name.
            return cultureInfo == null ? fileName : fileName.Replace($".{cultureInfo.Name}.json", ".json");
        }

        private async Task LoadAndDisplayFromFileInfo(IFileInfo fileInfo)
        {
            // Check that the file exists
            if (!fileInfo.Exists)
            {
                Console.WriteLine($"Embedded resource '{fileInfo.Name}' doesn't exist.");
                return;
            }

            Console.WriteLine($"Loading embedded resource '{fileInfo.Name}'.");

            // Get the file content stream.
            using var fileStream = fileInfo.CreateReadStream();

            // Load the Json data.
            var entries = await JsonSerializer
              .DeserializeAsync<Dictionary<string, string>>(fileStream);

            // Display all localization entries.
            foreach (var entry in entries)
            {
                Console.WriteLine($"Localization entry '{entry.Key}' is '{entry.Value}'.");
            }
            Console.WriteLine();
        }
    }
}
