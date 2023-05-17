namespace ConsolekeyType.Domain;

public class Word : ValueObject
{
    public string Value { get; }

    private Word(string value)
        => Value = value;

    public static Result<Word> Create(Maybe<string> word)
    {
        //TODO: CHECK WORD

        return Result.Success(new Word(word.Value));
    }

    public static implicit operator string(Word word)
        => word.Value;

    public static explicit operator Word(string word)
        => Create(word).Value;

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}