using System.Runtime.InteropServices;
using static Tilengine.TLN;

namespace QueryLayer
{
    public class Program
    {
        private const int Width = 424;
        private const int Height = 240;
        private const int LayerAmount = 4;
        private static readonly string[] LayerTypes = { "undefined", "tiles", "objects", "bitmap" };

        public static void Main()
        {
            TLN_Init(Width, Height, LayerAmount, 0, 0);
            TLN_SetLoadPath("assets");
            TLN_LoadWorld("map.tmx", 0);

            // Retrieve info about the layers.
            for (var c = 0; c < LayerAmount; c += 1)
            {
                ShowLayerInfo(c);
            }

            // Retrieve tile at layer 1, row 13 column 3, flip horizontally and update to the same position.
            var tilemap = TLN_GetLayerTilemap(1);
            TLN_GetTilemapTile(tilemap, 13, 3, out var tile);
            tile.flags |= TLN_TileFlags.FLAG_FLIPX;
            TLN_SetTilemapTile(tilemap, 13, 3, tile);

            // Release resources.
            TLN_ReleaseWorld();
            TLN_Deinit();
        }

        private static void ShowLayerInfo(int layerIndex)
        {
            var type = TLN_GetLayerType(layerIndex);
            if (type == TLN_LayerType.LAYER_NONE)
                return;

            // General info
            Console.WriteLine("Layer {0} type: {1}", layerIndex, LayerTypes[(int)type]);
            Console.WriteLine("  size: {0}x{1} pixels", TLN_GetLayerWidth(layerIndex), TLN_GetLayerHeight(layerIndex));
            Console.WriteLine("  Palette: {0}", TLN_GetLayerPalette(layerIndex));

            // Specific info
            switch (type)
            {
                case TLN_LayerType.LAYER_TILE:
                    var tilemap = TLN_GetLayerTilemap(layerIndex);
                    Console.WriteLine("  Tileset: {0}", TLN_GetLayerTileset(layerIndex));
                    Console.WriteLine("  Tilemap: {0}", tilemap);
                    Console.WriteLine("  Tilemap dimensions: {0} rows, {1} columns", TLN_GetTilemapRows(tilemap), TLN_GetTilemapCols(tilemap));
                    break;

                case TLN_LayerType.LAYER_OBJECT:
                    var objectList = TLN_GetLayerObjects(layerIndex);
                    Console.WriteLine("  Tileset: {0}", TLN_GetLayerTileset(layerIndex));
                    Console.WriteLine("  Objects: {0}", objectList);
                    Console.WriteLine("  num_objects = {0}", TLN_GetListNumObjects(objectList));

                    // Initialize unmanaged memory to hold the struct.
                    var infoPtr = Marshal.AllocHGlobal(Marshal.SizeOf<TLN_ObjectInfo>());

                    // Iterate objects and get info on each with TLN_GetListObject()
                    var hasObject = TLN_GetListObject(objectList, infoPtr);
                    while (hasObject)
                    {
                        var info = Marshal.PtrToStructure<TLN_ObjectInfo>(infoPtr);
                        Console.WriteLine("    id:{0} gid:{1} name:\"{2}\" pos:({3},{4}) size:({5}x{6}) type:{7}",
                            info.id, info.gid, info.name, info.x, info.y, info.width, info.height, info.type);
                        hasObject = TLN_GetListObject(objectList, IntPtr.Zero);
                    }

                    // Free the unmanaged memory.
                    Marshal.FreeHGlobal(infoPtr);
                    break;

                case TLN_LayerType.LAYER_BITMAP:
                    Console.WriteLine("  Bitmap: {0}", TLN_GetLayerBitmap(layerIndex));
                    break;
            }
        }
    }
}