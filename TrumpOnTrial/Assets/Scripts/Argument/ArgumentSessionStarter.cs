using FineGameDesign.Animation;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct SessionStartUI
    {
        public Button startEasyButton;
        public Button startHardButton;
        public ArgumentViewer argumentViewer;
        public string startUICloseAnimationName;
        internal Animator startUIAnimator;
    }

    [RequireComponent(typeof(Animator))]
    public sealed class ArgumentSessionStarter : MonoBehaviour
    {
        [SerializeField]
        private SessionStartUI m_StartUI = default;

        private void OnEnable()
        {
            m_StartUI.startEasyButton.onClick.RemoveListener(StartEasy);
            m_StartUI.startEasyButton.onClick.AddListener(StartEasy);
            m_StartUI.startHardButton.onClick.RemoveListener(StartHard);
            m_StartUI.startHardButton.onClick.AddListener(StartHard);
            m_StartUI.startUIAnimator = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            m_StartUI.startEasyButton.onClick.RemoveListener(StartEasy);
            m_StartUI.startHardButton.onClick.RemoveListener(StartHard);
        }

        private void StartEasy()
        {
            m_StartUI.argumentViewer.ConfigureEasy();
            Close();
        }

        private void StartHard()
        {
            m_StartUI.argumentViewer.ConfigureHard();
            Close();
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
            m_StartUI.argumentViewer.StartArguments();
        }
    }
}
