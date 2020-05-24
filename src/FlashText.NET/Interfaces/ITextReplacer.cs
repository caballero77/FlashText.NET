namespace FlashText.NET.Interfaces
{
    public interface ITextReplacer
    {
        string ReplaceWords(string text, params (string From, string To)[] replacementPairs);
    }
}
