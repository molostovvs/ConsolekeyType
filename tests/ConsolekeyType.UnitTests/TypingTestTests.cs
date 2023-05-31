namespace ConsolekeyType.UnitTests;

[TestFixture]
public class TypingTestTests
{
    private const string _defaultStr = "ponchi the dog";
    private readonly DateTime _startTime = new DateTime(2020, 1, 1, 10, 0, 0, 50);
    private readonly DateTime _endTime = new DateTime(2020, 1, 1, 10, 0, 2, 440);
    private readonly TimeSpan _duration = TimeSpan.FromMilliseconds(2390);

    [Test]
    public void Create()
    {
        var text = CreateDefaultText();

        var res = TypingTest.Create(text);

        res.Should().Succeed();
    }

    [Test]
    public void Create_with_null()
    {
        var res = TypingTest.Create(null);

        res.Should().Fail();
    }

    [Test]
    public void Total_characters()
    {
        var typingTest = CreateDefaultTypingTest();

        typingTest.TotalCharacters.Should().Be(12);
    }

    [Test]
    public void Start()
    {
        var typingTest = CreateDefaultTypingTest();

        var res = typingTest.Start(_startTime);

        res.Should().Succeed();
        typingTest.StartTime.Should().Be(_startTime);
    }

    [Test]
    public void End()
    {
        var typingTest = CreateDefaultTypingTest();

        var start = typingTest.Start(_startTime);
        var end = typingTest.End(_endTime);

        start.Should().Succeed();
        end.Should().Succeed();
    }

    [Test]
    public void Start_when_running()
    {
        var text = CreateDefaultText();
        var typingTest = TypingTest.Create(text).Value;

        var start = typingTest.Start(_startTime);

        var anotherStart = typingTest.Start(_startTime);

        start.Should().Succeed();
        anotherStart.Should().Fail();
    }

    [Test]
    public void Start_when_completed()
    {
        var text = CreateDefaultText();
        var typingTest = TypingTest.Create(text).Value;

        var start = typingTest.Start(_startTime);
        var end = typingTest.End(_endTime);
        var anotherStart = typingTest.Start(_startTime);

        start.Should().Succeed();
        end.Should().Succeed();
        anotherStart.Should().Fail();
    }

    [Test]
    public void End_when_not_started()
    {
        var typingTest = CreateDefaultTypingTest();

        var start = typingTest.Start(_startTime);
        var end = typingTest.End(_endTime);

        start.Should().Succeed();
        end.Should().Succeed();
    }

    [Test]
    public void End_when_completed()
    {
        var typingTest = CreateDefaultTypingTest();

        var start = typingTest.Start(_startTime);
        var end = typingTest.End(_endTime);

        var anotherEnd = typingTest.End(_endTime);
    }

    [Test]
    public void Duration()
    {
        var typingTest = CreateDefaultTypingTest();

        typingTest.Start(_startTime);
        typingTest.End(_endTime);

        typingTest.Duration.Value.Should().Be(_duration);
    }

    [Test]
    public void Duration_when_not_started()
    {
        var typingTest = CreateDefaultTypingTest();

        typingTest.Duration.HasNoValue.Should().BeTrue();
    }

    [Test]
    public void Duration_when_not_completed()
    {
        var typingTest = CreateDefaultTypingTest();

        typingTest.Start(_startTime);

        typingTest.Duration.HasNoValue.Should().BeTrue();
    }

    [Test]
    public void Entering_char()
    {
        var typingTest = CreateDefaultTypingTest();
        typingTest.Start(_startTime);

        var res = typingTest.EnterChar('a');

        res.Should().Succeed();
    }

    [Test]
    public void Entering_char_when_not_started()
    {
        var typingTest = CreateDefaultTypingTest();

        var res = typingTest.EnterChar('a');

        res.Should().Fail();
    }

    [Test]
    public void Entering_char_when_completed()
    {
        var typingTest = CreateDefaultTypingTest();
        typingTest.Start(_startTime);
        typingTest.End(_endTime);

        var res = typingTest.EnterChar('a');

        res.Should().Fail();
    }

    [Test]
    public void Deleting_last_char()
    {
        var typingTest = CreateDefaultTypingTest();
        typingTest.Start(_startTime);

        typingTest.EnterChar('a');
        var res = typingTest.DeleteLastChar();

        res.Should().Succeed();
        res.Value.Should().Be('a');
    }

    [Test]
    public void Deleting_last_char_when_not_started()
    {
        var typingTest = CreateDefaultTypingTest();

        var res = typingTest.DeleteLastChar();

        res.Should().Fail();
    }

    [Test]
    public void Deleting_last_char_when_no_chars_entered()
    {
        var typingTest = CreateDefaultTypingTest();
        typingTest.Start(_startTime);

        var res = typingTest.DeleteLastChar();

        res.Should().Fail();
    }

    [Test]
    public void Deleting_last_char_when_completed()
    {
        var typingTest = CreateDefaultTypingTest();
        typingTest.Start(_startTime);
        typingTest.EnterChar('a');
        typingTest.End(_endTime);

        var res = typingTest.DeleteLastChar();

        res.Should().Fail();
    }

    private static TypingTest CreateDefaultTypingTest()
        => TypingTest.Create(CreateDefaultText()).Value;

    private static Text CreateText(string str)
        => Text.Create(str, Language.English).Value;

    private static Text CreateDefaultText()
        => CreateText(_defaultStr);
}