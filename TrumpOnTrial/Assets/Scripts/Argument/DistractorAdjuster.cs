using System;
using UnityEngine;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct ArgumentFallacyBridge
    {
        public ArgumentViewer argumentViewer;
        public FallacyOptionViewer optionViewer;
    }

    public sealed class DistractorAdjuster : MonoBehaviour
    {
        [SerializeField]
        private ArgumentFallacyBridge m_Bridge = default;

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
                    m_OnEvaluated = m_Bridge.optionViewer.Lister.AdjustNumDistractors;
                }
                return m_OnEvaluated;
            }
        }

        private ArgumentEvaluator m_Evaluator;
        private ArgumentEvaluator Evaluator
        {
            get
            {
                if (m_Evaluator == null)
                {
                    m_Evaluator = m_Bridge.argumentViewer.Evaluator;
                }
                return m_Evaluator;
            }
        }

        private void OnEnable()
        {
            Evaluator.OnPopulated -= OnPopulated;
            Evaluator.OnPopulated += OnPopulated;
            Evaluator.OnEvaluated -= OnEvaluated;
            Evaluator.OnEvaluated += OnEvaluated;
        }

        private void OnDisable()
        {
            Evaluator.OnEvaluated -= OnEvaluated;
            Evaluator.OnPopulated -= OnPopulated;
        }

        private void PopulateFallacyLister(Argument argument, ArgumentRange range)
        {
            m_Bridge.optionViewer.Lister.Adjust(argument.correctFallacyOptionText);
        }
    }
}
