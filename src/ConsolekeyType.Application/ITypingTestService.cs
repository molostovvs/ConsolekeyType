using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

namespace ConsolekeyType.Application;

public interface ITypingTestService
{
    public TypingTest TypingTest { get; }
    public void StartTest(Text text);
    public void AbortTest();
    public void EndTest();
    public void EnterCharacter(char character);
    public void DeleteCharacter();
}