using FineGameDesign.Utils;
using System;

namespace FineGameDesign.FallacyRecognition
{
    /// <summary>
    /// Parses CSV into fallacies.
    /// Tests:
    /// <see cref="FineGameDesign.Tests.FallacyRecognition.FallaciesTableTests"/>
    /// </summary>
    [Serializable]
    public struct FallaciesTable
    {
        public string optionTextColumnName;
        private int optionTextColumn;

        public int numFallacies;
        public Fallacy[] fallacies;
        private string[][] fallaciesTable;
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
            
            fallaciesTable = StringUtil.ParseCsv(csvText, delimiter);
            numFallacies = fallaciesTable.Length - 1;
            fallacies = new Fallacy[numFallacies];
            string[] header = fallaciesTable[0];
            optionTextColumn = Array.IndexOf(header, optionTextColumnName);
            if (optionTextColumn < 0)
            {
                throw new InvalidOperationException(
                    "FallaciesTable.Parse: A column name was not found. " +
                    "header[0]=" + header[0]
                );
            }

            for (int index = 0; index < numFallacies; ++index)
            {
                int rowIndex = index + 1;
                string[] row = fallaciesTable[rowIndex];
                if (optionTextColumn >= row.Length)
                {
                    throw new InvalidOperationException("Not enough columns in row index " + index);
                }
                string optionText = row[optionTextColumn];
                var fallacy = new Fallacy();
                fallacy.optionText = optionText;
                fallacies[index] = fallacy;
            }
        }
    }
}
