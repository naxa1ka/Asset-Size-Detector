using System.Collections.Generic;

namespace AssetSizeDetector
{
    public interface IAssetParser
    {
        List<AssetInfo> GetAssetsInfo(string path);
    }
}