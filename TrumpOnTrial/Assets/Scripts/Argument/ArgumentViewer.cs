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
        private ArgumentView m_ArgumentView = default;

        [SerializeField]
        private AnswerFeedbackPublisher m_Feedback = default;

        [SerializeField]
        private SessionPerformanceAnimator m_SessionPerformance = default;

        [SerializeField]
        private GotoProgressAnimator m_ProgressAnimator = default;

        [SerializeField]
        private GotoProgressAnimator m_OpponentProgressAnimator = default;

        private ArgumentEvaluator m_Evaluator = new ArgumentEvaluator();
        public ArgumentEvaluator Evaluator
        {
            get { return m_Evaluator; }
        }

        private ArgumentEvaluator.Evaluate m_DisplayFeedbackAction;
        private ArgumentEvaluator.Evaluate DisplayFeedbackAction
        {
            get
            {
                if (m_DisplayFeedbackAction == null)
                {
                    m_DisplayFeedbackAction = DisplayFeedback;
                }
                return m_DisplayFeedbackAction;
            }
        }

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
            m_Evaluator.OnPopulated -= OnPopulated;
            m_Evaluator.OnPopulated += OnPopulated;

            m_Evaluator.OnEvaluated -= DisplayFeedbackAction;
            m_Evaluator.OnEvaluated += DisplayFeedbackAction;

            m_Evaluator.OnSessionEnded -= OnSessionEnded;
            m_Evaluator.OnSessionEnded += OnSessionEnded;

            m_Feedback.OnComplete -= OnFeedbackComplete;
            m_Feedback.OnComplete += OnFeedbackComplete;
        }

        private void OnDisable()
        {
            m_Feedback.OnComplete -= OnFeedbackComplete;
            m_Evaluator.OnPopulated -= OnPopulated;
            m_Evaluator.OnEvaluated -= DisplayFeedbackAction;
            m_Evaluator.OnSessionEnded -= OnSessionEnded;
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

        private void PopulateArgument(Argument argument, ArgumentRange rangeNotUsed)
        {
            PopulateText(argument, m_ArgumentView);

            m_ArgumentView.userInterfaceAnimator.Play(m_ArgumentView.openAnimationName);
        }

        private static void PopulateText(Argument argument, ArgumentView argumentView)
        {
            argumentView.argumentText.text = argument.argumentText;
        }

        private void DisplayFeedback(bool correct)
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

        private void DisplaySessionPerformance()
        {
            m_SessionPerformance.Display(m_ProgressAnimator.AnimatedProgress);
        }
    }
}
