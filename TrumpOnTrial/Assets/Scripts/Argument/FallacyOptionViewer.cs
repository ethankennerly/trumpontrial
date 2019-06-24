using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct OptionView
    {
        public TMP_Text optionText;
        public GameObject optionRoot;
    }

    public sealed class FallacyOptionViewer : MonoBehaviour
    {
        [SerializeField]
        private FallacyParser m_Parser;

        [SerializeField]
        private OptionView[] m_OptionViews;

        private void Awake()
        {
            m_Parser.ParseFallacies();
            PopulateTexts(m_Parser.Fallacies, m_OptionViews);
        }

        /// <summary>
        /// Copies each text and shows each root.
        /// Hides any options beyond number of fallacies.
        /// </summary>
        private static void PopulateTexts(Fallacy[] fallacies,
            OptionView[] optionViews)
        {
            int numFallacies = fallacies.Length;
            int numOptions = optionViews.Length;
            Debug.Assert(numOptions >= numFallacies,
                "Expected at least " + numOptions + " option views " +
                "equal or greater than " + numFallacies + " fallacies."
            );
            if (numOptions < numFallacies)
            {
                numFallacies = numOptions;
            }

            for (int optionIndex = 0; optionIndex < numFallacies; ++optionIndex)
            {
                OptionView optionView = optionViews[optionIndex];
                Fallacy fallacy = fallacies[optionIndex];
                optionView.optionText.text = fallacy.optionText;
                optionView.optionRoot.SetActive(true);
            }

            for (int optionIndex = numFallacies; optionIndex < numOptions; ++optionIndex)
            {
                OptionView optionView = optionViews[optionIndex];
                optionView.optionRoot.SetActive(false);
                optionView.optionText.text = "";
            }
        }

        private Vector3[] m_OptionWorldCornersCorners = new Vector3[4];
        private OptionView m_OverlappingOption;
        private float m_OverlappingY;

        public string GetOptionTextOverlappingY(float y)
        {
            m_OverlappingY = y;
            m_OverlappingOption = default(OptionView);
            foreach (OptionView optionView in m_OptionViews)
            {
                RectTransform optionRect = optionView.optionRoot.GetComponent<RectTransform>();
                optionRect.GetWorldCorners(m_OptionWorldCornersCorners);
                float bottom = m_OptionWorldCornersCorners[0].y;
                float top = m_OptionWorldCornersCorners[1].y;
                if (y < bottom || y > top)
                    continue;

                m_OverlappingOption = optionView;
                return optionView.optionText.text;
            }

            return null;
        }
    }
}
