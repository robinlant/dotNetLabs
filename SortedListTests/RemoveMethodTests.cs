namespace SortedListTests;

public class RemoveMethodTests
{
    [Fact]
    public void Remove_ValidArg_True()
    {
        var k = 10;
        var list = new SortedList<int> { 1, -100, 200, 87, -600, 3, k };

        bool isDeletionSuccessful = list.Remove(k);

        Assert.True(isDeletionSuccessful);
        Assert.DoesNotContain(k, list);
    }

    [Fact]
    public void Remove_InvalidItem_False()
    {
        var k = 10;
        var list = new SortedList<int> { 1, -100, 200, 87, -600, 3 };

        bool isDeletionSuccessful = list.Remove(k);

        Assert.False(isDeletionSuccessful);
        Assert.DoesNotContain(k, list);
    }

    [Fact]
    public void Remove_Null_ArgumentNullException()
    {
        var list = new SortedList<string> { "a", "b", "c" };
        string nullString = null;

        Assert.Throws<ArgumentNullException>(() => list.Remove(nullString));
    }
}