using FineGameDesign.Animation;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct SessionStartUI
    {
        public Button startButton;
        public ArgumentViewer argumentViewer;
        public string startUICloseAnimationName;
        internal Animator startUIAnimator;
    }

    [RequireComponent(typeof(Animator))]
    public sealed class ArgumentSessionStarter : MonoBehaviour
    {
        [SerializeField]
        private SessionStartUI m_StartUI;

        private void OnEnable()
        {
            m_StartUI.startButton.onClick.RemoveListener(Close);
            m_StartUI.startButton.onClick.AddListener(Close);
            m_StartUI.startUIAnimator = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            m_StartUI.startButton.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            m_StartUI.startUIAnimator.Play(m_StartUI.startUICloseAnimationName);
        }

        /// <remarks>
        /// Expects <see cref="FineGameDesign.Animation.AnimationEndPublisher"/>
        /// to send message "OnClosed".
        /// </remarks>
        public void OnClosed()
        {
            m_StartUI.argumentViewer.NextArgument();
        }
    }
}
