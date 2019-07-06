using FineGameDesign.UI;
using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    public sealed class FallacyOptionScroller : MonoBehaviour
    {
        [SerializeField]
        private ScrollView m_ScrollView;

        private ArgumentViewer.PopulateArgument m_OnArgumentPopulated;

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

            ArgumentViewer.OnArgumentPopulated -= m_OnArgumentPopulated;
            ArgumentViewer.OnArgumentPopulated += m_OnArgumentPopulated;
            m_OnArgumentPopulated();
        }

        private void RemoveListener()
        {
            ArgumentViewer.OnArgumentPopulated -= m_OnArgumentPopulated;
        }

        private void SnapToFirst()
        {
            ScrollRectSnapper.SnapToFirst(ref m_ScrollView);
        }
    }
}
