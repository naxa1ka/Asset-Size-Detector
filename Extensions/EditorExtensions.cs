using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetSizeDetector
{
    public static class EditorExtensions
    {
        public static void PingAndSelectObject(Object obj)
        {
            EditorGUILayout.ObjectField(obj, typeof(object), false);
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }

        public static void ShowInExplorer(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", "/select,"+path);
        }
        
        public static void ClampLongProp(SerializedProperty sp, long min = long.MinValue, long max = long.MaxValue)
        {
            if (min > max)
                (min, max) = (max, min);
            sp.longValue = MathExtensions.ClampLong(sp.longValue, min, max);
        }
        
        public static void ClampIntProp(SerializedProperty sp, int min = int.MinValue, int max = int.MaxValue)
        {
            if (min > max)
                (min, max) = (max, min);
            sp.intValue = Mathf.Clamp(sp.intValue, min, max);
        }

        public static int NearestNumber(this IEnumerable<int> list, int comparableValue)
        {
            return list.Aggregate((current, next) => Math.Abs(current - comparableValue) < Math.Abs(next - comparableValue) ? current : next);
        }

        public static int NearestNumber(this IEnumerable<int> list, long currentSpValue)
        {
            return NearestNumber(list, (int) currentSpValue);
        }
    }
}