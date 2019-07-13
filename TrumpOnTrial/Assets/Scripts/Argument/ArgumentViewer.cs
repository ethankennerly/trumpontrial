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

    public sealed class ArgumentViewer : MonoBehaviour
    {
        [SerializeField]
        private ArgumentParser m_Parser = default;

        [SerializeField]
        private FallacyOptionViewer m_OptionViewer = default;

        [SerializeField]
        private ArgumentView m_ArgumentView = default;

        [SerializeField]
        private AnswerFeedbackPublisher m_Feedback = default;

        [SerializeField]
        private SessionPerformanceAnimator m_SessionPerformance = default;

        [SerializeField]
        private GotoProgressAnimator m_ProgressAnimator = default;

        [SerializeField]
        private GotoProgressAnimator m_OpponentProgressAnimator = default;

        [SerializeField]
        private FallacyLister m_FallacyLister = default;

        private ArgumentEvaluator m_Evaluator = new ArgumentEvaluator();
        public ArgumentEvaluator Evaluator
        {
            get { return m_Evaluator; }
        }

        private FallacyOptionViewer.SelectText m_EvaluateFallacyDelegate;

        private ArgumentEvaluator.Evaluate m_DisplayFeedbackAction;

        private AnswerFeedbackPublisher.FeedbackComplete m_OnFeedbackComplete;
        private AnswerFeedbackPublisher.FeedbackComplete OnFeedbackComplete
        {
            get
            {
                if (m_OnFeedbackComplete == null)
                {
                    m_OnFeedbackComplete = m_Evaluator.NextArgument;
                }
                return m_OnFeedbackComplete;
            }
        }

        private ArgumentEvaluator.Populate m_OnPopulated;
        private ArgumentEvaluator.Populate OnPopulated
        {
            get
            {
                if (m_OnPopulated == null)
                {
                    m_OnPopulated = PopulateArgument;
                }
                return m_OnPopulated;
            }
        }

        private ArgumentEvaluator.EndSession m_OnSessionEnded;
        private ArgumentEvaluator.EndSession OnSessionEnded
        {
            get
            {
                if (m_OnSessionEnded == null)
                {
                    m_OnSessionEnded = DisplaySessionPerformance;
                }
                return m_OnSessionEnded;
            }
        }

        private void Start()
        {
            m_Parser.ParseArguments();
            m_Evaluator.Arguments = m_Parser.Arguments;
            
            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.closedAnimationName);
        }

        private void OnEnable()
        {
            if (m_EvaluateFallacyDelegate == null)
            {
                m_EvaluateFallacyDelegate = m_Evaluator.EvaluateFallacy;
            }

            m_OptionViewer.OnTextSelected -= m_EvaluateFallacyDelegate;
            m_OptionViewer.OnTextSelected += m_EvaluateFallacyDelegate;

            m_Evaluator.OnPopulated -= OnPopulated;
            m_Evaluator.OnPopulated += OnPopulated;

            if (m_DisplayFeedbackAction == null)
            {
                m_DisplayFeedbackAction = DisplayFeedback;
            }
            m_Evaluator.OnEvaluated -= m_DisplayFeedbackAction;
            m_Evaluator.OnEvaluated += m_DisplayFeedbackAction;

            m_Evaluator.OnSessionEnded -= OnSessionEnded;
            m_Evaluator.OnSessionEnded += OnSessionEnded;

            m_Feedback.OnComplete -= OnFeedbackComplete;
            m_Feedback.OnComplete += OnFeedbackComplete;
        }

        private void OnDisable()
        {
            m_OptionViewer.OnTextSelected -= m_EvaluateFallacyDelegate;
            m_Feedback.OnComplete -= OnFeedbackComplete;
            m_Evaluator.OnPopulated -= OnPopulated;
            m_Evaluator.OnEvaluated -= m_DisplayFeedbackAction;
            m_Evaluator.OnSessionEnded -= OnSessionEnded;
        }

        private void DisplayFeedback(int argumentIndex, bool correct, string answerText)
        {
            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.closeAnimationName);
            if (correct)
            {
                m_ProgressAnimator.AddQuantity(1f);
            }
            else
            {
                m_OpponentProgressAnimator.AddQuantity(1f);
            }

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
            m_Evaluator.ConfigureRange(0, 3);
            ConfigureProgress(m_Evaluator.NumArgumentsInRange());
        }

        public void ConfigureHard()
        {
            m_Evaluator.ConfigureRange(3, 18);
            ConfigureProgress(m_Evaluator.NumArgumentsInRange());
        }

        private void ConfigureProgress(int numArguments)
        {
            m_ProgressAnimator.ResetTotal(numArguments);
            m_OpponentProgressAnimator.ResetTotal(numArguments);
        }

        public void StartArguments()
        {
            m_Evaluator.StartArguments();
        }

        private void PopulateArgument(int argumentIndexUnused)
        {
            Argument argument = m_Evaluator.CurrentArgument;
            PopulateText(argument, m_ArgumentView);

            m_FallacyLister.Adjust(argument.correctFallacyOptionText, m_Evaluator.Correct);

            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.openAnimationName);
        }

        private static void PopulateText(Argument argument, ArgumentView argumentView)
        {
            argumentView.argumentText.text = argument.argumentText;
        }

        private void DisplaySessionPerformance()
        {
            m_SessionPerformance.Display(m_ProgressAnimator.AnimatedProgress);
        }
    }
}
