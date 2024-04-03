// ----------------------------------------------------------------------
// <copyright file="ClientGlobalStringLocalizer.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

#pragma warning disable IDE0005 // Using directive is unnecessary.
using Microsoft.Extensions.Localization;
using SoloX.BlazorJsonLocalization;
using MyWebApp.Client.Localizer;
using System.Collections.Generic;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace MyWebApp.Client.Localizer
{
    /// <summary>
    /// IClientGlobalStringLocalizer generated implementation.
    /// </summary>
    public class ClientGlobalStringLocalizer : IClientGlobalStringLocalizer
    {
        private readonly IStringLocalizer<ClientGlobal> stringLocalizer;

        /// <summary>
        /// Setup instance.
        /// </summary>
        /// <param name="stringLocalizer">Base string localizer.</param>
        public ClientGlobalStringLocalizer(IStringLocalizer<ClientGlobal> stringLocalizer)
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
        /// Get CounterPageTitle localized string.
        /// </summary>
        public string CounterPageTitle
        {
            get => this.stringLocalizer[nameof(CounterPageTitle)];
        }

        /// <summary>
        /// Get CounterTitle localized string.
        /// </summary>
        public string CounterTitle
        {
            get => this.stringLocalizer[nameof(CounterTitle)];
        }

        /// <summary>
        /// Get ClickMe localized string.
        /// </summary>
        public string ClickMe
        {
            get => this.stringLocalizer[nameof(ClickMe)];
        }

        /// <summary>
        /// Get CurrentCount localized string.
        /// </summary>
        public string CurrentCount(int currentCount)
            => this.stringLocalizer[nameof(CurrentCount), currentCount];
    }


}
