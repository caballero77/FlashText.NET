using System.Collections.Generic;

namespace FlashText.NET
{
    internal struct TrieNode
    {
        internal char Letter { get; set; }

        internal IDictionary<char, TrieNode> Nodes { get; set; }

        internal string Result { get; set; }
    }
}