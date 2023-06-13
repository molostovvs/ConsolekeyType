namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

//TODO: allow interrupt TypingTest
public partial class TypingTest : Entity, IAggregateRoot
{
    public sealed override long Id { get; protected set; }
    public Text Text { get; }

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Maybe<TimeSpan> Duration => IsCompleted ? EndTime - StartTime : Maybe<TimeSpan>.None;

    public bool IsRunning { get; private set; }
    public bool IsCompleted { get; private set; }

    public char NextChar => _enteredChars.Count > Text.Length ? ' ' : Text[_enteredChars.Count];

    public char CurrentChar
        => _enteredChars.Count > Text.Length ? ' ' : Text[_enteredChars.Count - 1];

    //TODO: implement monkeytype compatible CPM and WPM
    public Maybe<float> CPM => IsCompleted
        ? (float)(_correctCharsCounter / Duration.Value.TotalMilliseconds * 1000 * 60)
        : Maybe<float>.None;

    public Maybe<float> WPM => IsCompleted
        ? (float)(CPM.Value / Text.Words.Average(w => w.Value.Length))
        : Maybe<float>.None;

    private Stack<char> _enteredChars;
    private int _correctCharsCounter;
    private int _incorrectCharsCounter;

    public int TotalCharacters { get; }

    private TypingTest(Text text, long id = default)
    {
        Id = id;
        Text = text;
        _enteredChars = new Stack<char>();
        TotalCharacters = Text.Words.Sum(w => w.Value.Length);
    }

    public static Result<TypingTest> Create(Maybe<Text> text)
    {
        if (text.HasNoValue)
            return Result.Failure<TypingTest>("You must provide the text");

        return Result.Success(new TypingTest(text.Value));
    }

    public static Result<TypingTest> Create(Maybe<Text> text, long id)
    {
        if (text.HasNoValue)
            return Result.Failure<TypingTest>("You must provide the text");

        if (id < 0)
            return Result.Failure<TypingTest>("Id cannot be negative");

        return Result.Success(new TypingTest(text.Value, id));
    }

    public Result Start(DateTime startTime)
    {
        if (IsRunning || IsCompleted)
            return Result.Failure("The test is already started");

        StartTime = startTime;
        IsRunning = true;

        return Result.Success(startTime);
    }

    public Result End(DateTime endTime)
    {
        //TODO: Add wpm and cpm calculation

        if (!IsRunning || IsCompleted)
            return Result.Failure("The test cannot be completed");

        EndTime = endTime;
        IsCompleted = true;
        IsRunning = false;

        return Result.Success(endTime);
    }

    public Result<EnteredCharStatus> EnterChar(char @char)
    {
        if (!IsRunning)
            return Result.Failure<EnteredCharStatus>("Test is not in running phase");

        _enteredChars.Push(@char);

        if (Text[_enteredChars.Count - 1] == @char)
        {
            _correctCharsCounter++;
            return Result.Success(EnteredCharStatus.Correct);
        }

        _incorrectCharsCounter++;
        return Result.Success(EnteredCharStatus.Incorrect);
    }

    public Result<char> DeleteLastChar()
    {
        if (!IsRunning)
            return Result.Failure<char>("Test is not in running phase");

        if (!_enteredChars.TryPop(out var @char))
            return Result.Failure<char>("There is no entered chars yet");

        if (@char == Text[_enteredChars.Count])
            _correctCharsCounter--;
        else
            _incorrectCharsCounter--;

        return Result.Success(@char);
    }
}