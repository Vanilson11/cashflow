using System.Collections;

namespace WebApi.Test.InlineData;
public class CultureInfoInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "fr" };
        yield return new object[] { "pt-PT" };
        yield return new object[] { "pt-BR" };
        yield return new object[] { "en" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
