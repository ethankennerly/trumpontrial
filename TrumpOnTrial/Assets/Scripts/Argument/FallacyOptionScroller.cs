using FineGameDesign.UI;
using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    public sealed class FallacyOptionScroller : MonoBehaviour
    {
        [SerializeField]
        private ScrollView m_ScrollView;

        private ArgumentViewer.Populate m_OnArgumentPopulated;

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

            ArgumentViewer.OnPopulated -= m_OnArgumentPopulated;
            ArgumentViewer.OnPopulated += m_OnArgumentPopulated;
            m_OnArgumentPopulated(-1);
        }

        private void RemoveListener()
        {
            ArgumentViewer.OnPopulated -= m_OnArgumentPopulated;
        }

        private void SnapToFirst(int argumentIndexNotUsed)
        {
            ScrollRectSnapper.SnapToFirst(ref m_ScrollView);
        }
    }
}
