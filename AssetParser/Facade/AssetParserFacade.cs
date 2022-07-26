using System;
using System.Collections.Generic;
using System.IO;

namespace AssetSizeDetector
{
    public class AssetParserFacade : IAssetParserFacade
    {
        private readonly IPathProvider _pathProvider;
        private readonly IAssetParser _assetParser;
        private readonly IComparer<AssetInfo> _comparer;
        private readonly IAssetInfoFilter _assetInfoFilter;

        public AssetParserFacade(
            IPathProvider pathProvider,
            IAssetParser assetParser,
            IComparer<AssetInfo> comparer,
            IAssetInfoFilter assetInfoFilter)
        {
            _pathProvider = pathProvider;
            _assetParser = assetParser;
            _comparer = comparer;
            _assetInfoFilter = assetInfoFilter;
        }

        public bool IsPathValid(string path) => _pathProvider.IsPathValid(path);

        public string FormatPath(string path) => _pathProvider.FormatPath(path);

        public List<AssetInfo> GetAssetsInfo(string path)
        {
            List<AssetInfo> assetInfos;
            try
            {
                assetInfos = _assetParser.GetAssetsInfo(path);
            }
            catch (IOException)
            {
                throw new Exception(
                    $"Problems with the file... If you are using the editor's log in the unity folder, move the file to a local folder!");
            }
            
            var newList = new List<AssetInfo>();
            foreach (var assetInfo in assetInfos)
            {
                var isAssetCorrect = _assetInfoFilter.IsAssetCorrect(assetInfo);
                if (isAssetCorrect)
                    newList.SortedInsert(assetInfo, _comparer);   
            }
            return newList;
        }
    }
}