
using SortedList;

namespace ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        // Initialization
        var list = new SortedList<int>() {1, 8, 20, 11, 9,-1245, 44, 915 , -142};
        Print(list);
        list.Add(5);
        Print(list);
        // Events
        var AddCounter = 0;
        var RemoveCounter = 0;
        list.ItemAdded += (e,s) => AddCounter++;
        list.ItemRemoved += (e,s) => RemoveCounter++;
        list.ListCleared += (e, s) => Console.WriteLine("LIST WAS CLEARED.....");
        Console.WriteLine($"Add: {AddCounter}  Remove: {RemoveCounter}");
        // Add Remove Methods
        list.Add(33);
        list.Remove(11);
        Print(list);
        Console.WriteLine($"Add: {AddCounter}  Remove: {RemoveCounter}");
        // Copy To
        var arr = new int[list.Count + 2];
        arr[0] = 0;
        arr[1] = 0;
        list.CopyTo(arr,2);
        Print(arr);
        // Contains
        Console.WriteLine(list.Contains(1).ToString() + "  " + list.Contains(33).ToString());
        // Clear Method
        list.Clear();
        Print(list);
    }

    static void Print<T>(IEnumerable<T> list)
    {
        Console.Write("[");
        foreach (var var in list)
        {
            Console.Write(" " + var + ",");
        }

        var temp = Console.GetCursorPosition();
        Console.SetCursorPosition(temp.Left - 1, temp.Top);
        Console.WriteLine("]");
        // Reversed Enumerator
        Console.Write("[");
        foreach (var var in list.Reverse())
        {
            Console.Write(" " + var + ",");
        }

        temp = Console.GetCursorPosition();
        Console.SetCursorPosition(temp.Left - 1, temp.Top);
        Console.WriteLine("]");
    }
}