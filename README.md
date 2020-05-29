# FlashText.NET
![Buld, test and publish.](https://github.com/Caballero77/FlashText.NET/workflows/Buld,%20test%20and%20publish./badge.svg)

This simple package can be used to replace words from any texts or sentences.

# Installation
```dotnet add package FlashText.NET```

# API doc
The main service ```TextReplacer``` implements interface ```ITextReplacer``` with only one method
```csharp
string ReplaceWords(string text, params (string From, string To)[] replacementPairs);
```

# Usage
```csharp
using FlashText.NET;

var replacer = new TextReplacer();
var text = "I`m sure that IOS better than Android.";
var pairs = new []
{
  ("IOS better than Android", "both of them are good"),
  ("Android", "IOS")
};

Console.WriteLine(replacer.ReplaceWords(text, pairs));
```