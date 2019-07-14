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
            Evaluator.OnEvaluated -= OnEvaluated;
            Evaluator.OnEvaluated += OnEvaluated;
        }

        private void RemoveListeners()
        {
            Evaluator.OnEvaluated -= OnEvaluated;
        }
    }
}

