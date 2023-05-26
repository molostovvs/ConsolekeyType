namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

public interface ILanguageRepository
{
    public Result<IEnumerable<Language>> GetAll();
    public Maybe<Language> GetById(long id);
    public Maybe<Language> GeByName(string name);
}