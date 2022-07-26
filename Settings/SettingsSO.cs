using UnityEngine;

namespace AssetSizeDetector
{
    [CreateAssetMenu(fileName = "Settings", menuName = "AssetSizeDetector/AssetSizeDetectorSettings", order = 1)]
    public class SettingsSO : ScriptableObject
    {
        public SettingDto SettingDto;
    }
}