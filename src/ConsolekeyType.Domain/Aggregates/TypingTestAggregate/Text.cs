using System.Collections.Immutable;

namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

public class Text : ValueObject
{
    public ImmutableList<Word> Words { get; }

    public int WordsCount => Words.Count;

    public Language Language { get; }

    private Text(List<Word> words, Language language)
    {
        Words = words.ToImmutableList();
        Language = language;
    }

    public static Result<Text> Create(IEnumerable<Word> words, Language language)
    {
        if (words is null)
            throw new ArgumentNullException(nameof(words));

        if (language is null)
            throw new ArgumentNullException(nameof(language));

        var list = words.ToList();

        if (list.Count == 0)
            return Result.Failure<Text>("The number of words must be greater than 0");

        return Result.Success(new Text(list, language));
    }

    public static Result<Text> Create(string text, Language language)
        => throw new NotImplementedException();

    public static implicit operator string(Text text)
        => string.Join(" ", text.Words.Select(w => w.Value));

    public override string ToString()
        => string.Join(" ", Words.Select(w => w.Value));

    /*public static explicit operator Text(string text)
    {
        var words = text.Split(
                             " ",
                             StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                         )
                        .Select(w => Word.Create(w).Value);

        return Create(words).Value;
    }*/

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Language;

        foreach (var word in Words)
            yield return word;
    }
}