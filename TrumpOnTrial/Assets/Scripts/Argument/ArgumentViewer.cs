using System;
using TMPro;
using UnityEngine;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct ArgumentView
    {
        public TMP_Text argumentText;
    }

    public sealed class ArgumentViewer : MonoBehaviour
    {
        public delegate void Evaluate(bool correct);

        public static event Evaluate OnEvaluated;

        [SerializeField]
        private ArgumentParser m_Parser;

        [SerializeField]
        private ArgumentView m_ArgumentView;

        private int m_ArgumentIndex = -1;

        private bool m_Correct;

        private FallacySubmitter.Submit m_EvaluateFallacyDelegate;

        private void Awake()
        {
            m_Parser.ParseArguments();
            NextArgument();
        }

        private void OnEnable()
        {
            if (m_EvaluateFallacyDelegate == null)
                m_EvaluateFallacyDelegate = EvaluateFallacy;

            FallacySubmitter.OnSubmitted -= m_EvaluateFallacyDelegate;
            FallacySubmitter.OnSubmitted += m_EvaluateFallacyDelegate;
            FallacyOptionViewer.OnTextSelected -= m_EvaluateFallacyDelegate;
            FallacyOptionViewer.OnTextSelected += m_EvaluateFallacyDelegate;
        }

        private void OnDisable()
        {
            FallacySubmitter.OnSubmitted -= m_EvaluateFallacyDelegate;
            FallacyOptionViewer.OnTextSelected -= m_EvaluateFallacyDelegate;
        }

        private void EvaluateFallacy(string fallacyOptionText)
        {
            Argument argument = m_Parser.Arguments[m_ArgumentIndex];
            m_Correct = fallacyOptionText == argument.correctFallacyOptionText;

            if (OnEvaluated != null)
                OnEvaluated.Invoke(m_Correct);

            if (m_Correct)
                NextArgument();
        }

        private void NextArgument()
        {
            m_ArgumentIndex++;
            if (m_ArgumentIndex >= m_Parser.Arguments.Length)
                m_ArgumentIndex = 0;

            PopulateText(m_Parser.Arguments[m_ArgumentIndex], m_ArgumentView);
        }

        /// <summary>
        /// Copies each text.
        /// </summary>
        private static void PopulateText(Argument argument, ArgumentView argumentView)
        {
            argumentView.argumentText.text = argument.argumentText;
        }
    }
}
