using System;
using UnityEngine;

namespace FineGameDesign.UI
{
    [Serializable]
    public struct AnswerFeedbackView
    {
        public Animator animator;
        public string correctAnimationName;
        public string wrongAnimationName;
        internal bool correct;
    }

    public sealed class AnswerFeedbackPublisher : MonoBehaviour
    {
        public delegate void FeedbackComplete(bool correct);

        public event FeedbackComplete OnComplete;

        [SerializeField]
        private AnswerFeedbackView m_View = default;

        public void DisplayFeedback(bool correct)
        {
            m_View.correct = correct;

            string animationName = correct ?
                m_View.correctAnimationName :
                m_View.wrongAnimationName;

            m_View.animator.Play(animationName, -1, 0f);
        }

        /// <summary>
        /// Callable by a Mecanim animation event. Example:
        /// <a href="../../Assets/Animations/FallacyRecognitionFeedback_CorrectOpen.anim">
        /// CorrectOpen
        /// </a>
        /// </summary>
        public void PublishComplete()
        {
            if (OnComplete == null)
            {
                return;
            }

            OnComplete.Invoke(m_View.correct);
        }
    }
}
