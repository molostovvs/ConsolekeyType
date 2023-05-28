namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

public interface ILanguageRepository
{
    public Maybe<Language> GetById(long id);
    public Maybe<Language> GetByName(string name);
}