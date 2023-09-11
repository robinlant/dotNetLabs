using System.Collections;

namespace SortedList;


// FOR NOW IT COMPARES USING EQUALS (LINKS FOR CLASSES) CHANGE IT LATER
// ALSO AS I AM DOING A SORTED LIST MANY THINGS CAN BE OPTIMIZED BUT LATER
// CURRENTLY IT IS DEFAULT LINKED LIST WITHOUT A TAIL (I DONT NEED ONE FOR SORTED COLLECTION)
internal class SortedList<T> : ICollection<T> where T : IComparable<T>
{
    private MyNode<T>? _head;
    private MyNode<T>? _tail;
    public bool IsReadOnly { get; } = false;
    public int Count { get; private set; } // length of the list
    public int Version { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        IEnumerator <T> enumerator = new MyEnumerator(_head, this);
        return enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        // head is null
        if (_head == null)
        {
            _head = new MyNode<T>(item);
            _tail = _head;
            IncrementCount();
            UpdateVersion();

            return;
        }
        // item < head
        if (CountSubstraction(_head.Item, item) > 0)
        {
            var node = new MyNode<T>(item) { Next = _head };
            _head.Prev = node;
            _head = node;
            IncrementCount();
            UpdateVersion();

            return;
        }

        var current = _head;
        while (current.Next != null)
        {
            if (CountSubstraction(current.Item, item) > 0)
            {
                var node = new MyNode<T>(item) { Next = current, Prev = current.Prev};
                current.Prev = node;
                node.Prev!.Next = node;
                IncrementCount();
                UpdateVersion();

                return;
            }

            current = current.Next;
        }

        _tail = new MyNode<T>(item) { Prev = current };
        current.Next = _tail;
        IncrementCount();
        UpdateVersion();
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
        ResetVersion();
        ResetCount();
    }

    public bool Contains(T item)
    {
        var current = _head;
        while (current != null)
        {
            var temp = CountSubstraction(current.Item, item);
            if (temp < 0) return false; // further numbers are bigger
            if (temp == 0) return true;
            current = current.Next;
        }

        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException($"Array {nameof(array)} is null");
        }

        if (array.Length - arrayIndex < Count )
        {
            throw new ArgumentException("Not enough space. Count > array length - starting index");
        }

        if (arrayIndex < 0 || arrayIndex >= array.Length)
        {
            throw new ArgumentException($"Invalid Argument. arrayIndex = {arrayIndex}. It has to be greater than zero and smaller than array length");
        }

        var i = 0;
        foreach (var item in this)
        {
            array[arrayIndex + i] = item;
            i++;
        }
    }

    public bool Remove(T item)
    {
        if (_head == null) return false;
        if (_head.Item.CompareTo(item) > 0) return false; // item is not in the list
        if (_head.Item.CompareTo(item) == 0) // head  == item
        {
            if (_head.Next == null)
            {
                _head = null;
                _tail = null;
                ResetVersion();
                ResetCount();

                return true;
            }

            _head = _head.Next;
            UpdateVersion();
            DecrementCount();
        }

        var current = _head;
        while (current!.Next != null)
        {
            var temp = current.Item.CompareTo(item);
            if (temp > 0) return false; // item is not in the list
            if (temp == 0)
            {
                if (current.Next == null)
                {
                    _tail = current.Prev;
                    current.Prev!.Next = null;
                    DecrementCount();
                    UpdateVersion();

                    return true;
                }

                current.Next!.Prev = current.Prev;
                current.Prev!.Next = current.Next;
                DecrementCount();
                UpdateVersion();

                return true;
            }

            current = current.Next;
        }

        return false;
    }
//TODO remove CountSubstraction after the program is DONE
    private int CountSubstraction(T a, T b)
    {
        return a.CompareTo(b);
    }

    // version and length control methods
    private void UpdateVersion()
    {
        Version++;
    }

    private void ResetVersion()
    {
        Version = 0;
    }

    private void IncrementCount()
    {
        Count++;
    }

    private void DecrementCount()
    {
        Count--;
    }

    private void ResetCount()
    {
        Count = 0;
    }

    // Enumerator
    private class MyEnumerator : IEnumerator<T>
    {
        private MyNode<T>? _current;
        private MyNode<T>? _head;

        private readonly SortedList<T> _list;
        private readonly int _listStarterVersion;

        public MyEnumerator(MyNode<T>? head, SortedList<T> list)
        {
            _current = null;
            _head = head;
            _list = list;
            _listStarterVersion = _list.Version;
        }

        public T Current
        {
            get
            {
                CheckVersion();
                if (_current == null)
                    throw new InvalidOperationException("Enumeration has not been started ot it is already finished");
                return _current.Item; // THIS ENSURES THAT USER WILL WORK WITH T INSTEAD OF THE NODE
            }
        }

        private void CheckVersion()
        {
            if (_listStarterVersion != _list.Version)
            {
                throw new InvalidOperationException("Collection was modified");
            }
        }

        public bool MoveNext()
        {
            CheckVersion();
            if (_current == null)
            {
                if (_head == null)
                    return false; // list is empty

                _current = _head;
                return true;
            }

            _current = _current.Next;
            return _current != null;
        }

        public void Reset()
        {
            throw new NotSupportedException("Reset is not implemented due to safety reasons");
            // If changes are made to the collection,
            // such as adding, modifying, or deleting elements,
            // the behavior of Reset is undefined.
            // https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.reset?view=net-6.0
        }

#nullable disable
        object IEnumerator.Current => Current;
#nullable enable

        public void Dispose() { }
    }
}

internal class MyNode<T>
{
    public T Item { get; set; }
    public MyNode<T>? Next { get; set; }
    public MyNode<T>? Prev { get; set; }

    public MyNode(T value)
    {
        Item = value;
    }
}