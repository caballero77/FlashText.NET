using FlashText.NET.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashText.NET
{
    public class TextReplacer : ITextReplacer
    {
        public string ReplaceWords(string text, params (string From, string To)[] replacementPairs)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var trie = PrepareTrie(replacementPairs);

            var len = text.Length;
            var result = new StringBuilder(len);
            var originalText = text;

            var currentWord = new StringBuilder();
            var current = trie;
            var sequenceEndPos = 0;
            var idx = 0;

            while (idx < len)
            {
                var c = text[idx];
                currentWord.Append(originalText[idx]);

                char? currentWhiteSpace;
                if (IsWordSeparator(c))
                {
                    currentWhiteSpace = c;

                    var currentCodeContainsChar = current.Nodes.TryGetValue(c, out var continuedNode);

                    if (current.Result != null || currentCodeContainsChar)
                    {
                        var longestSequenceFound = "";
                        var IsLongestSequenceFound = false;

                        if (current.Result != null)
                        {
                            longestSequenceFound = current.Result;
                            sequenceEndPos = idx;
                        }

                        if (currentCodeContainsChar)
                        {
                            var currentDictContinued = continuedNode;
                            var currentWordContinued = new StringBuilder();
                            currentWordContinued.Append(currentWord);
                            var idy = idx + 1;

                            while (idy < len)
                            {
                                var innerChar = text[idy];
                                currentWordContinued.Append(originalText[idy]);

                                if (IsWordSeparator(innerChar) && currentDictContinued.Result != null)
                                {
                                    currentWhiteSpace = innerChar;
                                    longestSequenceFound = currentDictContinued.Result;
                                    sequenceEndPos = idy;
                                    IsLongestSequenceFound = true;
                                }
                                if (currentDictContinued.Nodes.TryGetValue(innerChar, out var nextNode))
                                {
                                    currentDictContinued = nextNode;
                                }
                                else
                                {
                                    break;
                                }
                                idy++;
                            }
                            if (idy >= len)
                            {
                                if (currentDictContinued.Result != null)
                                {
                                    currentWhiteSpace = null;
                                    longestSequenceFound = currentDictContinued.Result;
                                    sequenceEndPos = idy;
                                    IsLongestSequenceFound = true;
                                }
                            }
                            if (IsLongestSequenceFound)
                            {
                                idx = sequenceEndPos;
                                currentWord = currentWordContinued;
                            }
                        }
                        current = trie;
                        if (!string.IsNullOrEmpty(longestSequenceFound))
                        {
                            result.Append(longestSequenceFound + currentWhiteSpace);
                        }
                        else
                        {
                            result.Append(currentWord);

                        }
                        currentWord = new StringBuilder();
                    }
                    else
                    {
                        current = trie;
                        result.Append(currentWord);
                        currentWord = new StringBuilder();
                    }
                }
                else if (current.Nodes.TryGetValue(c, out var nextNode))
                {
                    current = nextNode;
                }
                else
                {
                    current = trie;

                    var idy = idx + 1;
                    while (idy < len)
                    {
                        c = text[idy];
                        currentWord.Append(originalText[idy]);

                        if (IsWordSeparator(c))
                        {
                            break;
                        }
                        idy++;
                    }
                    idx = idy;
                    result.Append(currentWord);
                    currentWord = new StringBuilder();
                }
                if (idx + 1 >= len)
                {
                    if (current.Result != null)
                    {
                        result.Append(current.Result);
                    }
                    else
                    {
                        result.Append(currentWord);
                    }
                }
                idx++;
            }

            return result.ToString();
        }

        private static TrieNode PrepareTrie((string From, string To)[] replacementPairs)
        {
            var trie = new TrieNode
            {
                Letter = ' ',
                Nodes = new Dictionary<char, TrieNode>(),
                Result = null
            };

            foreach (var pair in replacementPairs)
            {
                AddPair(ref trie, pair);
            }

            return trie;
        }

        private static void AddPair(ref TrieNode trie, (string From, string To) pair)
        {
            var current = trie;

            for (var i = 0; i < pair.From.Length; i++)
            {
                if (current.Nodes.TryGetValue(pair.From[i], out var node))
                {
                    current = node;
                    continue;
                }
                var newNode = new TrieNode
                {
                    Letter = pair.From[i],
                    Nodes = new Dictionary<char, TrieNode>()
                };

                if (i + 1 == pair.From.Length)
                {
                    newNode.Result = pair.To;
                }

                current.Nodes.Add(pair.From[i], newNode);

                current = current.Nodes[pair.From[i]];
            }
        }

        private static char[] NotWordSeparators = new[] { '`', '-', '_' };

        private static bool IsWordSeparator(char c)
        {
            return !char.IsLetterOrDigit(c) && !NotWordSeparators.Contains(c);
        }
    }
}
