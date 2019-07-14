using FineGameDesign.Utils;
using System;

namespace FineGameDesign.FallacyRecognition
{
    /// <summary>
    /// Parses CSV into arguments.
    /// Tests:
    /// <see cref="FineGameDesign.Tests.FallacyRecognition.ArgumentsTableTests"/>
    /// </summary>
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

        private int numParses;
        public int NumParses
        {
            get { return numParses; }
        }

        public void ParseCsv(string csvText, string delimiter = "\t")
        {
            if (numParses > 0)
            {
                if (this.csvText == csvText)
                {
                    return;
                }
            }
            numParses++;
            this.csvText = csvText;
            
            argumentsTable = StringUtil.ParseCsv(csvText, delimiter);
            numArguments = argumentsTable.Length - 1;
            arguments = new Argument[numArguments];
            string[] header = argumentsTable[0];
            argumentTextColumn = Array.IndexOf(header, argumentTextColumnName);
            correctFallacyOptionTextColumn = Array.IndexOf(header, correctFallacyOptionTextColumnName);
            if (argumentTextColumn < 0 || correctFallacyOptionTextColumn < 0)
            {
                throw new InvalidOperationException(
                    "ArgumentsTable.Parse: A column name was not found. " +
                    "header[0]=" + header[0]
                );
            }

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
