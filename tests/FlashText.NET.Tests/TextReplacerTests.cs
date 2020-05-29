using FlashText.NET.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FlashText.NET.Tests
{
    public class TextReplacerTests
    {
        private readonly ITextReplacer textReplacer;

        public static IEnumerable<object[]> TestCases;

        public TextReplacerTests()
        {
            this.textReplacer = new TextReplacer();
        }

        static TextReplacerTests()
        {
            TestCases = JsonConvert
                .DeserializeObject<IEnumerable<TestCase>>(File.ReadAllText("test_data.json"))
                .Select(testCase => new object[] { testCase.Text, testCase.Words.Select(pair => (pair[0], pair[1])).ToArray(), testCase.Expected });
        }

        public class ReplaceWords : TextReplacerTests
        {
            [Theory]
            [MemberData(nameof(TestCases))]
            public void Test(string text, (string, string)[] pairs, string expected)
            {
                var actual = this.textReplacer.ReplaceWords(text, pairs);

                Assert.Equal(expected, actual);
            }
        }
    }
}
