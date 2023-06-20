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
        ShowMainWindow();
        ConsoleHelper.SetCursorMin();
        ConsoleHelper.Clear();
    }

    private const string Title = "ConsolekeyType";
    private Language _language = Language.English;
    private int _wordCount = 5;
    private WindowSection _currentSection = WindowSection.Text;
    private bool _isTextShouldBeUpdated = true;
    private Text _text = Text.Create("default", Language.English).Value;

    private void ShowMainWindow()
    {
        Action nextWindow;

        while (true)
        {
            Console.CursorVisible = _currentSection == WindowSection.Text;

            UpdateMainWindow(_currentSection);

            var input = Console.ReadKey(true);

            if (_currentSection is WindowSection.Text && input.IsLetter())
            {
                nextWindow = () => ShowTestWindow(_text, input);
                break;
            }

            if (input.Key is ConsoleKey.DownArrow)
            {
                if (_currentSection is WindowSection.Text)
                {
                    _currentSection = WindowSection.Language;
                    _isTextShouldBeUpdated = false;
                }
                else if (_currentSection is WindowSection.Language)
                {
                    var langs = Language.All.ToList();
                    var curIndex = langs.IndexOf(_language);
                    var nextIndex = curIndex + 1 == langs.Count ? 0 : curIndex + 1;

                    _language = langs[nextIndex];
                    _isTextShouldBeUpdated = true;
                }
                else if (_currentSection is WindowSection.WordsCount)
                {
                    _wordCount = _wordCount == 100 ? 1 : _wordCount + 1;
                    _isTextShouldBeUpdated = true;
                }
            }
            else if (input.Key is ConsoleKey.UpArrow)
            {
                if (_currentSection is WindowSection.Text)
                {
                    _currentSection = WindowSection.WordsCount;
                    _isTextShouldBeUpdated = false;
                }
                else if (_currentSection is WindowSection.Language)
                {
                    var langs = Language.All.ToList();
                    var curIndex = langs.IndexOf(_language);
                    var nextIndex = curIndex - 1 == -1 ? langs.Count - 1 : curIndex - 1;

                    _language = langs[nextIndex];
                    _isTextShouldBeUpdated = true;
                }
                else if (_currentSection is WindowSection.WordsCount)
                {
                    _wordCount = _wordCount == 1 ? 100 : _wordCount - 1;
                    _isTextShouldBeUpdated = true;
                }
            }
            else if (input.Key is ConsoleKey.RightArrow)
            {
                if (_currentSection is WindowSection.Text)
                    _currentSection = WindowSection.WordsCount;
                else if (_currentSection is WindowSection.Language)
                    _currentSection = WindowSection.WordsCount;
                else if (_currentSection is WindowSection.WordsCount)
                    _currentSection = WindowSection.Text;

                _isTextShouldBeUpdated = false;
            }
            else if (input.Key is ConsoleKey.LeftArrow)
            {
                if (_currentSection is WindowSection.Text)
                    _currentSection = WindowSection.Language;
                else if (_currentSection is WindowSection.Language)
                    _currentSection = WindowSection.Text;
                else if (_currentSection is WindowSection.WordsCount)
                    _currentSection = WindowSection.Language;

                _isTextShouldBeUpdated = false;
            }
            else if (input.Key is ConsoleKey.Enter or ConsoleKey.Escape)
            {
                _currentSection = WindowSection.Text;
                _isTextShouldBeUpdated = false;
            }

            ConsoleHelper.SetCursorMin();
        }

        nextWindow.Invoke();
    }

    private void ShowTestWindow(Text text, ConsoleKeyInfo input)
    {
        var start = Console.GetCursorPosition();

        _typingTestService.StartTest(text);

        var enterCharResult = _typingTestService.EnterCharacter(input.KeyChar).Value;

        ConsoleHelper.WriteWithColor(
            _typingTestService.TypingTest.CurrentChar.ToString(),
            enterCharResult == EnteredCharStatus.Correct
                ? new Color(64, 255, 64)
                : new Color(255, 64, 128)
        );

        while (true)
        {
            input = Console.ReadKey(true);

            if (input.IsLetter() || input.Key is ConsoleKey.Spacebar)
            {
                enterCharResult = _typingTestService.EnterCharacter(input.KeyChar).Value;

                ConsoleHelper.WriteWithColor(
                    _typingTestService.TypingTest.CurrentChar.ToString(),
                    enterCharResult == EnteredCharStatus.Correct
                        ? new Color(64, 255, 64)
                        : new Color(255, 64, 128)
                );
            }

            if (input.Key is ConsoleKey.Backspace)
                if (Console.GetCursorPosition().Left > start.Left)
                {
                    _typingTestService.DeleteCharacter();
                    var cur = Console.GetCursorPosition();

                    Console.SetCursorPosition(cur.Left - 1, cur.Top);
                    Console.Write(_typingTestService.TypingTest.NextChar);
                    Console.SetCursorPosition(cur.Left - 1, cur.Top);
                }

            if (input.Key is ConsoleKey.Enter)
            {
                _typingTestService.EndTest();
                break;
            }
        }
    }

    private void UpdateMainWindow(WindowSection currentSection)
    {
        ConsoleHelper.Clear();
        ConsoleHelper.SetCursorCenterWithOffset(-Title.Length / 2, -5);

        ConsoleHelper.WriteWithColor(Title, new Color(64, 128, 255));
        var textStart = UpdateTextSection();
        UpdateLanguageSection(currentSection);
        UpdateWordCountSection(currentSection);

        if (currentSection == WindowSection.Text)
        {
            Console.CursorVisible = true;
            ConsoleHelper.SetCursorCenterWithOffset(textStart, 0);
        }
    }

    private int UpdateTextSection()
    {
        var textStart = -_text.ToString().Length / 2;
        if (_language == Language.Chinese || _language == Language.Japanese)
            textStart *= 2;

        if (_isTextShouldBeUpdated)
        {
            _text = _textService.GenerateText(_wordCount, _language);
            textStart = -_text.ToString().Length / 2;
            if (_language == Language.Chinese || _language == Language.Japanese)
                textStart *= 2;

            ConsoleHelper.SetCursorCenterWithOffset(-Console.WindowWidth / 2, 0);
            Console.Write(string.Concat(Enumerable.Repeat(" ", Console.WindowWidth)));

            ConsoleHelper.SetCursorCenterWithOffset(textStart, 0);
            Console.Write(_text);
        }

        return textStart;
    }

    private void UpdateLanguageSection(WindowSection currentSection)
    {
        if (currentSection is WindowSection.Language)
        {
            ConsoleHelper.SetCursorCenterWithOffset(-20, 5);
            ConsoleHelper.PrintScrollableListWithSelection(
                Language.All,
                _language,
                3,
                new Color(255, 64, 128)
            );
        }
        else
        {
            ConsoleHelper.SetCursorCenterWithOffset(-20, 5);
            ConsoleHelper.PrintScrollableList(Language.All, _language, 3);
        }
    }

    private void UpdateWordCountSection(WindowSection currentSection)
    {
        var nums = Enumerable.Range(1, 100);

        if (currentSection is WindowSection.WordsCount)
        {
            ConsoleHelper.SetCursorCenterWithOffset(20, 5);
            ConsoleHelper.PrintScrollableListWithSelection(
                nums,
                _wordCount,
                3,
                new Color(255, 64, 128)
            );
        }
        else
        {
            ConsoleHelper.SetCursorCenterWithOffset(20, 5);
            ConsoleHelper.PrintScrollableList(nums, _wordCount, 3);
        }
    }

    private enum WindowSection
    {
        Text, Language, WordsCount,
    }

    public void ShowApology()
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("We have encountered an error! Sorry!");
        Console.ResetColor();
        Console.CursorVisible = true;
    }

    public void FailFast()
    {
        ConsoleHelper.SetCursorMin();
        ConsoleHelper.Clear();
    }
}