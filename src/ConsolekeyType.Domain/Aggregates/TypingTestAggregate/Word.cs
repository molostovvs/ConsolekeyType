using System.Text.RegularExpressions;

namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

public class Word : ValueObject
{
    private const int _maxAllowedLength = 40;

    public string Value { get; }

    private Word(string value)
        => Value = value;

    public static Result<Word> Create(Maybe<string> word)
        => word.ToResult("Word should not be empty")
               .Ensure(w => w.Length > 0, "Word should not be empty")
               .Ensure(
                    w => w.Length < _maxAllowedLength,
                    w => $"The word length is {w.Length}, which exceeds the allowed value of {_maxAllowedLength}"
                )
               .Ensure(
                    w => Regex.IsMatch(w, @"^[\p{L}\p{M}]+$"),
                    "There are other symbols in the word besides letters"
                )
               .Map(w => new Word(w));

    public static implicit operator string(Word word)
        => word.Value;

    public static explicit operator Word(string str)
    {
        var result = Create(str);

        if (result.IsFailure)
            throw new DomainException($"Can not convert provided string: {str} to Word");

        return result.Value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}