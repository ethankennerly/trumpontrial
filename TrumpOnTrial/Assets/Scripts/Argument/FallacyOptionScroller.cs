using FineGameDesign.UI;
using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    public sealed class FallacyOptionScroller : MonoBehaviour
    {
        [SerializeField]
        private ScrollView m_ScrollView = default;

        [SerializeField]
        private ArgumentViewer m_ArgumentViewer = default;

        private ArgumentEvaluator.Populate m_OnArgumentPopulated;

        private void OnEnable()
        {
            AddListener();
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void AddListener()
        {
            if (m_OnArgumentPopulated == null)
            {
                m_OnArgumentPopulated = SnapToFirst;
            }

            m_ArgumentViewer.Evaluator.OnPopulated -= m_OnArgumentPopulated;
            m_ArgumentViewer.Evaluator.OnPopulated += m_OnArgumentPopulated;
        }

        private void RemoveListener()
        {
            m_ArgumentViewer.Evaluator.OnPopulated -= m_OnArgumentPopulated;
        }

        private void SnapToFirst(Argument argumentNotUsed, ArgumentRange rangeNotUsed)
        {
            ScrollRectSnapper.SnapToFirst(ref m_ScrollView);
        }
    }
}
