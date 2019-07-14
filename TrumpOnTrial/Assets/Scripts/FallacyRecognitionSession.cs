using System;

namespace FineGameDesign.FallacyRecognition
{
    /// <summary>
    /// TODO: Bridges evaluator and lister.
    /// Tests:
    /// <see cref="FineGameDesign.Tests.FallacyRecognition.FallacyRecognitionSessionTests"/>
    /// </summary>
    [Serializable]
    public struct FallacyRecognitionSession
    {
        private ArgumentEvaluator m_Evaluator;
        public ArgumentEvaluator Evaluator
        {
            get { return m_Evaluator; }
        }

        private FallacyLister m_Lister;
        public FallacyLister Lister
        {
            get { return m_Lister; }
        }

        public void Init()
        {
            m_Evaluator = new ArgumentEvaluator();
            m_Lister = new FallacyLister();
        }
    }
}

