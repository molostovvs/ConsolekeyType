namespace ConsolekeyType.UnitTests;

[TestFixture]
public class TextTests
{
    [Test]
    public void Creating_text_with_zero_words_throws_exception()
    {
        Action createMethod = () => Text.Create(Enumerable.Empty<Word>());

        createMethod.Should()
                    .ThrowExactly<DomainException>()
                    .WithMessage("The number of words must be greater than 0");
    }

    [Test]
    public void Creating_text_with_null_throws_exception()
    {
        Action createMethod = () => Text.Create(null);

        createMethod.Should()
                    .ThrowExactly<NullReferenceException>()
                    .WithMessage("You have to provide the words to create the text");
    }

    [Test]
    public void Creating_text_with_words()
    {
        var words = CreateDefaultWords();

        var createMethod = () => Text.Create(words);

        createMethod.Should().NotThrow();
    }

    [Test]
    public void Identical_texts_is_equivalent()
    {
        var words = CreateDefaultWords();

        var text1 = Text.Create(words).Value;
        var text2 = Text.Create(words).Value;

        text1.Equals(text2);

        text1.Should().BeEquivalentTo(text2);
    }

    [Test]
    public void Different_texts_is_not_equivalent()
    {
        var words1 = CreateWords("a", "b", "c");
        var words2 = CreateWords("a", "b", "x");

        var text1 = Text.Create(words1).Value;
        var text2 = Text.Create(words2).Value;

        text1.Should().NotBe(text2);
    }

    [Test]
    public void Converting_text_to_string()
    {
        var words = CreateDefaultWords();

        var text = Text.Create(words).Value;

        string stringText = text;

        stringText.Should().Be(DefaultWords);
    }

    [Test]
    public void Converting_from_string_to_text()
    {
        const string str = "a b c";
        var expected = Text.Create(CreateWords("a", "b", "c")).Value;

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

    private IReadOnlyList<Word> CreateWords(params string[] words)
        => words.Select(word => Word.Create(word).Value).ToList();

    private const string DefaultWords = "a b c";

    private IReadOnlyList<Word> CreateDefaultWords()
        => CreateWords("a", "b", "c");
}