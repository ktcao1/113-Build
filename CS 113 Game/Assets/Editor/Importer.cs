using UnityEditor;
using UnityEngine;
 
//Throw this inside YourGame/Assets/Editor/Importer.cs
public class Importer: AssetPostprocessor {
 
    const int PostProcessOrder = 0;
 
    public override int GetPostprocessOrder () {
        return PostProcessOrder;
    }
 
    void OnPreprocessTexture () {
        var textureImporter = assetImporter as TextureImporter;
 
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.spritePixelsPerUnit = 16;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed; 
        //textureImporter.filterMode = FilterMode.Bilinear;
        //textureImporter.filterMode = FilterMode.Trilinear;
    }
}
