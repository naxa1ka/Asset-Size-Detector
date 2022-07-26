using System;
using System.IO;

namespace AssetSizeDetector
{
    public class EditorLogAssetPathProvider : IPathProvider
    {
        public static readonly string PathToEditorLog = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%\\Unity\\Editor\\Editor.log");
        
        public bool IsPathValid(string path)
        {
            return File.Exists(path);
        }

        public string FormatPath(string path)
        {
            return path;
        }
    }
}