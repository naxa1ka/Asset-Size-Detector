using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AssetSizeDetector
{
    [CustomEditor(typeof(SettingsSO))]
    public class SettingsSOCustomEditor : Editor
    {
        private readonly List<ButtonAction> _buttonActions = new()
        {
            new ButtonAction
            {
                Action = i => i * 1,
                Name = "+"
            },
            new ButtonAction
            {
                Action = i => i * -1,
                Name = "-"
            }
        };
        
        private struct ButtonAction
        {
            public string Name;
            public Func<long, long> Action;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var settingDto = serializedObject.FindProperty("SettingDto");
            var targetExtensions = settingDto.FindPropertyRelative("TargetExtensions");
            var minMaxFileSize = settingDto.FindPropertyRelative("MinMaxFileSizeSettings");
            var minFileSize = minMaxFileSize.FindPropertyRelative("MinFileSize");
            var maxFileSize = minMaxFileSize.FindPropertyRelative("MaxFileSize");

            for (var i = 0; i < targetExtensions.arraySize; i++)
            {
                var element = targetExtensions.GetArrayElementAtIndex(i);

                if (!Regex.IsMatch(element.stringValue, @"^\.\w*\w\b$"))
                {
                    EditorGUILayout.HelpBox($"Extension not correct {element.stringValue}", MessageType.Warning);
                }
            }

            EditorExtensions.ClampLongProp(minFileSize, 1, Math.Abs(maxFileSize.longValue));
            EditorExtensions.ClampLongProp(maxFileSize, 1);

            EditorGUILayout.PropertyField(targetExtensions);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Min file size:", new ByteSize(minFileSize.longValue).ToString());
            DrawAddFileSizeButton(minFileSize);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Max file size:", new ByteSize(maxFileSize.longValue).ToString());
            DrawAddFileSizeButton(maxFileSize);
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private struct SizeInfo
        {
            public string Name;
            public long Size;
        }

        private void DrawAddFileSizeButton(SerializedProperty sp)
        {
            var size = sp.longValue;
            
            //todo: think about this
            var checkByteSizeForDrawing = new Dictionary<Func<bool>, SizeInfo>();
            checkByteSizeForDrawing.Add(() => ByteSize.ToMB(size) > 0, new SizeInfo
            {
                Name = "MB",
                Size = ByteSize.MB
            });
            checkByteSizeForDrawing.Add(() => ByteSize.ToKB(size) > 0, new SizeInfo
            {
                Name = "KB",
                Size = ByteSize.KB
            });
            checkByteSizeForDrawing.Add(() => ByteSize.ToB(size) > 0, new SizeInfo
            {
                Name = "B",
                Size = ByteSize.B
            });

            var powersOfTwo = MathExtensions.GetListWithPowersOfTwo(1024);
            
            foreach (var (isByteSizeGreatZero, sizeInfo) in checkByteSizeForDrawing)
            {
                if (!isByteSizeGreatZero.Invoke()) continue;
                
                var currentSpValue = sp.longValue / sizeInfo.Size;
                var nearest = powersOfTwo.NearestNumber(currentSpValue);
                var index = powersOfTwo.IndexOf(nearest);
                
                foreach (var buttonAction in _buttonActions)
                {
                    DrawButton(buttonAction, index, sizeInfo);
                    if (index == 0) index = 1;
                    DrawButton(buttonAction, index - 1, sizeInfo);
                    if (index == 1) index = 2;
                    DrawButton(buttonAction, index - 2, sizeInfo);
                }
                
                break;
            }
            
            void DrawButton(ButtonAction buttonAction, int index, SizeInfo value)
            {
                if (GUILayout.Button($"{buttonAction.Name}{powersOfTwo[index]} {value.Name}"))
                {
                    sp.longValue += buttonAction.Action.Invoke(powersOfTwo[index] * value.Size);
                }
            }
        }
    }
}