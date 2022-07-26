namespace AssetSizeDetector
{
    public interface IAssetInfoFilter
    {
        public bool IsAssetCorrect(AssetInfo assetInfo);
    }
}