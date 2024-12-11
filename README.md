DVMusicEdit
===========

This tool allows you to organize and manage your Derail Valley tapes and radio stations.

## Key Features

- Edit all 10 tape playlists
- Edit the radio playlist
- Reordering of items within the playlist
- Computation of track length for tapes
- Import and conversion of media that is not MP3 or OGG
- Media preview

## Installation

This is a portable application, just download and launch it.

You can download the executable from the release section or via gitload:

https://gitload.net/AyrA/DVMusicEdit

## .NET

You will need the .NET framework 4.8, but it's likely already installed.
If not, you can download it here: https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net481-web-installer

## Usage

This is a simple Windows UI application.
It's pretty much "double click and go".

On the left side of the window are all playlists,
and in the main part you edit the playlist.

The application tries to find your Derail Valley installation automatically.
If this fails, you will be presented with a dialog box where you can search for it.

## Dependencies

Some actions need ffmpeg to work.
It's automatically downloaded if you don't yet have it.

If you think something is wrong with ffmpeg or the download got interrupted,
you can use the "More..." button to redownload it again.

## Keyboard shortcuts

Common keyboard shortcuts are implemented:

- `[ENTER]`: Edit playlist details of the current entry
- `[P]`: Play the current entry
- `[INSERT]`: Add a new track
- `[DELETE]`: Remove the selected tracks
- `[ALT]`+`[UP]`: Move the selected tracks up
- `[ALT]`+`[DOWN]`: Move the selected tracks down
- `[CTRL]`+`[R]`: Reload playlists from disk
- `[CTRL]`+`[S]`: Save changes
