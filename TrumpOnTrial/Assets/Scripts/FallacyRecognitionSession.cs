using System;

namespace FineGameDesign.FallacyRecognition
{
    /// <summary>
    /// TODO: Bridges evaluator and lister.
    /// Tests:
    /// <see cref="FineGameDesign.Tests.FallacyRecognition.FallacyRecognitionSessionTests"/>
    /// </summary>
    [Serializable]
    public sealed class FallacyRecognitionSession
    {
        private ArgumentEvaluator m_Evaluator = new ArgumentEvaluator();
        public ArgumentEvaluator Evaluator
        {
            get { return m_Evaluator; }
        }

        private FallacyLister m_Lister = new FallacyLister();
        public FallacyLister Lister
        {
            get { return m_Lister; }
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

        public FallacyRecognitionSession()
        {
            AddListeners();
        }

        ~FallacyRecognitionSession()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            Evaluator.OnPopulated -= OnPopulated;
            Evaluator.OnPopulated += OnPopulated;
            Evaluator.OnEvaluated -= OnEvaluated;
            Evaluator.OnEvaluated += OnEvaluated;
        }

        private void RemoveListeners()
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

