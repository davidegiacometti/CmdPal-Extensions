# PowerToys Command Palette Extensions

This monorepo is the home for my [PowerToys Command Palette](https://learn.microsoft.com/windows/powertoys/command-palette/overview) extensions.

| | Extension | Description |
| --- | --- | --- |
| <img src="src/EdgeFavoritesExtension/Assets/EdgeFavorites.svg" alt="Edge Favorites Logo" height="40"> | Edge Favorites | Search Microsoft Edge favorites. Based on the existing [PowerToys Run Edge Favorite plugin](https://github.com/davidegiacometti/PowerToys-Run-EdgeFavorite). |
| <img src="src/VisualStudioExtension/Assets/VisualStudio.svg" alt="Visual Studio Logo" height="40"> | Visual Studio | Search Visual Studio recents. Based on the existing [PowerToys Run Visual Studio plugin](https://github.com/davidegiacometti/PowerToys-Run-VisualStudio). |

## Installation

### Command Palette

You can install the extensions directly from Command Palette.

### WinGet

You can install each extension with [WinGet](https://learn.microsoft.com/windows/package-manager/winget/) from PowerShell or another terminal:

**Edge Favorites**

```powershell
winget install davidegiacometti.EdgeFavoritesForCmdPal
```

**Visual Studio**

```powershell
winget install davidegiacometti.VisualStudioForCmdPal
```

### Microsoft Store

- [Edge Favorites](https://apps.microsoft.com/detail/9nqwbr1dj2pq)
- [Visual Studio](https://apps.microsoft.com/detail/9p0p3jxp7m3g)

### GitHub Releases

You can install the extensions manually using the MSIX packages available in the [GitHub releases](https://github.com/davidegiacometti/CmdPal-Extensions/releases).

## Contributing

- **New Extensions:** I’m not accepting PRs for new extensions. If you have a new idea, feel free to create your own project!
- **Fixes & Improvements:** I do accept PRs for bug fixes and improvements to existing code. However, to avoid the risk of your PR being rejected, please ensure that you file an issue and receive feedback before starting any work.
