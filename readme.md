# Sunrise Blf Tool
This tool allows you to edit blf files used by Halo, which include data like network configuration and matchmaking playlists.

## How To Use
The tool is currently setup to deal with title storage data as a whole, and built for Halo 3.<br>
The launch arguments are as follows:

`<desired output type (blf or json)> <input folder> <output folder>`

For example<br>
`blf ".\title storage\json" ".\title stoage\blf"`

## How it works
The tool will convert files to and from json using the Newtonsoft json library.
Hashes are ommitted from the JSON files, instead these are couputed as the tool converts to BLF.
The online manifest fime (onmf) is not converted, instead it is rebuilt as it only contains hashes.
Map variants are not currently supported and are copied as is.
