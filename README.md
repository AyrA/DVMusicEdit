# DVMusicEdit

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
If not, you can download it here:

https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net481-web-installer

## Usage

This is a simple Windows UI application.
It's pretty much "double click and go".

On the left side of the window are all playlists,
and in the main part you edit the playlist.

The application tries to find your Derail Valley installation automatically.
If this fails, you will be presented with a dialog box where you can search for it.

## Supported Formats

The game supports MP3 and OGG audio formats and streams.

DVMusicEdit allows you to select other formats, including WAV, WEBM, MP4, M4A, FLAC, AAC, WMA.
Unsupported formats are automatically converted into DV compatible files using FFmpeg.
The file selection dialog has an "All Files" entry in the type drop down below the file name field.
Selecting this makes it show all files regardless of their type.
This allows you to add any file as long as it's supported by ffmpeg,
which includes some obscure and less common file formats.

If you supply a video file, the first audio track will be converted.

If you supply an invalid or broken file,
you may end up with a corrupt audio file in the conversion folder.
In that case, check the file name from the playlist in DVMusicEdit, then delete the file from the disk and playlist.

### Converted Files

Converted files are placed in a subdirectory created in the playlist folder.
Currently this folder is `DerailValley_Data\StreamingAssets\music\converted`.

This folder may be deleted when you uninstall the game or force a file integrity check,
but since it's inside of the folder that also holds playlists, the playlists likely get deleted too.

DVMusicEdit will never delete any audio files.

## Network Streams

The game supports MP3 and OGG data streamed over HTTP(s) only.

FFmpeg is pretty smart when it comes to playing back network streams.
If a stream works in DVMusicEdit but not in the game itself,
it's likely not a pure HTTP stream but may use other technologies like HLS, which the game doesn't supports,
or it's in an audio format the game doesn't understands.

Should you have such a stream it's best to just open it in a dedicated media player.

## FFmpeg

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
