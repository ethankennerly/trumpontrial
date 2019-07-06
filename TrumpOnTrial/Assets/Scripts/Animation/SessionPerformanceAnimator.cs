using System;
using TMPro;
using UnityEngine;

namespace FineGameDesign.Animation
{
    [Serializable]
    public struct ResultsUI
    {
        public Animator animator;
        public string animationName;
        public TMP_Text accuracyText;
    }
    
    public sealed class SessionPerformanceAnimator : MonoBehaviour
    {
        [SerializeField]
        private ResultsUI m_ResultsUI;

        [SerializeField]
        private AnimatedProgress m_Results;

        private SessionPerformance.Summarize m_OnSummarized;
        private SessionPerformance.Summarize OnSummarized
        {
            get
            {
                if (m_OnSummarized == null)
                {
                    m_OnSummarized = Display;
                }
                return m_OnSummarized;
            }
        }

        private void OnEnable()
        {
            SessionPerformance.OnSummarized -= OnSummarized;
            SessionPerformance.OnSummarized += OnSummarized;
        }

        private void OnDisable()
        {
            SessionPerformance.OnSummarized -= OnSummarized;
        }

        private void Display(AnimatedProgress results)
        {
            m_Results = results;
            int accuracyPercent = (int)Mathf.Round(100 * results.progress);
            m_ResultsUI.accuracyText.text = accuracyPercent.ToString() + "%";
            m_ResultsUI.animator.Play(m_ResultsUI.animationName, -1, 0f);
        }
    }

    public static class SessionPerformance
    {
        public delegate void Summarize(AnimatedProgress results);
        public static event Summarize OnSummarized;

        public static void Publish(AnimatedProgress results)
        {
            if (OnSummarized != null)
            {
                OnSummarized.Invoke(results);
            }
        }
    }
}
