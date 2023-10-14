using System.Text;
using ConsolekeyType.Application;
using ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

namespace ConsolekeyType.UI;

public class TypingTestUI
{
    private readonly ITypingTestService _typingTestService;
    private readonly ITextService _textService;

    private const string Title = "ConsolekeyType";
    private Language _language = Language.English;
    private int _wordCount = 5;
    private WindowSection _currentSection = WindowSection.Text;
    private bool _isTextShouldBeGenerated;
    private Text _text;

    public TypingTestUI(ITypingTestService typingTestService, ITextService textService)
    {
        _typingTestService = typingTestService;
        _textService = textService;
        _text = _textService.GenerateText(_wordCount, _language);
    }

    public void Run()
    {
        ConsoleHelper.PrepareScreen();
        ShowMainWindow();
        ConsoleHelper.Clear();
    }

    private void ShowMainWindow()
    {
        ConsoleKeyInfo input;

        while (true)
        {
            Console.CursorVisible = _currentSection == WindowSection.Text;

            UpdateMainWindow(_currentSection);

            input = Console.ReadKey(true);

            if (input.Key is ConsoleKey.DownArrow)
            {
                if (_currentSection is WindowSection.Text)
                {
                    _currentSection = WindowSection.Language;
                    _isTextShouldBeGenerated = false;
                }
                else if (_currentSection is WindowSection.Language)
                {
                    var langs = Language.All.ToList();
                    var curIndex = langs.IndexOf(_language);
                    var nextIndex = curIndex + 1 == langs.Count ? 0 : curIndex + 1;

                    _language = langs[nextIndex];
                    _isTextShouldBeGenerated = true;
                }
                else if (_currentSection is WindowSection.WordsCount)
                {
                    _wordCount = _wordCount == 100 ? 1 : _wordCount + 1;
                    _isTextShouldBeGenerated = true;
                }
            }
            else if (input.Key is ConsoleKey.UpArrow)
            {
                if (_currentSection is WindowSection.Text)
                {
                    _currentSection = WindowSection.WordsCount;
                    _isTextShouldBeGenerated = false;
                }
                else if (_currentSection is WindowSection.Language)
                {
                    var langs = Language.All.ToList();
                    var curIndex = langs.IndexOf(_language);
                    var nextIndex = curIndex - 1 == -1 ? langs.Count - 1 : curIndex - 1;

                    _language = langs[nextIndex];
                    _isTextShouldBeGenerated = true;
                }
                else if (_currentSection is WindowSection.WordsCount)
                {
                    _wordCount = _wordCount == 1 ? 100 : _wordCount - 1;
                    _isTextShouldBeGenerated = true;
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

                _isTextShouldBeGenerated = false;
            }
            else if (input.Key is ConsoleKey.LeftArrow)
            {
                if (_currentSection is WindowSection.Text)
                    _currentSection = WindowSection.Language;
                else if (_currentSection is WindowSection.Language)
                    _currentSection = WindowSection.Text;
                else if (_currentSection is WindowSection.WordsCount)
                    _currentSection = WindowSection.Language;

                _isTextShouldBeGenerated = false;
            }
            else if (input.Key is ConsoleKey.Enter or ConsoleKey.Escape)
            {
                _currentSection = WindowSection.Text;
                _isTextShouldBeGenerated = false;
            }

            if (_currentSection is WindowSection.Text && input.IsLetter())
                break;
        }

        ShowTestWindow(_text, input);
    }

    private void ShowTestWindow(Text text, ConsoleKeyInfo input)
    {
        var start = Console.GetCursorPosition();
        ConsoleHelper.Clear();

        UpdateTextSection();
        PrintCurrentLanguageAndWordCount();

        _typingTestService.StartTest(text);

        var enterCharResult = _typingTestService.EnterCharacter(input.KeyChar).Value;

        ConsoleHelper.WriteWithColor(
            _typingTestService.TypingTest.CurrentChar.ToString(),
            enterCharResult == EnteredCharStatus.Correct ? Color.Green : Color.Red
        );

        while (true)
        {
            input = Console.ReadKey(true);

            if (input.IsLetter() || input.Key is ConsoleKey.Spacebar)
            {
                enterCharResult = _typingTestService.EnterCharacter(input.KeyChar).Value;

                ConsoleHelper.WriteWithColor(
                    _typingTestService.TypingTest.CurrentChar.ToString(),
                    enterCharResult == EnteredCharStatus.Correct ? Color.Green : Color.Red
                );
            }
            else if (input.Key is ConsoleKey.Backspace)
            {
                if (Console.GetCursorPosition().Left > start.Left)
                {
                    _typingTestService.DeleteCharacter();
                    var cur = Console.GetCursorPosition();

                    Console.SetCursorPosition(cur.Left - 1, cur.Top);
                    Console.Write(_typingTestService.TypingTest.NextChar);
                    Console.SetCursorPosition(cur.Left - 1, cur.Top);
                }
            }
            else if (input.Key is ConsoleKey.Enter)
            {
                _typingTestService.EndTest();
                break;
            }
        }

        ShowProfileWindow();
    }

    private void ShowProfileWindow()
    {
        Console.CursorVisible = false;
        ConsoleHelper.Clear();

        ConsoleHelper.SetCursorCenterWithOffset(
            -"YOUR PROFILE".Length / 2,
            -Console.WindowHeight / 2 + 2
        );

        Console.WriteLine("YOUR PROFILE");

        ConsoleHelper.SetCursorCenter();
        ConsoleHelper.WriteCentered(
            "[DATE] [CPM] [WPM] [CHARS] [LANGUAGE] [WORDS NUM] [DICTIONARY]"
        );

        var tests = _typingTestService.GetLastXTypingTests(5);

        var start = ConsoleHelper.GetCenter();
        start.y++;

        foreach (var test in tests)
        {
            Console.SetCursorPosition(start.x, start.y);

            var testStr = string.Join(
                ' ',
                test.StartTime.ToShortDateString(),
                test.CPM,
                test.WPM,
                test.TotalCharacters,l
                test.Text.Language,
                test.Text.WordsCount,
                "DICTIONARY"
            );

            ConsoleHelper.WriteCentered(testStr);
            start.y += 1;
        }

        Console.ReadKey();
    }

    private void PrintCurrentLanguageAndWordCount()
    {
        ConsoleHelper.SaveAndRestoreCursorPosition(
            () =>
            {
                ConsoleHelper.SetCursorCenterWithOffset(-20, 5);
                ConsoleHelper.WriteWithColor(
                    string.Concat("[", _language.ToString(), "]"),
                    Color.Red
                );
                ConsoleHelper.SetCursorCenterWithOffset(20, 5);
                ConsoleHelper.WriteWithColor(
                    string.Concat("[", _wordCount.ToString(), "]"),
                    Color.Red
                );
            }
        );
    }

    private void UpdateMainWindow(WindowSection currentSection)
    {
        ConsoleHelper.Clear();
        ConsoleHelper.SetCursorCenterWithOffset(-Title.Length / 2, -5);

        ConsoleHelper.WriteWithColor(Title, Color.Blue);
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
        if (_isTextShouldBeGenerated)
            _text = _textService.GenerateText(_wordCount, _language);

        var textStart = -_text.ToString().Length / 2;
        if (IsLanguageWithWideLetters())
            textStart *= 2;

        ConsoleHelper.SetCursorCenterWithOffset(-Console.WindowWidth / 2, 0);
        Console.Write(new string(' ', Console.WindowWidth));
        ConsoleHelper.SetCursorCenterWithOffset(textStart, 0);
        Console.Write(_text);
        ConsoleHelper.SetCursorCenterWithOffset(textStart, 0);

        return textStart;

        bool IsLanguageWithWideLetters()
            => _language == Language.Chinese || _language == Language.Japanese;
    }

    private void UpdateLanguageSection(WindowSection currentSection)
    {
        if (currentSection is WindowSection.Language)
        {
            ConsoleHelper.SetCursorCenterWithOffset(-20, 5);
            ConsoleHelper.PrintScrollableListWithSelection(Language.All, _language, 3, Color.Red);
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
            ConsoleHelper.PrintScrollableListWithSelection(nums, _wordCount, 3, Color.Red);
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
        Console.ReadKey();
    }

    public void FailFast()
    {
        ConsoleHelper.SetCursorMin();
        ConsoleHelper.Clear();
    }
}