namespace SortedListTests;

public class RemoveMethodTests
{
    public static IEnumerable<object[]>RemoveData_InTheList()
    {
        yield return new object[]
        {
            new SortedList<int> { -600, -100, 1, 3, 10, 87, 200 },
            10,
            new [] { -600, -100, 1, 3, 87, 200 },
        };
        yield return new object[]
        {
            new SortedList<int> { -100, 1, 3, 10, -600, 87, 200 },
            -600,
            new [] { -100, 1, 3, 10, 87, 200 },
        };
    }

    public static IEnumerable<object[]>RemoveData_NotInTheList()
    {
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3 },
            10,
        };
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3 },
            1992,
        };
    }

    [Theory]
    [MemberData(nameof(RemoveData_InTheList))]
    public void Remove_ItemInTheList_True<T>(SortedList<T> list, T item, IEnumerable<T> expected) where T : IComparable<T>
    {
        bool isDeletionSuccessful = list.Remove(item);

        Assert.True(isDeletionSuccessful);
        Helper.AssertEqual(list, expected);
    }

    [Theory]
    [MemberData(nameof(RemoveData_NotInTheList))]
    public void Remove_ItemNotInTheList_False<T>(SortedList<T> list, T item) where T : IComparable<T>
    {
        bool isDeletionSuccessful = list.Remove(item);

        Assert.False(isDeletionSuccessful);
        Assert.DoesNotContain(item, list);
    }

    [Fact]
    public void Remove_Null_ArgumentNullException()
    {
        var list = new SortedList<string>();
        string nullString = null;

        Assert.Throws<ArgumentNullException>(() => list.Remove(nullString));
    }
}