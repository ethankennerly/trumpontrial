using FineGameDesign.UI;
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

        private EvaluatorListerBridge m_EvaluatorListerBridge = new EvaluatorListerBridge();

        private ArgumentEvaluator.Populate m_OnPopulated;
        private ArgumentEvaluator.Populate OnPopulated
        {
            get
            {
                if (m_OnPopulated == null)
                {
                    m_OnPopulated = PopulateTextLister;
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
                    m_OnEvaluated = Lister.AdjustNumDistractors;
                }
                return m_OnEvaluated;
            }
        }

        private TextLister m_Lister;
        private TextLister Lister
        {
            get
            {
                if (m_Lister == null)
                {
                    m_Lister = m_Bridge.optionViewer.Lister;
                }
                return m_Lister;
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
                    m_OnTextSelected = EvaluateFallacy;
                }
                return m_OnTextSelected;
            }
        }

        private void OnEnable()
        {
            m_EvaluatorListerBridge.Evaluator = Evaluator;
            m_EvaluatorListerBridge.Lister = Lister;
            m_EvaluatorListerBridge.AddListeners();

            m_Bridge.optionViewer.OnTextSelected -= OnTextSelected;
            m_Bridge.optionViewer.OnTextSelected += OnTextSelected;
        }

        private void OnDisable()
        {
            m_EvaluatorListerBridge.RemoveListeners();

            m_Bridge.optionViewer.OnTextSelected -= OnTextSelected;
        }

        private void PopulateTextLister(Argument argument, ArgumentRange range)
        {
            Lister.Adjust(argument.correctFallacyOptionText);
        }

        private void EvaluateFallacy(string selectedText)
        {
            Evaluator.EvaluateFallacy(selectedText);
        }
    }
}
