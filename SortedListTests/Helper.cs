namespace  SortedListTests;

public static class Helper
{
    public static void AssertEqual<T>(IEnumerable<T> enum1, IEnumerable<T> enum2) where T : IComparable<T>
    {
        var enumOne = enum1.GetEnumerator();
        var enumTwo = enum2.GetEnumerator();

        while (enumTwo.MoveNext() | enumOne.MoveNext())
        {
            Assert.Equal(0, enumOne.Current.CompareTo(enumTwo.Current));
        }

        enumOne.Dispose();
        enumTwo.Dispose();
    }
}

