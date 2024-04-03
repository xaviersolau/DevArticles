// ----------------------------------------------------------------------
// <copyright file="ServerGlobalStringLocalizerExtensions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.Localization;
using MyWebApp.Localizer;
using System;

namespace MyWebApp.Localizer
{
    /// <summary>
    /// ServerGlobalStringLocalizerExtensions generated extension methods.
    /// </summary>
    public static class ServerGlobalStringLocalizerExtensions
    {
        /// <summary>
        /// Convert StringLocalizer to IServerGlobalStringLocalizer
        /// </summary>
        /// <param name="stringLocalizer">Base string localizer.</param>
        /// <returns>The localizer instance.</returns>
        public static IServerGlobalStringLocalizer ToServerGlobalStringLocalizer(this IStringLocalizer<ServerGlobal> stringLocalizer)
            => new ServerGlobalStringLocalizer(stringLocalizer);

        /// <summary>
        /// Get HomePageTitle localized value from the String Localizer.
        /// </summary>
        /// <param name="stringLocalizer">String localizer to get the value from.</param>
        /// <returns>The localized value.</returns>
        public static string HomePageTitle(this IStringLocalizer<ServerGlobal> stringLocalizer)
        {
            if (stringLocalizer == null)
            {
                throw new ArgumentNullException(nameof(stringLocalizer));
            }

            return stringLocalizer[nameof(HomePageTitle)];
        }

        /// <summary>
        /// Get HelloTitle localized value from the String Localizer.
        /// </summary>
        /// <param name="stringLocalizer">String localizer to get the value from.</param>
        /// <returns>The localized value.</returns>
        public static string HelloTitle(this IStringLocalizer<ServerGlobal> stringLocalizer)
        {
            if (stringLocalizer == null)
            {
                throw new ArgumentNullException(nameof(stringLocalizer));
            }

            return stringLocalizer[nameof(HelloTitle)];
        }

        /// <summary>
        /// Get HelloBody localized value from the String Localizer.
        /// </summary>
        /// <param name="stringLocalizer">String localizer to get the value from.</param>
        /// <returns>The localized value.</returns>
        public static string HelloBody(this IStringLocalizer<ServerGlobal> stringLocalizer)
        {
            if (stringLocalizer == null)
            {
                throw new ArgumentNullException(nameof(stringLocalizer));
            }

            return stringLocalizer[nameof(HelloBody)];
        }
    }
}
