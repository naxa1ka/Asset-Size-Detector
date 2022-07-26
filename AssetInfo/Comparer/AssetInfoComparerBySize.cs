using System.Collections.Generic;

namespace AssetSizeDetector
{
    public class AssetInfoComparerBySize : IComparer<AssetInfo>
    {
        public int Compare(AssetInfo x, AssetInfo y)
        {
            return y.Size.ToB().CompareTo(x.Size.ToB());
        }
    }
}