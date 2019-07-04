using FineGameDesign.UI;
using System;
using TMPro;
using UnityEngine;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct ArgumentView
    {
        public Animator userInterfaceAnimator;
        public string openAnimationName;
        public string closeAnimationName;
        public TMP_Text argumentText;
    }

    public sealed class ArgumentViewer : MonoBehaviour
    {
        public delegate void Evaluate(bool correct);
        public delegate void PopulateArgument();

        public static event Evaluate OnEvaluated;

        public static event PopulateArgument OnArgumentPopulated;

        [SerializeField]
        private ArgumentParser m_Parser;

        [SerializeField]
        private ArgumentView m_ArgumentView;

        [SerializeField]
        private AnswerFeedbackPublisher m_Feedback;

        private int m_ArgumentIndex = -1;

        private bool m_Correct;

        private FallacySubmitter.Submit m_EvaluateFallacyDelegate;

        private Evaluate m_DisplayFeedbackAction;

        private AnswerFeedbackPublisher.FeedbackComplete m_OnFeedbackComplete;

        private void Awake()
        {
            m_Parser.ParseArguments();
            NextArgument();
        }

        private void OnEnable()
        {
            if (m_EvaluateFallacyDelegate == null)
            {
                m_EvaluateFallacyDelegate = EvaluateFallacy;
            }

            FallacySubmitter.OnSubmitted -= m_EvaluateFallacyDelegate;
            FallacySubmitter.OnSubmitted += m_EvaluateFallacyDelegate;
            FallacyOptionViewer.OnTextSelected -= m_EvaluateFallacyDelegate;
            FallacyOptionViewer.OnTextSelected += m_EvaluateFallacyDelegate;

            if (m_DisplayFeedbackAction == null)
            {
                m_DisplayFeedbackAction = (bool correct) =>
                {
                    if (m_Feedback == null)
                    {
                        Debug.Assert(m_Feedback != null,
                            "Expected feedback defined.",
                            context: this);
                        return;
                    }
                    m_Feedback.DisplayFeedback(correct);
                };
            }
            OnEvaluated -= m_DisplayFeedbackAction;
            OnEvaluated += m_DisplayFeedbackAction;

            if (m_OnFeedbackComplete == null)
            {
                m_OnFeedbackComplete = NextArgument;
            }

            AnswerFeedbackPublisher.OnComplete -= m_OnFeedbackComplete;
            AnswerFeedbackPublisher.OnComplete += m_OnFeedbackComplete;
        }

        private void OnDisable()
        {
            FallacySubmitter.OnSubmitted -= m_EvaluateFallacyDelegate;
            FallacyOptionViewer.OnTextSelected -= m_EvaluateFallacyDelegate;
            AnswerFeedbackPublisher.OnComplete -= m_OnFeedbackComplete;
            OnEvaluated -= m_DisplayFeedbackAction;
        }

        private void EvaluateFallacy(string fallacyOptionText)
        {
            Argument argument = m_Parser.Arguments[m_ArgumentIndex];
            m_Correct = fallacyOptionText == argument.correctFallacyOptionText;

            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.closeAnimationName);

            if (OnEvaluated != null)
            {
                OnEvaluated.Invoke(m_Correct);
            }
        }

        private void NextArgument()
        {
            m_ArgumentIndex++;
            if (m_ArgumentIndex >= m_Parser.Arguments.Length)
            {
                m_ArgumentIndex = 0;
            }

            PopulateText(m_Parser.Arguments[m_ArgumentIndex], m_ArgumentView);

            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.openAnimationName);

            if (OnArgumentPopulated != null)
            {
                OnArgumentPopulated.Invoke();
            }
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
