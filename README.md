This application allows to save the currently selected text on Windows with a keyboard shortcut. The text is saved in a Markdown file named after the window title where the text was selected.

For example, if you select text in a Chrome tab named "Github - Some Repository" and press the shortcut (<kbd>Ctrl</kbd> + <kbd>Q</kbd> by default), the text will be saved in a local file named "Github - Some Repository.md". If the file already exists, the text will be appended to the file with a separator.

This is a quick way to save highlights while reading, and becomes especially useful when paired with something like [Obsidian](https://obsidian.md/).

### Installation

1. Install .NET 6.0
2. Build the app in Visual Studio
3. Run HighlightHotkey.exe


### Configuration

You can specify both the keyboard shortcut and the folder that will contain the files (e.g. your Obsidian vault).

To do so, create `HighlightHotkey/appsettings.json` with the following structure:

```
{
  "Settings": {
    "HighlightsFolderPath": "C:\\Users\\You\\Obsidian\\MyVault",
    "Hotkey": "Control+H"
  }
}
```

Then build and restart HighlightHotkey.exe.