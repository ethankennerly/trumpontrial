using System;
using System.Collections.Generic;
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

    [Serializable]
    public struct ContentSize
    {
        public RectTransform content;
        public float heightPerOption;
    }

    public sealed class FallacyOptionViewer : MonoBehaviour
    {
        public delegate void SelectText(string fallacyOptionText);

        public event SelectText OnTextSelected;

        [SerializeField]
        private FallacyParser m_Parser = default;

        [SerializeField]
        private FallacyLister m_Lister = default;
        public FallacyLister Lister
        {
            get { return m_Lister; }
        }

        [SerializeField]
        private ContentSize m_ContentSize = default;

        [SerializeField]
        private OptionView[] m_OptionViews = default;

        private FallacyLister.Populate m_OnOptionsChanged;
        private FallacyLister.Populate OnOptionsChanged
        {
            get
            {
                if (m_OnOptionsChanged == null)
                {
                    m_OnOptionsChanged = UpdateOptions;
                }
                return m_OnOptionsChanged;
            }
        }

        private void OnEnable()
        {
            m_Lister.OnOptionsChanged -= OnOptionsChanged;
            m_Lister.OnOptionsChanged += OnOptionsChanged;
            m_Parser.ParseFallaciesOnce();
            m_Lister.PopulateFallacies(m_Parser.Fallacies);
        }

        private void OnDisable()
        {
            m_Lister.OnOptionsChanged -= OnOptionsChanged;
            RemoveListeners(m_OptionViews);
        }

        private void UpdateOptions(List<Fallacy> fallacies)
        {
            PopulateOptions(fallacies, m_OptionViews);
            ResizeContent(m_ContentSize, fallacies.Count);
        }

        /// <summary>
        /// Copies each text and shows each root.
        /// Hides any options beyond number of fallacies.
        ///
        /// Assigns option index and button subscribes to selected event.
        /// </summary>
        private void PopulateOptions(
            List<Fallacy> fallacies,
            OptionView[] optionViews)
        {
            int numFallacies = fallacies == null ? 0 : fallacies.Count;
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

        private static void ResizeContent(ContentSize size, int numOptions, int borderOptions = 3)
        {
            int reservedOptions = numOptions + borderOptions;
            float height = size.heightPerOption * reservedOptions;
            Vector2 contentSize = size.content.sizeDelta;
            contentSize.y = height;
            size.content.sizeDelta = contentSize;
        }

        private void AddListener(OptionView optionView, string optionText)
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

        private void SelectOptionText(string selectedText)
        {
            if (OnTextSelected == null)
            {
                return;
            }
            
            OnTextSelected.Invoke(selectedText);
        }
    }
}
