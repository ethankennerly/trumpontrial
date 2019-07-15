using System;

namespace FineGameDesign.FallacyRecognition
{
    /// <summary>
    /// Fallacy Lister subscribes to from Argument Evaluator.
    /// Tests:
    /// <see cref="FineGameDesign.Tests.FallacyRecognition.EvaluatorListerBridgeTests"/>
    /// </summary>
    public sealed class EvaluatorListerBridge
    {
        private ArgumentEvaluator m_Evaluator;
        public ArgumentEvaluator Evaluator
        {
            get { return m_Evaluator; }
            set { m_Evaluator = value; }
        }

        private FallacyLister m_Lister;
        public FallacyLister Lister
        {
            get { return m_Lister; }
            set { m_Lister = value; }
        }

        private ArgumentEvaluator.Populate m_OnPopulated;
        private ArgumentEvaluator.Populate OnPopulated
        {
            get
            {
                if (m_OnPopulated == null)
                {
                    m_OnPopulated = PopulateFallacyLister;
                }
                return m_OnPopulated;
            }
        }

        private ArgumentEvaluator.Evaluate m_OnEvaluated;
        private ArgumentEvaluator.Evaluate OnEvaluated
        {
            get
            {
                if (m_OnEvaluated == null)
                {
                    m_OnEvaluated = m_Lister.AdjustNumDistractors;
                }
                return m_OnEvaluated;
            }
        }

        public void AddListeners()
        {
            Evaluator.OnPopulated -= OnPopulated;
            Evaluator.OnPopulated += OnPopulated;
            Evaluator.OnEvaluated -= OnEvaluated;
            Evaluator.OnEvaluated += OnEvaluated;
        }

        public void RemoveListeners()
        {
            Evaluator.OnPopulated -= OnPopulated;
            Evaluator.OnEvaluated -= OnEvaluated;
        }

        private void PopulateFallacyLister(Argument argument, ArgumentRange range)
        {
            Lister.Adjust(argument.correctFallacyOptionText);
        }
    }
}

