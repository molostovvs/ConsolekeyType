using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

namespace ConsolekeyType.Application;

public class TextService : ITextService
{
    private readonly IWordRepository _wordRepository;

    public TextService(IWordRepository wordRepository)
        => _wordRepository = wordRepository;

    public Text GenerateText(int wordCount, Language language)
    {
        var words = _wordRepository.Get(wordCount, language);

        if (words.IsFailure)
            throw new Exception(); //TODO: Specify exception (and check should we even throw here)

        var text = Text.Create(words.Value, language);

        if (text.IsFailure)
            throw new Exception(); //TODO: check previous if statement

        return text.Value;
    }
}