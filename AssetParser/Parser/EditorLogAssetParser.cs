using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AssetSizeDetector
{
    public class EditorLogParser : IAssetParser
    {
        private const string Pattern = @"\s+?([\d.]+\s+?\w+?)\s+?[\d.%]+\s+?(.*)";
        private const string StartBorder = "Used Assets and files from the Resources folder";
        private const string EndBorder = "-------------------------------------------------------------------------------";
        
        private readonly List<AssetInfo> _assetInfos;
        
        public EditorLogParser()
        {
            _assetInfos = new List<AssetInfo>();
        }
        
        public List<AssetInfo> GetAssetsInfo(string path)
        {
            _assetInfos.Clear();
            
            var isStartAssetsInfo = false;
            var assetsInfo = new List<string>();
            using (var reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isStartAssetsInfo && line.Contains(EndBorder))
                        break;
                    
                    if (isStartAssetsInfo)
                        assetsInfo.Add(line);
                    
                    if (line.Contains(StartBorder))
                        isStartAssetsInfo = true;
                }
            }

            foreach (var asset in assetsInfo)
            {
                var match = Regex.Match(asset, Pattern);
                _assetInfos.Add(new AssetInfo
                {
                    Name = match.Groups[2].Value,
                    Path = match.Groups[2].Value,
                    Size = ByteSize.Parse(match.Groups[1].Value)
                });
            }

            return _assetInfos;
        }
    }
}