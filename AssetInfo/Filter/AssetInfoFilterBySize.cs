namespace AssetSizeDetector
{
    public class AssetInfoFilterBySize : IAssetInfoFilter
    {
        private readonly MinMaxFileSizeSettings _minMaxFileSizeSettings;


        public AssetInfoFilterBySize(MinMaxFileSizeSettings minMaxFileSizeSettings)
        {
            _minMaxFileSizeSettings = minMaxFileSizeSettings;
        }

        public bool IsAssetCorrect(AssetInfo assetInfo)
        {
            return _minMaxFileSizeSettings.MaxFileSize > assetInfo.Size.ToB() && assetInfo.Size.ToB() > _minMaxFileSizeSettings.MinFileSize;
        }
    }
}