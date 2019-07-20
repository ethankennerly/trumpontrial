using System;
using System.Collections.Generic;
using UnityEngine;

namespace FineGameDesign.UI
{
    [Serializable]
    public struct OptionDifficulty
    {
        public int numDistractors;
        public int minDistractors;
        public int maxDistractors;
        public int addDistractorsPerCorrect;
        public string requiredText;
        public List<string> options;
        public string[] possibleTexts;
        internal bool correct;
    }

    [Serializable]
    public sealed class TextLister
    {
        public delegate void Populate(List<string> optionTexts);
        public event Populate OnOptionsChanged;

        [SerializeField]
        private OptionDifficulty m_Difficulty = default;
        public OptionDifficulty Difficulty
        {
            get { return m_Difficulty; }
            set { m_Difficulty = value; }
        }

        public void PopulatePossibleTexts(string[] possibleTexts)
        {
            m_Difficulty.possibleTexts = possibleTexts;
            UpdatePossibleTexts();
        }

        /// <summary>
        /// Defaults min and max distractors.
        /// Otherwise, when not set in editor, no distractors.
        /// </summary>
        public void UpdatePossibleTexts()
        {
            if (m_Difficulty.minDistractors <= 0)
            {
                m_Difficulty.minDistractors = 1;
            }
            if (m_Difficulty.maxDistractors <= 0)
            {
                m_Difficulty.maxDistractors = m_Difficulty.possibleTexts.Length;
            }

            AdjustNumDistractors(false);
            Adjust(null);
        }

        public void Adjust(string requiredText)
        {
            m_Difficulty.requiredText = requiredText;
            PopulateOptions(ref m_Difficulty);
        }

        public void AdjustNumDistractors(bool correct)
        {
            m_Difficulty.correct = correct;
            if (correct)
            {
                m_Difficulty.numDistractors += m_Difficulty.addDistractorsPerCorrect;
            }
            else
            {
                m_Difficulty.numDistractors -= m_Difficulty.addDistractorsPerCorrect;
            }

            if (m_Difficulty.maxDistractors > m_Difficulty.possibleTexts.Length)
            {
                m_Difficulty.maxDistractors = m_Difficulty.possibleTexts.Length;
            }
            m_Difficulty.numDistractors = (int)Mathf.Clamp(
                m_Difficulty.numDistractors,
                m_Difficulty.minDistractors,
                m_Difficulty.maxDistractors
            );
        }

        /// <summary>
        /// Selects options with required text and distractor possibleTexts.
        /// Maintains original order of possibleTexts.
        /// </summary>
        private void PopulateOptions(ref OptionDifficulty difficulty)
        {
            if (difficulty.options == null)
            {
                difficulty.options = new List<string>(difficulty.possibleTexts.Length);
            }
            difficulty.options.Clear();

            int numDistractors = 0;
            foreach (string possibleText in difficulty.possibleTexts)
            {
                if (difficulty.requiredText != possibleText)
                {
                    numDistractors++;
                    if (numDistractors > difficulty.numDistractors)
                    {
                        continue;
                    }
                }

                difficulty.options.Add(possibleText);
            }

            if (OnOptionsChanged != null)
            {
                OnOptionsChanged.Invoke(difficulty.options);
            }
        }
    }
}
