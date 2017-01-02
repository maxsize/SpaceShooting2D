using UnityEditor;
using System.IO;

public class ExportAssetBundles
{
    [MenuItem ("Assets/Build All AssetBundles")]
    static void BuildAll()
    {
        CheckDirectory("Assets/AssetBundles");
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    static void CheckDirectory(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
}