using FineGameDesign.Utils;
using System;
using UnityEngine;

namespace FineGameDesign.Argument
{
    public sealed class ArgumentParser : MonoBehaviour
    {
        [SerializeField]
        private TextAsset m_ArgumentsCsv;

        [SerializeField]
        private string m_ArgumentTextColumnName = "argumentText";
        private int m_ArgumentTextColumn;

        [SerializeField]
        private string m_CorrectFallacyOptionTextColumnName = "correctFallacyOptionText";
        private int m_CorrectFallacyOptionTextColumn;

        private string[][] m_ArgumentsTable;

        private Argument[] m_Arguments;
        public Argument[] Arguments
        {
            get { return m_Arguments; }
        }

        private int m_NumArguments;

        public static ArgumentParser instance;

        private void Awake()
        {
            instance = this;
        }

        public void ParseArguments()
        {
            string csvText = m_ArgumentsCsv.text;
            
            m_ArgumentsTable = StringUtil.ParseCsv(csvText, "\t");
            m_NumArguments = m_ArgumentsTable.Length - 1;
            m_Arguments = new Argument[m_NumArguments];
            string[] header = m_ArgumentsTable[0];
            m_ArgumentTextColumn = Array.IndexOf(header, m_ArgumentTextColumnName);
            m_CorrectFallacyOptionTextColumn = Array.IndexOf(header, m_CorrectFallacyOptionTextColumnName);
            Debug.Assert(m_ArgumentTextColumn >= 0 && m_CorrectFallacyOptionTextColumn >= 0,
                "ArgumentParser: header[0]=" + header[0],
                context: this
            );
            for (int index = 0; index < m_NumArguments; ++index)
            {
                int rowIndex = index + 1;
                string[] row = m_ArgumentsTable[rowIndex];
                string argumentText = row[m_ArgumentTextColumn];
                string correctFallacyOptionText = row[m_CorrectFallacyOptionTextColumn];
                var argument = new Argument();
                argument.argumentText = argumentText;
                argument.correctFallacyOptionText = correctFallacyOptionText;
                m_Arguments[index] = argument;
            }
        }
    }
}
