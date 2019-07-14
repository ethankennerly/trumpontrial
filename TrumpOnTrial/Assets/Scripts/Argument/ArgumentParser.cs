using FineGameDesign.Utils;
using System;
using UnityEngine;

namespace FineGameDesign.Argument
{
    public sealed class ArgumentParser : MonoBehaviour
    {
        [SerializeField]
        private TextAsset m_ArgumentsCsv = default;

        [SerializeField]
        private ArgumentsTable m_Table;

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

        private int m_NumArguments = default;
        public int NumArguments
        {
            get { return m_NumArguments; }
        }

        public void ParseArguments()
        {
            m_Table.ParseCsv(m_ArgumentsCsv.text); // TODO

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

    [Serializable]
    public struct ArgumentsTable
    {
        public string argumentTextColumnName;
        private int argumentTextColumn;

        public string correctFallacyOptionTextColumnName;
        private int correctFallacyOptionTextColumn;

        public int numArguments;
        public Argument[] arguments;
        private string[][] argumentsTable;
        private string csvText;

        public void ParseCsv(string csvText, string delimiter = "\t")
        {
            this.csvText = csvText;
            
            argumentsTable = StringUtil.ParseCsv(csvText, delimiter);
            numArguments = argumentsTable.Length - 1;
            arguments = new Argument[numArguments];
            string[] header = argumentsTable[0];
            argumentTextColumn = Array.IndexOf(header, argumentTextColumnName);
            correctFallacyOptionTextColumn = Array.IndexOf(header, correctFallacyOptionTextColumnName);
            Debug.Assert(
                argumentTextColumn >= 0 && correctFallacyOptionTextColumn >= 0,
                "ArgumentsTable.Parse: header[0]=" + header[0]
            );
            for (int index = 0; index < numArguments; ++index)
            {
                int rowIndex = index + 1;
                string[] row = argumentsTable[rowIndex];
                if (argumentTextColumn >= row.Length)
                {
                    throw new InvalidOperationException("Not enough columns in row index " + index);
                }
                string argumentText = row[argumentTextColumn];
                if (correctFallacyOptionTextColumn >= row.Length)
                {
                    throw new InvalidOperationException("Not enough columns in row index " + index);
                }
                string correctFallacyOptionText = row[correctFallacyOptionTextColumn];
                var argument = new Argument();
                argument.argumentText = argumentText;
                argument.correctFallacyOptionText = correctFallacyOptionText;
                arguments[index] = argument;
            }
        }
    }
}
