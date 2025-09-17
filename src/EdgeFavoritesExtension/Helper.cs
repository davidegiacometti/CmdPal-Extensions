// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace EdgeFavoritesExtension
{
    internal static class Helper
    {
        internal static readonly IconInfo ExtensionIcon = IconHelpers.FromRelativePath(@"Assets\EdgeFavorites.png");

        private static readonly IconInfo UndetectedIcon = new("\uE7BA");
        private static readonly IconInfo NotFoundIcon = new("\uE728");

        internal static ICommandItem GetEmptyContent(bool edgeChannelDetected)
        {
            var (icon, title, subtitle) = GetEmptyContentInfo(edgeChannelDetected);
            return new CommandItem(new NoOpCommand())
            {
                Icon = icon,
                Title = title,
                Subtitle = subtitle,
            };
        }

        private static (IconInfo Icon, string Title, string Subtitle) GetEmptyContentInfo(bool edgeChannelDetected) => edgeChannelDetected
            ? (NotFoundIcon, "EmptyContent_Search_Title".GetLocalized(), string.Empty)
            : (UndetectedIcon, "EmptyContent_Undetected_Title".GetLocalized(), "EmptyContent_Undetected_Subtitle".GetLocalized());
    }
}
