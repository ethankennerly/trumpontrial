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
        private ResultsUI m_ResultsUI = default;

        [SerializeField]
        private AnimatedProgress m_Results = default;

        private void OnEnable()
        {
            m_ResultsUI.animator.speed = 0f;
        }

        private void OnDisable()
        {
            m_ResultsUI.animator.speed = 1f;
        }

        public void Display(AnimatedProgress results)
        {
            m_Results = results;
            int accuracyPercent = (int)Mathf.Round(100 * results.progress);
            m_ResultsUI.accuracyText.text = accuracyPercent.ToString() + "%";

            m_ResultsUI.animator.speed = 1f;
            m_ResultsUI.animator.Play(m_ResultsUI.animationName, -1, 0f);
        }
    }
}
