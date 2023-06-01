namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

public interface IWordRepository
{
    public Result<IReadOnlyList<Word>> Get(int count, Language language);
}