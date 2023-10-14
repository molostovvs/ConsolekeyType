using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using CSharpFunctionalExtensions;

namespace ConsolekeyType.Application;

public class TypingTestService : ITypingTestService
{
    private readonly ITypingTestRepository _repository;

    public TypingTest TypingTest { get; private set; }

    public TypingTestService(ITypingTestRepository repository)
        => _repository = repository;

    public void StartTest(Text text)
    {
        var (_, isFailure, typingTest) = TypingTest.Create(text);

        if (isFailure)
            throw new Exception(); //TODO: Specify exception (and check should we even throw here)

        typingTest.Start(DateTime.Now);
        TypingTest = typingTest;
    }

    //do we even need this?
    public void AbortTest()
    {
        var (_, isFailure) = TypingTest.End(DateTime.Now);

        if (isFailure)
            throw new Exception(); //TODO: check previous todo
    }

    public void EndTest()
    {
        TypingTest.End(DateTime.Now);
        var (_, isFailure) = _repository.Save(TypingTest);

        if (isFailure)
            throw new Exception(); //TODO: check previous todo
    }

    public Result<EnteredCharStatus> EnterCharacter(char character)
    {
        var (_, isFailure, value) = TypingTest.EnterChar(character);

        if (isFailure)
            throw new Exception(); //TODO: check previous todo

        return value;
    }

    public void DeleteCharacter()
    {
        var (_, isFailure, _) = TypingTest.DeleteLastChar();

        if (isFailure)
            throw new Exception(); //TODO: check previous todo
    }

    public IEnumerable<TypingTest> GetLastXTypingTests(int count)
    {
        var allTestsOrNothing = _repository.GetAll();

        if (allTestsOrNothing.IsSuccess)
            return allTestsOrNothing.Value.Take(count);

        return Enumerable.Empty<TypingTest>();
    }
}