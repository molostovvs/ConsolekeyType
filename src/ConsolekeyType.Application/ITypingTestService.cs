using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;
using CSharpFunctionalExtensions;

namespace ConsolekeyType.Application;

public interface ITypingTestService
{
    public TypingTest TypingTest { get; }
    public void StartTest(Text text);
    public void AbortTest();
    public void EndTest();
    public Result<EnteredCharStatus> EnterCharacter(char character);
    public void DeleteCharacter();
}