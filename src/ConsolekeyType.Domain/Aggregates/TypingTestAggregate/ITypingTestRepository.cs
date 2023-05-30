namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

public interface ITypingTestRepository
{
    public Result Save(TypingTest typingTest);
    public Result<IReadOnlyCollection<TypingTest>> GetAll();
    public Maybe<TypingTest> GetById(long id);
}