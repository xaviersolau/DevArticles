// ----------------------------------------------------------------------
// <copyright file="ClientGlobalStringLocalizerExtensions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.Localization;
using MyWebApp.Client.Localizer;
using System;

namespace MyWebApp.Client.Localizer
{
    /// <summary>
    /// ClientGlobalStringLocalizerExtensions generated extension methods.
    /// </summary>
    public static class ClientGlobalStringLocalizerExtensions
    {
        /// <summary>
        /// Convert StringLocalizer to IClientGlobalStringLocalizer
        /// </summary>
        /// <param name="stringLocalizer">Base string localizer.</param>
        /// <returns>The localizer instance.</returns>
        public static IClientGlobalStringLocalizer ToClientGlobalStringLocalizer(this IStringLocalizer<ClientGlobal> stringLocalizer)
            => new ClientGlobalStringLocalizer(stringLocalizer);

        /// <summary>
        /// Get CounterPageTitle localized value from the String Localizer.
        /// </summary>
        /// <param name="stringLocalizer">String localizer to get the value from.</param>
        /// <returns>The localized value.</returns>
        public static string CounterPageTitle(this IStringLocalizer<ClientGlobal> stringLocalizer)
        {
            if (stringLocalizer == null)
            {
                throw new ArgumentNullException(nameof(stringLocalizer));
            }

            return stringLocalizer[nameof(CounterPageTitle)];
        }

        /// <summary>
        /// Get CounterTitle localized value from the String Localizer.
        /// </summary>
        /// <param name="stringLocalizer">String localizer to get the value from.</param>
        /// <returns>The localized value.</returns>
        public static string CounterTitle(this IStringLocalizer<ClientGlobal> stringLocalizer)
        {
            if (stringLocalizer == null)
            {
                throw new ArgumentNullException(nameof(stringLocalizer));
            }

            return stringLocalizer[nameof(CounterTitle)];
        }

        /// <summary>
        /// Get ClickMe localized value from the String Localizer.
        /// </summary>
        /// <param name="stringLocalizer">String localizer to get the value from.</param>
        /// <returns>The localized value.</returns>
        public static string ClickMe(this IStringLocalizer<ClientGlobal> stringLocalizer)
        {
            if (stringLocalizer == null)
            {
                throw new ArgumentNullException(nameof(stringLocalizer));
            }

            return stringLocalizer[nameof(ClickMe)];
        }

        /// <summary>
        /// Get CurrentCount localized value from the String Localizer.
        /// </summary>
        /// <param name="stringLocalizer">String localizer to get the value from.</param>
        /// <returns>The localized value.</returns>
        public static string CurrentCount(this IStringLocalizer<ClientGlobal> stringLocalizer, int currentCount)
        {
            if (stringLocalizer == null)
            {
                throw new ArgumentNullException(nameof(stringLocalizer));
            }

            return stringLocalizer[nameof(CurrentCount), currentCount];
        }
    }
}
