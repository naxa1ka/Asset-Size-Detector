using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Scroll
{
    public class ScrollViewWithPages
    {
        private readonly int _amountUnitsInOnePage;
        private ICollection _collection;
        private Vector2 _scrollPosition;
        private int _currentPage;

        private int MinBorder => 1;
        private int MaxBorder => Mathf.CeilToInt(_collection.Count / (float)_amountUnitsInOnePage);

        public int LeftBorder { get; private set; }
        public int RightBorder { get; private set; }

        public ScrollViewWithPages(int amountUnitsInOnePage) => _amountUnitsInOnePage = amountUnitsInOnePage;

        public void BeginScrollView() => _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        public void EndScrollView() => EditorGUILayout.EndScrollView();

        public void Init(ICollection collection)
        {
            var prevCollection = _collection;
            _collection = collection;

            if (collection != null && (prevCollection == null || !prevCollection.Equals(collection)))
                SetPage(1);
        }

        public void DrawButtons()
        {
            if (_collection == null)
            {
                EditorGUILayout.HelpBox("Collection is null", MessageType.Error);
                return;
            }

            if (_collection.Count == 0)
            {
                EditorGUILayout.HelpBox("Collection is empty", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginHorizontal();

            GUI.enabled = _currentPage != MinBorder;
            if (GUILayout.Button("<<"))
                SetPage(0);
            if (GUILayout.Button("<"))
                SetPage(_currentPage - 1);
            GUI.enabled = true;

            EditorGUILayout.LabelField(_currentPage.ToString(), GUI.skin.button);

            GUI.enabled = _currentPage != MaxBorder;
            if (GUILayout.Button(">"))
                SetPage(_currentPage + 1);
            if (GUILayout.Button(">>"))
                SetPage(_collection.Count);
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        private void SetPage(int page)
        {
            _currentPage = page;

            ClampPage();

            LeftBorder = _amountUnitsInOnePage * (_currentPage - 1);
            RightBorder = _amountUnitsInOnePage * _currentPage;

            if (LeftBorder < 0)
                LeftBorder = 0;            
            if (RightBorder >= _collection.Count)
                RightBorder = _collection.Count;
        }

        private void ClampPage() => _currentPage = Mathf.Clamp(_currentPage, MinBorder, MaxBorder);
    }
}