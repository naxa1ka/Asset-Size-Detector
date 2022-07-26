using System;
using System.Collections.Generic;

namespace AssetSizeDetector
{
    [Serializable]
    public class SettingDto
    {
        public SettingDto()
        {
            TargetExtensions = new List<string> { ".cs", ".png" };
            MinMaxFileSizeSettings = new MinMaxFileSizeSettings()
            {
                MaxFileSize = ByteSize.MB * 10,
                MinFileSize = ByteSize.KB
            };
        }

        public MinMaxFileSizeSettings MinMaxFileSizeSettings;
        public List<string> TargetExtensions;
    }
    
    [Serializable]
    public class MinMaxFileSizeSettings
    {
        public long MinFileSize;
        public long MaxFileSize;
    }
}