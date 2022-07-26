using System.Collections.Generic;
using System.Linq;

namespace AssetSizeDetector
{
    public class AssetInfoFilter : IAssetInfoFilter
    {
        private readonly List<IAssetInfoFilter> _assetInfoFilters;

        public AssetInfoFilter()
        {
            _assetInfoFilters = new List<IAssetInfoFilter>();
        }

        public bool IsAssetCorrect(AssetInfo assetInfo)
        {
            return _assetInfoFilters.All(assetInfoFilter => assetInfoFilter.IsAssetCorrect(assetInfo));
        }

        public void Add(IAssetInfoFilter assetInfoFilter)
        {
            _assetInfoFilters.Add(assetInfoFilter);
        }
    }
}