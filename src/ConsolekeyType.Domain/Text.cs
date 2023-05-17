using System.Collections.Immutable;

namespace ConsolekeyType.Domain;

public class Text : ValueObject
{
    public ImmutableList<Word> Value { get; }

    public int Length => Value.Count;

    // public Language Language { get; }

    private Text(IEnumerable<Word> words)
    {
        Value = ImmutableList<Word>.Empty.AddRange(words);
        // Language = language;
    }

    public static Result<Text> Create(Maybe<IEnumerable<Word>> words, Maybe<Language> language)
    {
        
        
        // if (words is null)
        //     throw new NullReferenceException("You have to provide the words to create the text");
        //
        // var list = words.ToList();
        //
        // if (list.Count == 0)
        //     throw new DomainException("The number of words must be greater than 0");

        // return Result.Success(new Text(list));
    }

    public static implicit operator string(Text text)
        => string.Join(" ", text.Value.Select(w => w.Value));

    public static explicit operator Text(string text)
    {
        var words = text.Split(
                             " ",
                             StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                         )
                        .Select(w => Word.Create(w).Value);

        return Create(words, null).Value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
        => Value;
}