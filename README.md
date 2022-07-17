![Logo](Logo.png)

# CsTilenginePure
CsTilenginePure is an alternative C# binding for [Tilengine](https://github.com/megamarc/Tilengine). 
It is a direct 1:1 API translation of the original C library, so it is used and works exactly as its C counterpart. 
The naming schemes for this library will match those of the C library, with little-to-no concern for the standard C# style. 
Everything else will be as close to the C version as technically possible.

## Contents
* The ```src/``` directory contains the single `Tilengine.cs` module with the binding itself.

## Prerequisites
You will need to install the Tilengine native shared library separately. You can find instructions on how to do so at https://github.com/megamarc/Tilengine.

### Windows
.NET Framework 2.0 or later must be installed

### Linux/OSX
Mono tools and runtime must be installed. In Debian-based distros please execute the following command:
```
sudo apt-get install mono-mcs
```

## Installation
The C# binding for SDL2 is required. This binding is included as a submodule for this repository.

You have to make sure that `SDL2.cs` and `Tilengine.cs` are accessible from within your own project. This could be done by creating a submodule of this repository for your project with the recursive parameter set to true.

Alternatively, you could add the project in this repository as a reference to your project.

## Samples
The samples for this binding have been moved to its own repository to prevent multiple ```Main``` methods from interfering when using this binding as a submodule. 

These samples are now located at https://github.com/vonhoff/CsTilenginePure.Samples.

## Basic program
The following program requires assets from the [Platfomer sample](https://github.com/vonhoff/CsTilenginePure.Samples/tree/main/src/Platformer/assets). You need to include this directory in your binary folder to get the desired result.

The program does the following actions:
1. Initializes the engine with a resolution of 400x240, one layer, no sprites, and 20 animation slots.
2. Sets the loading path to the assets folder.
3. Loads a tilemap, which is an asset that contains background layer data.
4. Attaches the loaded tilemap to the allocated background layer.
5. Creates a display window with default parameters: windowed, auto scale, and CRT effect enabled.
6. Runs the window loop, updating the display at each iteration until the window is closed.

Source code:
```csharp
using static Tilengine.TLN;

public class Test
{
    public static void Main(string[] args)
    {
        TLN_Init(400, 240, 2, 1, 1);
        TLN_SetLoadPath("assets");
        var foreground = TLN_LoadTilemap("Sonic_md_fg1.tmx", null);
        TLN_SetLayerTilemap(0, foreground);

        TLN_CreateWindow(null, TLN_CreateWindowFlags.CWF_VSYNC);
        while (TLN_ProcessWindow())
        {
            TLN_DrawFrame(0);
        }
    }
}
```

Resulting output:

![Test](test.png)

## License
- Copyright &copy; 2022 Simon Vonhoff & Contributors - Provided under the permissive [MIT License](LICENSE).
