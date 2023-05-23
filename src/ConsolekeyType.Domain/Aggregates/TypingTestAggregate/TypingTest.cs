namespace ConsolekeyType.Domain.Aggregates.TypingTestAggregate;

//TODO: allow interrupt TypingTest
public class TypingTest : Entity, IAggregateRoot
{
    public Text Text { get; }

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Maybe<TimeSpan> Duration => IsCompleted ? EndTime - StartTime : Maybe<TimeSpan>.None;

    public bool IsRunning { get; private set; }
    public bool IsCompleted { get; private set; }

    /*//CPM and WPM
    //TODO: implement cpm and wpm
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
        _enteredChars = new Stack<char>();
        TotalCharacters = Text.Words.Sum(w => w.Value.Length);
    }

    public static Result<TypingTest> Create(Maybe<Text> text)
    {
        if (text.HasNoValue)
            return Result.Failure<TypingTest>("You must provide the text");

        return Result.Success(new TypingTest(text.Value));
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

    public Result EnterChar(char @char)
    {
        if (!IsRunning)
            return Result.Failure("Test is not in running phase");

        _enteredChars.Push(@char);

        return Result.Success(@char);
    }

    public Result<char> DeleteLastChar()
    {
        if (!IsRunning)
            return Result.Failure<char>("Test is not in running phase");

        if (!_enteredChars.TryPop(out var @char))
            return Result.Failure<char>("There is no entered chars yet");

        return Result.Success(@char);
    }
}