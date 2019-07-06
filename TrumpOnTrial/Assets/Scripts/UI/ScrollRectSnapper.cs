using System;
using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.UI
{
    [Serializable]
    public struct ScrollView
    {
        public ScrollRect scrollRect;
        internal RectTransform target;
        internal Vector2 nextContentPosition;
    }

    public static class ScrollRectSnapper
    {
        public static void SnapToFirst(ref ScrollView scrollView)
        {
            scrollView.target = (RectTransform)GetChild(scrollView.scrollRect.content, 0);
            SnapToTarget(ref scrollView);
        }

        /// <remarks>
        /// Depends on target being the immediate child of content.
        /// And on content scale being 100%.
        ///
        /// Does not transform from world to local.
        /// Otherwise, that approach failed when reloading the active scene.
        /// <a href="https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui#_=_">
        /// How to scroll to a specific element in ScrollRect with Unity UI?
        /// </a>
        /// </remarks>
        public static void SnapToTarget(ref ScrollView scrollView)
        {
            ScrollRect scroll = scrollView.scrollRect;
            RectTransform target = scrollView.target;

            if (scroll == null)
            {
                Debug.LogError("Expected scroll was defined.");
                return;
            }

            RectTransform content = scroll.content;
            if (content == null)
            {
                Debug.LogError("Expected content defined in scroll=" + scroll);
                return;
            }
            Transform scrollTransform = scroll.transform;

            if (target == null)
            {
                Debug.LogError("Expected target defined for content=" + content);
                return;
            }

            Canvas.ForceUpdateCanvases();

            scrollView.nextContentPosition = -target.anchoredPosition;

            if (!scroll.horizontal)
            {
                scrollView.nextContentPosition.x = content.anchoredPosition.x;
            }
            if (!scroll.vertical)
            {
                scrollView.nextContentPosition.x = content.anchoredPosition.y;
            }
            content.anchoredPosition = scrollView.nextContentPosition;
        }

        public static Transform GetChild(Transform parent, int childIndex)
        {
            if (parent == null)
            {
                Debug.LogError("Expected parent defined.");
                return null;
            }

            int numChildren = parent.childCount;
            if (numChildren == 0)
            {
                Debug.LogError("Expected parent has a child. parent=" + parent);
                return null;
            }

            childIndex = Mathf.Clamp(childIndex, 0, numChildren - 1);
            Transform child = parent.GetChild(childIndex);
            return child;
        }
    }
}
