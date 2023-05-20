namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

//TODO: allow interrupt TypingTest
public class TypingTest : Entity, IAggregateRoot
{
    public Text Text { get; }

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Maybe<TimeSpan> Duration => IsCompleted ? EndTime - StartTime : Maybe<TimeSpan>.None;

    public bool IsStarted { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsCompleted { get; private set; }

    /*//CPM and WPM
    private float _cpm;
    private float _wpm;

    public Maybe<float> CPM => IsCompleted ? _cpm : Maybe<float>.None;
    public Maybe<float> WPM => IsCompleted ? _wpm : Maybe<float>.None;*/

    private IReadOnlyCollection<char> _text;
    private Stack<char> _enteredChars;

    public int TotalCharacters { get; }

    private TypingTest(Text text)
    {
        Text = text;
        _text = text.ToString().ToCharArray();
        TotalCharacters = Text.Words.Sum(w => w.Value.Length);
    }

    public static Result<TypingTest> Create(Maybe<Text> text)
    {
        if (text.HasNoValue)
            return Result.Failure<TypingTest>("You must provide the text");

        return Result.Success(new TypingTest(text.Value));
    }

    public void Start(DateTime startTime)
    {
        if (IsRunning || IsCompleted)
            throw new InvalidOperationException("The test is already started");

        StartTime = startTime;
        IsStarted = true;
    }

    public void End(DateTime endTime)
    {
        //TODO: Add wpm and cpm calculation

        if (!IsRunning || IsCompleted)
            throw new InvalidOperationException("The test cannot be completed");

        EndTime = endTime;
        IsCompleted = true;
        IsRunning = false;
    }

    public void EnterChar(char @char)
    {
        //TODO: maybe just ignore char instead of throwing ex?
        if (IsCompleted)
            throw new InvalidOperationException("Test is completed");

        _enteredChars.Push(@char);
    }

    public void DeleteLastChar()
    {
        //TODO: maybe just ignore operation instead of throwing ex?
        if (IsCompleted)
            throw new InvalidOperationException("Test is completed");

        _enteredChars.Pop();
    }
}