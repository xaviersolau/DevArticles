// ----------------------------------------------------------------------
// <copyright file="ServerGlobalStringLocalizer.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

#pragma warning disable IDE0005 // Using directive is unnecessary.
using Microsoft.Extensions.Localization;
using SoloX.BlazorJsonLocalization;
using MyWebApp.Localizer;
using System.Collections.Generic;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace MyWebApp.Localizer
{
    /// <summary>
    /// IServerGlobalStringLocalizer generated implementation.
    /// </summary>
    public class ServerGlobalStringLocalizer : IServerGlobalStringLocalizer
    {
        private readonly IStringLocalizer<ServerGlobal> stringLocalizer;

        /// <summary>
        /// Setup instance.
        /// </summary>
        /// <param name="stringLocalizer">Base string localizer.</param>
        public ServerGlobalStringLocalizer(IStringLocalizer<ServerGlobal> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }

        /// <inheritdoc/>
        public LocalizedString this[string name]
            => this.stringLocalizer[name];

        /// <inheritdoc/>
        public LocalizedString this[string name, params object[] arguments]
            => this.stringLocalizer[name, arguments];

        /// <inheritdoc/>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => this.stringLocalizer.GetAllStrings(includeParentCultures);

        /// <summary>
        /// Get HomePageTitle localized string.
        /// </summary>
        public string HomePageTitle
        {
            get => this.stringLocalizer[nameof(HomePageTitle)];
        }

        /// <summary>
        /// Get HelloTitle localized string.
        /// </summary>
        public string HelloTitle
        {
            get => this.stringLocalizer[nameof(HelloTitle)];
        }

        /// <summary>
        /// Get HelloBody localized string.
        /// </summary>
        public string HelloBody
        {
            get => this.stringLocalizer[nameof(HelloBody)];
        }
    }


}
