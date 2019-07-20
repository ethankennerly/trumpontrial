using FineGameDesign.UI;
using NUnit.Framework;
using System.Collections.Generic;

namespace FineGameDesign.Tests.UI
{
    public static class TextListerTests
    {
        [Test]
        public static void PopulatePossibleTexts_TwoOptions_InvokesOnOptionsChanged()
        {
            TextLister lister = new TextLister();
            string[] possibleTexts = new string[]{"a", "b"};
            lister.PopulatePossibleTexts(possibleTexts);

            TextListAppender appender = new TextListAppender();
            lister.PopulatePossibleTexts(possibleTexts);
            Assert.AreEqual(0, appender.PopulatedOptionTexts.Count);

            lister.OnOptionsChanged += appender.AddTextsDelegate;
            lister.PopulatePossibleTexts(possibleTexts);
            Assert.AreEqual(0, appender.PopulatedOptionTexts.IndexOf("a"));
            Assert.AreEqual(1, appender.PopulatedOptionTexts.Count);

            lister.OnOptionsChanged -= appender.AddTextsDelegate;
            lister.PopulatePossibleTexts(possibleTexts);
            Assert.AreEqual(1, appender.PopulatedOptionTexts.Count);
        }
    }

    sealed class TextListAppender
    {
        private List<string> m_PopulatedOptionTexts = new List<string>();
        internal List<string> PopulatedOptionTexts
        {
            get { return m_PopulatedOptionTexts; }
        }
        
        internal TextLister.Populate AddTextsDelegate;

        private void AddTexts(List<string> optionTexts)
        {
            m_PopulatedOptionTexts.AddRange(optionTexts);
        }

        internal TextListAppender()
        {
            AddTextsDelegate = AddTexts;
        }
    }
}
