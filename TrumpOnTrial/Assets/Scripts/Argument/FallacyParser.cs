using FineGameDesign.Utils;
using System;
using UnityEngine;

namespace FineGameDesign.Argument
{
    public sealed class FallacyParser : MonoBehaviour
    {
        [SerializeField]
        private TextAsset m_FallaciesCsv;

        [SerializeField]
        private string m_OptionTextColumnName = "optionText";
        private int m_OptionTextColumn;

        private string[][] m_FallaciesTable;

        private Fallacy[] m_Fallacies;
        public Fallacy[] Fallacies
        {
            get { return m_Fallacies; }
        }

        private int m_NumFallacies;

        public static FallacyParser instance;

        private void Awake()
        {
            instance = this;
        }

        public void ParseFallacies()
        {
            string csvText = m_FallaciesCsv.text;
            
            m_FallaciesTable = StringUtil.ParseCsv(csvText, "\t");
            m_NumFallacies = m_FallaciesTable.Length - 1;
            m_Fallacies = new Fallacy[m_NumFallacies];
            string[] header = m_FallaciesTable[0];
            m_OptionTextColumn = Array.IndexOf(header, m_OptionTextColumnName);
            for (int index = 0; index < m_NumFallacies; ++index)
            {
                int rowIndex = index + 1;
                string[] row = m_FallaciesTable[rowIndex];
                string optionText = row[m_OptionTextColumn];
                var fallacy = new Fallacy();
                fallacy.optionText = optionText;
                m_Fallacies[index] = fallacy;
            }
        }
    }
}
