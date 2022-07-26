using System.Collections.Generic;
using Scroll;
using UnityEditor;
using UnityEngine;

namespace AssetSizeDetector
{
    public class AssetSizeDetectorEditorWindow : EditorWindow
    {
        private readonly ScrollViewWithPages _scrollViewWithPages = new(25);
        private AssetParserFacadeFactory _assetParserFacadeFactory;
        private IAssetParserFacade _localAssetParserFacade;
        private IAssetParserFacade _editorAssetParserFacade;
        private IAssetParserFacade _assetParserFacade;
        
        private string _rootAssetsPath = "Assets/";
        private List<AssetInfo> _assetsInfo;

        private bool _isWorkWithEditorLog;
        private bool _isInitialized;
        
        private SettingsSO SettingsSo
        {
            get
            {
                if (_settingsSo == null)
                {
                    _settingsSo = Resources.Load<SettingsSO>("AssetSizeDetectorSettings");
                    if (_settingsSo == null)
                    {
                        _settingsSo = CreateInstance<SettingsSO>();
                        _settingsSo.SettingDto = new SettingDto();
                        AssetDatabase.CreateAsset(_settingsSo, "Assets/Resources/AssetSizeDetectorSettings.asset");
                        AssetDatabase.SaveAssets();
                    }
                }

                return _settingsSo;
            }
        }
        private SettingsSO _settingsSo;

        private SettingDto Settings => _settings ??= SettingsSo.SettingDto;
        private SettingDto _settings;
        
        [MenuItem("ThirdParty/AssetSizeDetector")]
        private static void Init()
        {
            var window = (AssetSizeDetectorEditorWindow) GetWindow(typeof(AssetSizeDetectorEditorWindow));
            window.titleContent = new GUIContent("AssetSizeDetector");
            window.Show();
        }

        private void TryInitialize()
        {
            if (_isInitialized) return;

            _assetParserFacadeFactory ??= new AssetParserFacadeFactory(Settings);
            _localAssetParserFacade ??= _assetParserFacadeFactory.GetLocalAssetParserFacade();
            _editorAssetParserFacade ??= _assetParserFacadeFactory.GetEditorLogParserFacade();
            _isInitialized = true;
        }
        
        private void OnGUI()
        {
            TryInitialize();
            
            _isWorkWithEditorLog = EditorGUILayout.Toggle("Working with editor log", _isWorkWithEditorLog);

            _assetParserFacade = _isWorkWithEditorLog ? _editorAssetParserFacade : _localAssetParserFacade;

            if (_isWorkWithEditorLog)
            {
                EditorGUILayout.HelpBox("Copy the editor's log to some folder to correct working", MessageType.Info);
                if (GUILayout.Button("Open editor log"))
                    EditorExtensions.ShowInExplorer(EditorLogAssetPathProvider.PathToEditorLog);
            }
           
            DrawSelectionPath();
            DrawIsPathValid();
            EditorGUILayout.BeginHorizontal();
            DrawFind();
            DrawSettings();
            EditorGUILayout.EndHorizontal();
            DrawFoundAssets();
        }

        private void DrawSelectionPath()
        {
            EditorGUILayout.BeginHorizontal();
            _rootAssetsPath = EditorGUILayout.TextField("Root asset path", _rootAssetsPath);
            if (GUILayout.Button("Select"))
            {
                if (_isWorkWithEditorLog)
                    _rootAssetsPath = EditorUtility.OpenFilePanel("Editor.log", "Assets/", "log");
                else
                    _rootAssetsPath = EditorUtility.OpenFolderPanel("Choose root assets", "Assets/", "");
            }
            _rootAssetsPath = _assetParserFacade.FormatPath(_rootAssetsPath);
            Repaint();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawIsPathValid()
        {
            var isPathNotValid = !_assetParserFacade.IsPathValid(_rootAssetsPath);
            if (isPathNotValid)
                EditorGUILayout.HelpBox("Path isn't valid", MessageType.Warning);
            
            GUI.enabled = !isPathNotValid;
        }
        
        private void DrawFind()
        {
            if (GUILayout.Button("Find", GUILayout.Width(100), GUILayout.Height(25)))
                _assetsInfo = _assetParserFacade.GetAssetsInfo(_rootAssetsPath);
        }
        
        private void DrawSettings()
        {
            if (GUILayout.Button("Open settings", GUILayout.Width(100), GUILayout.Height(25)))
                EditorExtensions.PingAndSelectObject(SettingsSo);
        }
        
        private void DrawFoundAssets()
        {
            _scrollViewWithPages.Init(_assetsInfo);
            _scrollViewWithPages.BeginScrollView();
            
            for (var i = _scrollViewWithPages.LeftBorder; i < _scrollViewWithPages.RightBorder; i++)
            {
                var assetInfo = _assetsInfo[i];
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField("Name", assetInfo.Name);
                EditorGUILayout.TextField("Size", assetInfo.Size.ToString());
                if (GUILayout.Button("Open"))
                {
                    var file = AssetDatabase.LoadMainAssetAtPath(assetInfo.Path);
                    EditorExtensions.PingAndSelectObject(file);
                }
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    _assetsInfo.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            _scrollViewWithPages.EndScrollView();
            _scrollViewWithPages.DrawButtons();
        }
    }
}