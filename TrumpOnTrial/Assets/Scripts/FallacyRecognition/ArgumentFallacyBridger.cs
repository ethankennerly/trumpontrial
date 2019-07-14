using System;
using UnityEngine;

namespace FineGameDesign.FallacyRecognition
{
    [Serializable]
    public struct ArgumentFallacyBridge
    {
        public ArgumentViewer argumentViewer;
        public FallacyOptionViewer optionViewer;
    }

    public sealed class ArgumentFallacyBridger : MonoBehaviour
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

        private FallacyOptionViewer.SelectText m_OnTextSelected;
        private FallacyOptionViewer.SelectText OnTextSelected
        {
            get
            {
                if (m_OnTextSelected == null)
                {
                    m_OnTextSelected = Evaluator.EvaluateFallacy;
                }
                return m_OnTextSelected;
            }
        }

        private void OnEnable()
        {
            Evaluator.OnPopulated -= OnPopulated;
            Evaluator.OnPopulated += OnPopulated;
            Evaluator.OnEvaluated -= OnEvaluated;
            Evaluator.OnEvaluated += OnEvaluated;

            m_Bridge.optionViewer.OnTextSelected -= OnTextSelected;
            m_Bridge.optionViewer.OnTextSelected += OnTextSelected;
        }

        private void OnDisable()
        {
            Evaluator.OnEvaluated -= OnEvaluated;
            Evaluator.OnPopulated -= OnPopulated;

            m_Bridge.optionViewer.OnTextSelected -= OnTextSelected;
        }

        private void PopulateFallacyLister(Argument argument, ArgumentRange range)
        {
            m_Bridge.optionViewer.Lister.Adjust(argument.correctFallacyOptionText);
        }
    }
}
