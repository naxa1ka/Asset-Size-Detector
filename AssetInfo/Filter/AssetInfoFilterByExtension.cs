using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetSizeDetector
{
    public class AssetInfoFilterByExtension : IAssetInfoFilter
    {
        private readonly List<string> _targetExtension;

        public AssetInfoFilterByExtension(List<string> targetExtension)
        {
            _targetExtension = targetExtension;
        }

        public bool IsAssetCorrect(AssetInfo assetInfo)
        {
            var assetExtension = Path.GetExtension(assetInfo.Path);
            return _targetExtension.Any(extension => assetExtension.Equals(extension));
        }
    }
}