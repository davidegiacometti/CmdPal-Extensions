# Windows Command Palette Extensions

This monorepo is the home for my Windows Command Palette extensions.

| Extension | Description |
| --- | --- |
| Visual Studio | Search Visual Studio recents. Based on the existing [PowerToys Run Visual Studio plugin](https://github.com/davidegiacometti/PowerToys-Run-VisualStudio). |
| Edge Favorites | Search Microsoft Edge favorites. Based on the existing [PowerToys Run  Edge Favorite plugin](https://github.com/davidegiacometti/PowerToys-Run-EdgeFavorite). |

## Installation

### Via Command Palette

You can install the extensions directly from Command Palette.

### Via WinGet

You can install the extensions manually via [WinGet](https://learn.microsoft.com/windows/package-manager/winget/) running the following commands from the command line / PowerShell:

```
winget install davidegiacometti.VisualStudioForCmdPal
winget install davidegiacometti.EdgeFavoritesForCmdPal
```

## Contributing

- **New Extensions:** I’m not accepting PRs for new extensions. If you have a new idea, feel free to create your own project!
- **Fixes & Improvements:** I do accept PRs for bug fixes and improvements to existing code. However, to avoid the risk of your PR being rejected, please ensure that you file an issue and receive feedback before starting any work.
