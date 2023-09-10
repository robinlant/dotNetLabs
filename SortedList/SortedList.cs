using System.Collections;

namespace SortedList;


// FOR NOW IT COMPARES USING EQUALS (LINKS FOR CLASSES) CHANGE IT LATER
// ALSO AS I AM DOING A SORTED LIST MANY THINGS CAN BE OPTIMIZED BUT LATER
// CURRENTLY IT IS DEFAULT LINKED LIST WITHOUT A TAIL (I DONT NEED ONE FOR SORTED COLLECTION)
internal class SortedList<T> : ICollection<T> where T : IComparable<T>
{
    private MyNode<T>? _head;
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
            IncrementCount();
            UpdateVersion();

            return;
        }

        var current = _head;
        while (current.Next != null)
        {
            current = current.Next;
        }

        current.Next = new MyNode<T>(item);
        IncrementCount();
        UpdateVersion();
    }

    public void Clear()
    {
        _head = null;
        ResetVersion();
        ResetCount();
    }

    public bool Contains(T item)
    {
        var current = _head;

        while (current != null)
        {
            if (current.Item.Equals(item)) return true;
            current = current.Next;
        }

        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
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
        if (Count == 0) return false;
        if (item!.Equals(_head))
        {
            if (_head!.Next != null)
            {
                _head = _head.Next;
                UpdateVersion();
                DecrementCount();
                return true;
            }

            _head = null;
            DecrementCount();
            UpdateVersion();
            return true;
        }

        var current = _head;
        var prev = current;
        while (current != null)
        {
            if (current.Item!.Equals(item))
            {
                prev!.Next = current.Next;
                DecrementCount();
                UpdateVersion();
                return true;
            }
            prev = current;
            current = current.Next;
        }

        return false;
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

    public MyNode(T value)
    {
        Item = value;
    }
}