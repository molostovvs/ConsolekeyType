using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

namespace ConsolekeyType.Application;

public interface ITextService
{
    public Text GenerateText(int wordCount, Language language);
}