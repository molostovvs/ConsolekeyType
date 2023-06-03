using ConsolekeyType.Application;
using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

namespace ConsolekeyType.UI;

public class TypingTestUI
{
    private readonly ITypingTestService _typingTestService;
    private readonly ITextService _textService;

    public TypingTestUI(ITypingTestService typingTestService, ITextService textService)
    {
        _typingTestService = typingTestService;
        _textService = textService;
    }

    public void Run()
    {
        Console.CursorVisible = false;

        var languages = Language.All.ToList();
        var selectedLanguageIndex = 0;

        Console.WriteLine("Choose a language for the test:");

        while (true)
        {
            Console.Clear();

            for (int i = 0; i < languages.Count; i++)
            {
                if (i == selectedLanguageIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine($"{i + 1}. {languages[i].Name}");

                Console.ResetColor();
            }

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedLanguageIndex--;

                if (selectedLanguageIndex < 0)
                    selectedLanguageIndex = languages.Count - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedLanguageIndex++;

                if (selectedLanguageIndex >= languages.Count)
                    selectedLanguageIndex = 0;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        var selectedLanguage = languages[selectedLanguageIndex];

        Console.WriteLine("Enter the number of words for the test:");
        var numWords = int.Parse(Console.ReadLine());

        var text = _textService.GenerateText(numWords, selectedLanguage);

        _typingTestService.StartTest(text);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Type the following text:");
            Console.WriteLine(_typingTestService.TypingTest.Text);

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace)
            {
                _typingTestService.DeleteCharacter();
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                _typingTestService.EndTest();
                Console.WriteLine("Test complete!");
                break;
            }
            else
            {
                _typingTestService.EnterCharacter(key.KeyChar);
            }
        }
    }

    public void ShowApology()
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("SORRY!");
        Console.ResetColor();
    }
}