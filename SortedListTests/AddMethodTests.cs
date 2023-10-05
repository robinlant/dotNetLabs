namespace SortedListTests;

public class AddMethodTests
{
    public static IEnumerable<object[]> AddData_NoDuplicates()
    {
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3 },
            10,
            new [] { -600, -100, 1, 3, 10, 87, 200 }
        };
    }

    public static IEnumerable<object[]> AddData_Duplicates()
    {
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3, 10 },
            10
        };
    }


    [Theory]
    [MemberData(nameof(AddData_NoDuplicates))]
    public void Add_NoDuplicates_SortedList<T>(SortedList<T> list, T item, T[] expOutput) where T : IComparable<T>
    {
        list.Add(item);

        Helper.AssertEqual(list,expOutput);
    }

    [Fact]
    public void Add_Duplicates_ArgumentException()
    {
        var list = new SortedList<int> { 1, -100, 200, 87, -600, 3 };

        Assert.Throws<ArgumentException>(() => list.Add(1));
    }

    [Fact]
    public void Add_Null_ArgumentNullException()
    {
        var list = new SortedList<string> {};
        string nullString = null;

        Assert.Throws<ArgumentNullException>(() => list.Add(nullString));
    }
}