using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct OptionDifficulty
    {
        public int numDistractors;
        public int maxDistractors;
        public int addDistractorsPerCorrect;
        internal bool correct;
        internal string requiredFallacyText;
        internal List<Fallacy> options;
    }

    public sealed class FallacyLister : MonoBehaviour
    {
        public delegate void Populate(List<Fallacy> fallacies);
        public event Populate OnOptionsChanged;

        [SerializeField]
        private FallacyParser m_Parser;

        [SerializeField]
        private OptionDifficulty m_Difficulty;
        public OptionDifficulty Difficulty
        {
            get { return m_Difficulty; }
        }

        private void Start()
        {
            m_Parser.ParseFallaciesOnce();

            Adjust(null, true);
        }

        public void Adjust(string requiredFallacyText, bool correct)
        {
            AdjustNumDistractors(correct);
            m_Difficulty.requiredFallacyText = requiredFallacyText;
            PopulateOptions(m_Parser.Fallacies, m_Difficulty);
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

            if (m_Difficulty.maxDistractors > m_Parser.Fallacies.Length)
            {
                m_Difficulty.maxDistractors = m_Parser.Fallacies.Length;
            }
            m_Difficulty.numDistractors = (int)Mathf.Clamp(
                m_Difficulty.numDistractors, 1, m_Difficulty.maxDistractors);
        }

        /// <summary>
        /// Selects options with required text and distractor fallacies.
        /// Maintains original order of fallacies.
        /// </summary>
        private void PopulateOptions(Fallacy[] fallacies, OptionDifficulty difficulty)
        {
            if (difficulty.options == null)
            {
                difficulty.options = new List<Fallacy>(fallacies.Length);
            }
            difficulty.options.Clear();

            int numDistractors = 0;
            foreach (Fallacy fallacy in fallacies)
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
