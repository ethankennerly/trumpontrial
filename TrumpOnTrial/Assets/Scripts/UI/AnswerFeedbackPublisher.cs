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
    }

    public sealed class AnswerFeedbackPublisher : MonoBehaviour
    {
        public delegate void FeedbackComplete();

        public static event FeedbackComplete OnComplete;

        [SerializeField]
        private AnswerFeedbackView m_View;

        public void DisplayFeedback(bool correct)
        {
            string animationName = correct ?
                m_View.correctAnimationName :
                m_View.wrongAnimationName;

            m_View.animator.Play(animationName, -1, 0f);
        }

        public void PublishComplete()
        {
            if (OnComplete == null)
            {
                return;
            }

            OnComplete.Invoke();
        }
    }
}
