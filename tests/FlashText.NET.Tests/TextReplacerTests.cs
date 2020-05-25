using FlashText.NET.Interfaces;
using System.Collections.Generic;
using Xunit;

namespace FlashText.NET.Tests
{
    public class TextReplacerTests
    {
        private readonly ITextReplacer textReplacer;

        public TextReplacerTests()
        {
            this.textReplacer = new TextReplacer();
        }

        public class ReplaceWords : TextReplacerTests
        {
            [Theory]
            [MemberData(nameof(Data))]
            public void Test(string text, (string, string)[] pairs, string expected)
            {
                var actual = this.textReplacer.ReplaceWords(text, pairs);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> Data = new[]
            {
                new object[] { "SimpleTest", new[] { ("SimpleTest", "SuccessResult") }, "SuccessResult" },
                new object[] { "Test case for test.", new[] { ("test", "tset") }, "Test case for tset." },
                new object[] { "I`m sure that IOS better than Android.", new[] { ("IOS", "Android"), ("Android", "IOS") }, "I`m sure that Android better than IOS." },
                new object[] { "Shouldn`t replace.", new [] { ("Shouldn`t", "Should") }, "Should replace." },
                new object[] { "Shouldn`t replace.", new [] { ("Should", "Could") }, "Shouldn`t replace." },
                new object[] { "Very simple sentence.", new [] { ("sentence", "test case") }, "Very simple test case." },
                new object[] { "Very simple sentence.", new [] { ("Very", "Second") }, "Second simple sentence." },
                new object[] { "Replace a single char.", new [] { ("a", "the") }, "Replace the single char." },
                new object[] { "I`m sure that IOS better than Android.", new[] { ("IOS better than Android", "both of them are good"), ("Android", "IOS") }, "I`m sure that both of them are good." },
                new object[] { "distributed super distributed super computing institute.", new [] {
                    ("distributed super computing", "Distributed Super Computing"),
                    ("distributed super computing institute", "Distributed Super Computing Institute"),
                }, "distributed super Distributed Super Computing Institute." },
            };
        }
    }
}
