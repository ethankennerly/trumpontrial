using System;
using System.Collections.Generic;
using UnityEngine;

namespace FineGameDesign.FallacyRecognition
{
    [Serializable]
    public struct OptionDifficulty
    {
        public int numDistractors;
        public int minDistractors;
        public int maxDistractors;
        public int addDistractorsPerCorrect;
        public string requiredFallacyText;
        public List<Fallacy> options;
        internal bool correct;
        internal Fallacy[] fallacies;
    }

    [Serializable]
    public sealed class FallacyLister
    {
        public delegate void Populate(List<Fallacy> fallacies);
        public event Populate OnOptionsChanged;

        [SerializeField]
        private OptionDifficulty m_Difficulty = default;
        public OptionDifficulty Difficulty
        {
            get { return m_Difficulty; }
            set { m_Difficulty = value; }
        }

        /// <summary>
        /// Defaults min and max distractors.
        /// Otherwise, when not set in editor, no distractors.
        /// </summary>
        public void PopulateFallacies(Fallacy[] fallacies)
        {
            m_Difficulty.fallacies = fallacies;

            if (m_Difficulty.minDistractors <= 0)
            {
                m_Difficulty.minDistractors = 1;
            }
            if (m_Difficulty.maxDistractors <= 0)
            {
                m_Difficulty.maxDistractors = fallacies.Length;
            }

            AdjustNumDistractors(false);
            Adjust(null);
        }

        public void Adjust(string requiredFallacyText)
        {
            m_Difficulty.requiredFallacyText = requiredFallacyText;
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

            if (m_Difficulty.maxDistractors > m_Difficulty.fallacies.Length)
            {
                m_Difficulty.maxDistractors = m_Difficulty.fallacies.Length;
            }
            m_Difficulty.numDistractors = (int)Mathf.Clamp(
                m_Difficulty.numDistractors,
                m_Difficulty.minDistractors,
                m_Difficulty.maxDistractors
            );
        }

        /// <summary>
        /// Selects options with required text and distractor fallacies.
        /// Maintains original order of fallacies.
        /// </summary>
        private void PopulateOptions(ref OptionDifficulty difficulty)
        {
            if (difficulty.options == null)
            {
                difficulty.options = new List<Fallacy>(difficulty.fallacies.Length);
            }
            difficulty.options.Clear();

            int numDistractors = 0;
            foreach (Fallacy fallacy in difficulty.fallacies)
            {
                if (difficulty.requiredFallacyText != fallacy.optionText)
                {
                    numDistractors++;
                    if (numDistractors > difficulty.numDistractors)
                    {
                        continue;
                    }
                }

                difficulty.options.Add(fallacy);
            }

            if (OnOptionsChanged != null)
            {
                OnOptionsChanged.Invoke(difficulty.options);
            }
        }
    }
}
