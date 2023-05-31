namespace ConsolekeyType.Application;

public interface ITypingTestService
{
    public void StartTest();
    public void EndTest();
    public void EnterChar();
    public void DeleteChar();
}