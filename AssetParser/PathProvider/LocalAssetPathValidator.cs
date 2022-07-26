using System.IO;
using UnityEngine;

namespace AssetSizeDetector
{
    public class LocalAssetPathProvider : IPathProvider
    {
        public bool IsPathValid(string path)
        {
            return Directory.Exists(path) && path.StartsWith("Assets/");   
        }

        public string FormatPath(string path)
        {
            var formatPath = path.Replace(Application.dataPath, "Assets/");
            return formatPath.Replace("//", "/");
        }
    }
}