using FineGameDesign.Animation;
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
        public string closedAnimationName;
        public TMP_Text argumentText;
    }

    [Serializable]
    public struct ArgumentRange
    {
        public int start;
        public int current;
        public int end;
        public int length;
    }

    public sealed class ArgumentViewer : MonoBehaviour
    {
        public delegate void Evaluate(
            int argumentIndex, bool correct, string answerText);
        public static event Evaluate OnEvaluated;

        public delegate void Populate(int argumentIndex);
        public static event Populate OnPopulated;

        [SerializeField]
        private ArgumentParser m_Parser = null;

        [SerializeField]
        private ArgumentView m_ArgumentView = default(ArgumentView);

        [SerializeField]
        private AnswerFeedbackPublisher m_Feedback = null;

        [SerializeField]
        private GotoProgressAnimator m_ProgressAnimator = null;

        [SerializeField]
        private GotoProgressAnimator m_OpponentProgressAnimator = null;

        [SerializeField]
        private bool m_Verbose = false;

        [SerializeField]
        private FallacyLister m_FallacyLister = default;

        private ArgumentRange m_ArgumentRange;

        private bool m_Correct;

        private FallacySubmitter.Submit m_EvaluateFallacyDelegate;

        private Evaluate m_DisplayFeedbackAction;

        private AnswerFeedbackPublisher.FeedbackComplete m_OnFeedbackComplete;
        private AnswerFeedbackPublisher.FeedbackComplete OnFeedbackComplete
        {
            get
            {
                if (m_OnFeedbackComplete == null)
                {
                    m_OnFeedbackComplete = NextArgument;
                }
                return m_OnFeedbackComplete;
            }
        }

        private void Start()
        {
            m_Parser.ParseArguments();
            
            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.closedAnimationName);
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
                m_DisplayFeedbackAction = DisplayFeedback;
            }
            OnEvaluated -= m_DisplayFeedbackAction;
            OnEvaluated += m_DisplayFeedbackAction;

            AnswerFeedbackPublisher.OnComplete -= OnFeedbackComplete;
            AnswerFeedbackPublisher.OnComplete += OnFeedbackComplete;
        }

        private void OnDisable()
        {
            FallacySubmitter.OnSubmitted -= m_EvaluateFallacyDelegate;
            FallacyOptionViewer.OnTextSelected -= m_EvaluateFallacyDelegate;
            AnswerFeedbackPublisher.OnComplete -= OnFeedbackComplete;
            OnEvaluated -= m_DisplayFeedbackAction;
        }

        private void EvaluateFallacy(string fallacyOptionText)
        {
            Argument argument = m_Parser.Arguments[m_ArgumentRange.current];
            m_Correct = fallacyOptionText == argument.correctFallacyOptionText;

            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.closeAnimationName);
            if (m_Correct)
            {
                m_ProgressAnimator.AddQuantity(1f);
            }
            else
            {
                m_OpponentProgressAnimator.AddQuantity(1f);
            }

            if (OnEvaluated != null)
            {
                OnEvaluated.Invoke(
                    m_ArgumentRange.current, m_Correct, fallacyOptionText);
            }
        }

        private void DisplayFeedback(int argumentIndex, bool correct, string answerText)
        {
            if (m_Feedback == null)
            {
                Debug.Assert(m_Feedback != null,
                    "Expected feedback defined.",
                    context: this);
                return;
            }
            m_Feedback.DisplayFeedback(correct);
        }

        public void ConfigureEasy()
        {
            ConfigureRange(0, 3);
            ConfigureProgress(m_ArgumentRange.length);
        }

        public void ConfigureHard()
        {
            ConfigureRange(3, 18);
            ConfigureProgress(m_ArgumentRange.length);
        }

        private void ConfigureRange(int start, int end)
        {
            m_ArgumentRange.start = start;
            m_ArgumentRange.end = end;
            m_ArgumentRange.length = end - start;
            m_ArgumentRange.current = start - 1;
        }

        private void ConfigureProgress(int numArguments)
        {
            m_ProgressAnimator.ResetTotal(numArguments);
            m_OpponentProgressAnimator.ResetTotal(numArguments);
        }

        public void StartArguments()
        {
            m_ArgumentRange.current = m_ArgumentRange.start - 1;
            NextArgument(true);
        }

        private void NextArgument(bool correct)
        {
            if (m_Verbose)
            {
                Debug.Log("NextArgument", context: this);
            }

            m_ArgumentRange.current++;
            if (m_ArgumentRange.current >= m_ArgumentRange.end)
            {
                SessionPerformance.Publish(m_ProgressAnimator.AnimatedProgress);
                return;
            }

            Argument argument = m_Parser.Arguments[m_ArgumentRange.current];
            PopulateText(argument, m_ArgumentView);

            m_FallacyLister.Adjust(argument.correctFallacyOptionText, m_Correct);

            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.openAnimationName);

            if (OnPopulated != null)
            {
                OnPopulated.Invoke(m_ArgumentRange.current);
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
