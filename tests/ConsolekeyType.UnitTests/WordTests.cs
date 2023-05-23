namespace ConsolekeyType.UnitTests;

[TestFixture]
public class WordTests
{
    [Test]
    public void Create_with_empty_input()
    {
        var str = string.Empty;

        var word = Word.Create(str);
        var expected = Result.Failure<Word>("Word should not be empty");

        AssertionExtensions.Should(word).Be(expected);
    }

    [Test]
    public void Create_with_null_input()
    {
        var word = Word.Create(null);
        var expected = Result.Failure<Word>("Word should not be empty");

        AssertionExtensions.Should(word).Be(expected);
    }

    [Test]
    public void Create_with_correct_length()
    {
        const string str = "hello";

        var word = Word.Create(str);

        word.Should().Succeed();
        word.Value.Value.Should().Be("hello");
    }

    [Test]
    public void Create_with_incorrect_length()
    {
        const string str = "hellohellohellohellohellohellohellohellohello";

        var word = Word.Create(str);

        word.Should().Fail();
        word.Error.Should()
            .Be($"The word length is {str.Length}, which exceeds the allowed value of 40");
    }

    [Test]
    public void Create_with_non_letters()
    {
        const string str = "hell0";

        var word = Word.Create(str);

        word.Should().Fail();
        word.Error.Should().Be("There are other symbols in the word besides letters");
    }

    [Test]
    public void Convert_word_to_string()
    {
        const string expected = "hello";
        var word = Word.Create(expected);

        string actual = word.Value;

        actual.Should().Be(expected);
    }

    [Test]
    public void Convert_string_to_word()
    {
        const string expected = "hello";

        Word word = (Word)expected;

        word.Value.Should().Be(expected);
    }

    [Test]
    public void Convert_invalid_string_to_word()
    {
        const string expected = "hell0";

        var act = () => (Word)expected;

        act.Should()
           .ThrowExactly<DomainException>($"Can not convert provided string: {expected} to Word");
    }

    [Test]
    public void Compare_identical()
    {
        const string str = "hello";
        var word1 = Word.Create(str);
        var word2 = Word.Create(str);

        var compare = Equals(word1, word2);

        compare.Should().BeTrue();
    }

    [Test]
    public void Compare_different()
    {
        const string str1 = "hello";
        const string str2 = "world";
        var word1 = Word.Create(str1);
        var word2 = Word.Create(str2);

        var compare = Equals(word1, word2);

        compare.Should().BeFalse();
    }
}