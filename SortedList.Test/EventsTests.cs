namespace SortedList.Test;

public class EventsTests
{
    [Fact]
    public void ItemAdded_ItemAdded_EventInvoked()
    {
        var list = new SortedList<int>();


        var isInvoked = false;
        list.ItemAdded += (o, e) => isInvoked = true;
        list.Add(default);

        Assert.True(isInvoked);
    }

    [Fact]
    public void ItemRemoved_ItemRemovedReturnsTrue_EventInvoked()
    {
        var list = new SortedList<int>() {default};

        var isInvoked = false;
        list.ItemRemoved += (o, e) => isInvoked = true;
        list.Remove(default);

        Assert.True(isInvoked);
    }

    [Fact]
    public void ItemRemoved_ItemRemovedReturnsFalse_EventIsNotInvoked()
    {
        var list = new SortedList<int>() {};

        var isInvoked = false;
        list.ItemRemoved += (o, e) => isInvoked = true;
        list.Remove(default);

        Assert.False(isInvoked);
    }

    [Fact]
    public void ListCleared_RemoveLastElement_EventIsInvoked()
    {
        var list = new SortedList<int> {default};

        var isInvoked = false;
        list.ListCleared += (o, e) => isInvoked = true;
        list.Remove(default);

        Assert.True(isInvoked);
    }
    [Fact]
    public void ListCleared_ListClearedWhileBeingNotEmpty_EventIsInvoked()
    {
        var list = new SortedList<int> {default};

        var isInvoked = false;
        list.ListCleared += (o, e) => isInvoked = true;
        list.Clear();

        Assert.True(isInvoked);
    }

    [Fact]
    public void ListCleared_ListClearedWhileBeingEmpty_EventIsNotInvoked()
    {
        var list = new SortedList<int> { };

        var isInvoked = false;
        list.ListCleared += (o, e) => isInvoked = true;
        list.Clear();

        Assert.False(isInvoked);
    }
}