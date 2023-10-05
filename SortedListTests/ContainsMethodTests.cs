namespace SortedListTests;

public class ContainsMethodTests
{
    [Fact]
    public void Contains_ItemInTheList_True()
    {
        var k = 10;
        var list = new SortedList<int> { 1, -100, 200, 87, -600, 3, k };

        bool contains = list.Contains(k);

        Assert.True(contains);
        Assert.Contains(k, list);
    }

    [Fact]
    public void Contains_ItemNotInTheList_False()
    {
        var k = 10;
        var list = new SortedList<int> { 1, -100, 200, 87, -600, 3 };

        bool contains = list.Contains(k);

        Assert.False(contains);
        Assert.DoesNotContain(k, list);
    }

    [Fact]
    public void Contains_Null_ArgumentNullException()
    {
        var list = new SortedList<string> {};
        string nullString = null;

        Assert.Throws<ArgumentNullException>(() => list.Contains(nullString));
    }
}
