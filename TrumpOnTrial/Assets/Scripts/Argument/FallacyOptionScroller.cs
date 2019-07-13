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
        private ArgumentEvaluator.Populate OnArgumentPopulated
        {
            get
            {
                if (m_OnArgumentPopulated == null)
                {
                    m_OnArgumentPopulated = SnapToFirst;
                }
                return m_OnArgumentPopulated;
            }
        }


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
            m_ArgumentViewer.Evaluator.OnPopulated -= OnArgumentPopulated;
            m_ArgumentViewer.Evaluator.OnPopulated += OnArgumentPopulated;
        }

        private void RemoveListener()
        {
            m_ArgumentViewer.Evaluator.OnPopulated -= OnArgumentPopulated;
        }

        private void SnapToFirst(Argument argumentNotUsed, ArgumentRange rangeNotUsed)
        {
            ScrollRectSnapper.SnapToFirst(ref m_ScrollView);
        }
    }
}
