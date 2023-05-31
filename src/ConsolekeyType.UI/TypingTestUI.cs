using ConsolekeyType.Application;

namespace ConsolekeyType.UI;

public class TypingTestUI
{
    private ITypingTestService _typingTestService;

    public TypingTestUI(ITypingTestService typingTestService)
        => _typingTestService = typingTestService;
}