using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

namespace ConsolekeyType.Application;

public class TypingTestService : ITypingTestService
{
    private readonly ITypingTestRepository _repository;

    public TypingTestService(ITypingTestRepository repository)
        => _repository = repository;

    public void StartTest()
        => throw new NotImplementedException();

    public void EndTest()
        => throw new NotImplementedException();

    public void EnterChar()
        => throw new NotImplementedException();

    public void DeleteChar()
        => throw new NotImplementedException();
}