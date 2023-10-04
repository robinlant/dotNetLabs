namespace SortedListTests;

public class AddMethodTests
{
    [Fact]
    public void Add_NoDuplicates_SortedList()
    {
        var list = new SortedList<int> { 1, -100, 200, 87, -600, 3 };

        Assert.Collection(list,
            item => Assert.Equal(-600,item),
            item => Assert.Equal(-100,item),
            item => Assert.Equal(1,item),
            item => Assert.Equal(3,item),
            item => Assert.Equal(87,item),
            item => Assert.Equal(200,item)
            );
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
        var list = new SortedList<string> { "a", "b", "c" };
        string nullString = null;

        Assert.Throws<ArgumentNullException>(() => list.Add(nullString));
    }
}