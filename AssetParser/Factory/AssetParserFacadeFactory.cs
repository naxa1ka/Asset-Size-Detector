namespace AssetSizeDetector
{
    public class AssetParserFacadeFactory
    {
        private readonly IAssetInfoFilter _assetInfoFilter;

        public AssetParserFacadeFactory(SettingDto settingDto)
        {
            var assetInfoFilter = new AssetInfoFilter();
            var assetInfoFilterByExtension = new AssetInfoFilterByExtension(settingDto.TargetExtensions);
            var assetInfoFilterBySize = new AssetInfoFilterBySize(settingDto.MinMaxFileSizeSettings);
            assetInfoFilter.Add(assetInfoFilterByExtension);
            assetInfoFilter.Add(assetInfoFilterBySize);
            _assetInfoFilter = assetInfoFilter;
        }

        public IAssetParserFacade GetEditorLogParserFacade()
        {
            return new AssetParserFacade(
                new EditorLogAssetPathProvider(),
                new EditorLogParser(),
                new AssetInfoComparerBySize(),
                _assetInfoFilter);
        }

        public IAssetParserFacade GetLocalAssetParserFacade()
        {
            return new AssetParserFacade(
                new LocalAssetPathProvider(),
                new LocalAssetParser(),
                new AssetInfoComparerBySize(),
                _assetInfoFilter);
        }
    }
}