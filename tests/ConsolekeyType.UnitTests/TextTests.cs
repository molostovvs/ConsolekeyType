namespace ConsolekeyType.UnitTests;

[TestFixture]
public class TextTests
{
    [Test]
    public void Words_count()
    {
        var words = CreateDefaultWords();
        var text = Text.Create(words, Language.English).Value;

        text.WordsCount.Should().Be(_defaultWords.Split(" ").Length);
    }

    [Test]
    public void Creating_text_with_zero_words()
    {
        var result = Text.Create(Enumerable.Empty<Word>(), Language.English);

        result.Should().Fail();
    }

    [Test]
    public void Create_text_with_null_enumerable()
    {
        var res = Text.Create((IEnumerable<Word>)null, Language.English);

        res.Should().Fail();
    }

    [Test]
    public void Create_text_with__words()
    {
        var words = new List<Word> { null, null };

        var res = Text.Create(words, Language.English);

        res.Should().Fail();
    }

    [Test]
    public void Create_text_with_null_language()
    {
        var words = CreateDefaultWords();

        var res = Text.Create(words, null);

        res.Should().Fail();
    }

    [Test]
    public void Text_with_words()
    {
        var words = CreateDefaultWords();

        var createMethod = () => Text.Create(words, Language.English);

        createMethod.Should().NotThrow();
    }

    [Test]
    public void Create_from_valid_string()
    {
        var text = "ponchi the dog";

        var res = Text.Create(text, Language.English);

        res.Should().Succeed();
    }

    [Test]
    public void Create_from_empty_string()
    {
        var text = string.Empty;

        var res = Text.Create(text, Language.English);

        res.Should().Fail();
    }

    [Test]
    public void Create_from_null_string()
    {
        var text = (string)null;

        var res = Text.Create(text, Language.English);

        res.Should().Fail();
    }

    [Test]
    public void Create_from_long_word()
    {
        var text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; // text.Length = 50

        var res = Text.Create(text, Language.English);

        res.Should().Fail();
    }

    [Test]
    public void Compare_fully_identical()
    {
        var words = CreateDefaultWords();

        var text1 = Text.Create(words, Language.English).Value;
        var text2 = Text.Create(words, Language.English).Value;

        var equality = text1.Equals(text2);

        equality.Should().BeTrue();
    }

    [Test]
    public void Compare_with_identical_words_and_different_languages()
    {
        var words = CreateDefaultWords();
        var text1 = Text.Create(words, Language.English).Value;
        var text2 = Text.Create(words, Language.Russian).Value;

        var equality = text1.Equals(text2);

        equality.Should().BeFalse();
    }

    [Test]
    public void Compare_with_different_words_and_identical_languages()
    {
        var words1 = CreateWords("a", "b", "c");
        var words2 = CreateWords("a", "b", "x");

        var text1 = Text.Create(words1, Language.English).Value;
        var text2 = Text.Create(words2, Language.English).Value;

        var equality = text1.Equals(text2);

        equality.Should().BeFalse();
    }

    [Test]
    public void Compare_completely_different()
    {
        var words1 = CreateWords("a", "b", "c");
        var words2 = CreateWords("a", "b", "x");

        var text1 = Text.Create(words1, Language.English).Value;
        var text2 = Text.Create(words2, Language.Russian).Value;

        var equality = text1.Equals(text2);

        equality.Should().BeFalse();
    }

    [Test]
    public void Converting_text_to_string()
    {
        var words = CreateDefaultWords();

        var text = Text.Create(words, Language.English).Value;

        string stringText = text;

        stringText.Should().Be(_defaultWords);
    }

    [Test]
    public void ToString_method()
    {
        var result = Text.Create(CreateDefaultWords(), Language.English);

        var str = result.Value.ToString();

        str.Should().BeEquivalentTo(_defaultWords);
    }

    /*//converting to text
    [Test]
    public void Converting_from_string_to_text()
    {
        const string str = "a b c";
        var expected = Text.Create(CreateWords("a", "b", "c"), Language.English).Value;

        var actual = (Text)str;

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Converting_from_empty_string_to_text_throws()
    {
        const string str = "       ";

        var act = () => (Text)str;

        act.Should().ThrowExactly<DomainException>("The number of words must be greater than 0");
    }

    [Test]
    public void Converting_from_null_string_to_text_throws()
    {
        const string str = null;

        var act = () => (Text)str;

        act.Should().ThrowExactly<DomainException>("The number of words must be greater than 0");
    }*/

    private IReadOnlyList<Word> CreateWords(params string[] words)
        => words.Select(word => Word.Create(word).Value).ToList();

    private const string _defaultWords = "a b c";

    private IReadOnlyList<Word> CreateDefaultWords()
        => CreateWords(_defaultWords.Split(" "));
}