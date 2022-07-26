namespace AssetSizeDetector
{
    public interface IPathProvider
    {
        bool IsPathValid(string path);
        string FormatPath(string path);
    }
}