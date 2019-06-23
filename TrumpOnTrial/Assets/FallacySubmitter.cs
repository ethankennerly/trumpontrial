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
        public delegate void Submit();

        public static event Submit OnSubmitted;

        [SerializeField]
        private SubmitView m_SubmitView;

        private void OnEnable()
        {
            m_SubmitView.submitButton.onClick.AddListener(OnSubmit);
        }

        private void OnDisable()
        {
            m_SubmitView.submitButton.onClick.RemoveListener(OnSubmit);
        }

        private void OnSubmit()
        {
            if (OnSubmitted != null)
                OnSubmitted.Invoke();
        }
    }
}
