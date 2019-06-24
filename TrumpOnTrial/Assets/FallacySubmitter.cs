using System;
using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct SubmitView
    {
        public Button submitButton;
    }

    public sealed class FallacySubmitter : MonoBehaviour
    {
        public delegate void Submit(string fallacyOptionText);

        public static event Submit OnSubmitted;

        [SerializeField]
        private SubmitView m_SubmitView;

        [SerializeField]
        private FallacyOptionViewer m_OptionViewer;

        private string m_FallacyOptionText;
        private float m_ButtonY;
        private bool m_OptionInRange;

        private void OnEnable()
        {
            m_SubmitView.submitButton.onClick.AddListener(OnSubmit);
            m_ButtonY = m_SubmitView.submitButton.transform.position.y;
        }

        private void OnDisable()
        {
            m_SubmitView.submitButton.onClick.RemoveListener(OnSubmit);
        }

        private void Update()
        {
            if (m_SubmitView.submitButton == null)
                return;

            m_FallacyOptionText = m_OptionViewer.GetOptionTextOverlappingY(m_ButtonY);
            m_OptionInRange = m_FallacyOptionText != null;
            m_SubmitView.submitButton.interactable = m_OptionInRange;
            m_SubmitView.submitButton.gameObject.SetActive(m_OptionInRange);
        }

        private void OnSubmit()
        {
            if (OnSubmitted != null)
                OnSubmitted.Invoke(m_FallacyOptionText);
        }
    }
}
