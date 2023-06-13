namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

public class Language : EnumValueObject<Language, int>
{
    private Language(int id, string name) : base(id, name) {}

    public static readonly Language English = new(1, nameof(English));
    public static readonly Language Russian = new(2, nameof(Russian));
    public static readonly Language Japanese = new(3, nameof(Japanese));
    public static readonly Language Kazakh = new(4, nameof(Kazakh));
    public static readonly Language Chinese = new(5, nameof(Chinese));
}