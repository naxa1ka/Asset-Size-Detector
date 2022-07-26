using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AssetSizeDetector
{
    public class LocalAssetParser : IAssetParser
    {
        private readonly List<AssetInfo> _assetInfos;

        public LocalAssetParser()
        {
            _assetInfos = new List<AssetInfo>();
        }

        public List<AssetInfo> GetAssetsInfo(string path)
        {
            _assetInfos.Clear();
            ProcessDirectory(path);
            return _assetInfos;
        }

        private void ProcessDirectory(string targetDirectory)
        {
            var directory = new DirectoryInfo(targetDirectory);

            foreach (var fileInfo in directory.GetFiles())
                ProcessFile(fileInfo);

            foreach (var directoryInfo in directory.GetDirectories())
                ProcessDirectory(directoryInfo.FullName);
        }

        private void ProcessFile(FileInfo file)
        {
            var assetInfo = new AssetInfo
            {
                Path = AssetsRelativePath(file.FullName),
                Name = file.Name,
                Size = new ByteSize(file.Length)
            };
            _assetInfos.Add(assetInfo);
        }

        private static string AssetsRelativePath(string absolutePath)
        {
            var dataPath = Application.dataPath.MakeOneFormatDirectorySeparators();
            if (absolutePath.StartsWith(dataPath))
            {
                return "Assets" + absolutePath.Substring(dataPath.Length);
            }

            throw new ArgumentException("Full path does not contain the current project's Assets folder", "absolutePath");
        }
    }
}