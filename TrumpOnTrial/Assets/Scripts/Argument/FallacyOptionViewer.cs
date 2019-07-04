using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct OptionView
    {
        public GameObject optionRoot;
        public TMP_Text optionText;
        public Button optionButton;
        public UnityAction onThisSelected;
    }

    public sealed class FallacyOptionViewer : MonoBehaviour
    {
        public static event FallacySubmitter.Submit OnTextSelected;

        [SerializeField]
        private FallacyParser m_Parser;

        [SerializeField]
        private OptionView[] m_OptionViews;

        private void OnEnable()
        {
            m_Parser.ParseFallaciesOnce();
            PopulateOptions(m_Parser.Fallacies, m_OptionViews);
        }

        private void OnDisable()
        {
            RemoveListeners(m_OptionViews);
        }

        /// <summary>
        /// Copies each text and shows each root.
        /// Hides any options beyond number of fallacies.
        ///
        /// Assigns option index and button subscribes to selected event.
        /// </summary>
        private static void PopulateOptions(Fallacy[] fallacies,
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
                AddListener(optionView, fallacy.optionText);
                optionView.optionRoot.SetActive(true);
            }

            for (int optionIndex = numFallacies; optionIndex < numOptions; ++optionIndex)
            {
                OptionView optionView = optionViews[optionIndex];
                optionView.optionRoot.SetActive(false);
                optionView.optionText.text = "";

                RemoveListener(optionView);
            }
        }

        private static void AddListener(OptionView optionView, string optionText)
        {
            if (optionView.onThisSelected == null ||
                optionView.optionText.text != optionText)
            {
                optionView.optionText.text = optionText;
                optionView.onThisSelected = () => SelectOptionText(optionText);
            }

            if (optionView.optionButton == null)
            {
                Debug.Assert(optionView.optionButton != null,
                    "Expected option button. optionText=" + optionText,
                    context: optionView.optionRoot
                );
                return;
            }
            optionView.optionButton.onClick.RemoveAllListeners();
            optionView.optionButton.onClick.AddListener(optionView.onThisSelected);
        }

        private static void RemoveListeners(OptionView[] optionViews)
        {
            foreach (OptionView optionView in optionViews)
            {
                RemoveListener(optionView);
            }
        }

        private static void RemoveListener(OptionView optionView)
        {
            if (optionView.onThisSelected == null)
            {
                return;
            }

            if (optionView.optionButton == null)
            {
                return;
            }

            optionView.optionButton.onClick.RemoveAllListeners();
        }

        private static void SelectOptionText(string selectedText)
        {
            if (OnTextSelected == null)
            {
                return;
            }
            
            OnTextSelected.Invoke(selectedText);
        }

        private Vector3[] m_OptionWorldCornersCorners = new Vector3[4];
        private OptionView m_OverlappingOption;
        private float m_OverlappingWorldY;

        public string GetOptionTextOverlappingY(float worldY)
        {
            m_OverlappingWorldY = worldY;
            m_OverlappingOption = default(OptionView);
            foreach (OptionView optionView in m_OptionViews)
            {
                RectTransform optionRect = optionView.optionRoot.GetComponent<RectTransform>();
                optionRect.GetWorldCorners(m_OptionWorldCornersCorners);
                float worldBottom = m_OptionWorldCornersCorners[0].y;
                float worldTop = m_OptionWorldCornersCorners[1].y;
                if (worldY < worldBottom || worldY > worldTop)
                    continue;

                m_OverlappingOption = optionView;
                return optionView.optionText.text;
            }

            return null;
        }
    }
}
