using System.Collections;

namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        ArrayList list = new ArrayList();
        list.Add("A");
        list.Add("B");
        list.Add("C");
        list.Add("D");
        list.Add("E");
        list.Add("F");
        list.Add("H");
        
        list.Remove(2);

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine("Item: " + list[i] + " - " + "list[" + (i + 1) + "]");
        }
        
        
        list.Insert(2, 5);
        
        ArrayList list2 = new ArrayList();
        list2.Add("1");
        list2.Add("2");
        list2.Add("3");
        list2.Add("4");
        list2.Add("5");
        
        list.Insert(list.Count, list2);
        list2.Clear();
        
        
        
        Hashtable hashtable = new Hashtable();
        hashtable.Add("A", 1);
        hashtable.Add("B", 1);
        hashtable.Add("C", 1);
        hashtable.Add("D", 1);
        hashtable.Add("E", 1);
        
        hashtable.Remove("A");

        foreach (DictionaryEntry VARIABLE in hashtable)
        {
            Console.WriteLine(VARIABLE.Key + " - " + VARIABLE.Value);
        }
        
        bool checkin = hashtable.ContainsKey("A");

        if (checkin)
        {
            Console.WriteLine("Checkin");   
        }
        
        
        
        Stack stack = new Stack();
        stack.Push("A");
        stack.Push("B");
        stack.Push("C");
        stack.Push("D");
        stack.Push("E");

        while(stack.Count > 1)
        {
            Console.WriteLine("Item: " + stack.Pop() + " - " + stack.Peek());
        }
        
        
        Queue queue = new Queue();
        queue.Enqueue("A");
        queue.Enqueue("B");
        queue.Enqueue("C");
        queue.Enqueue("D");
        queue.Enqueue("E");

        while (queue.Count > 0)
        {
            Console.WriteLine("Item: " + queue.Dequeue());
        }
        
        
        
        //________________________
        Random random = new Random();
        ArrayList list3 = new ArrayList();

        for (int i = 0; i < 50; i++)
        {
            //list3.Add(random.Next((int) DateTime.Now.Ticks));
        }
        
        SelectionSort(list3);

        foreach (var VARIABLE in list3)
        {
            Console.WriteLine(VARIABLE);
        }


        //-------------------------
        Hashtable newHashTable = new Hashtable();
        newHashTable.Add("Key1", "Value1");
        newHashTable.Add("Key2", "Value2");
        newHashTable.Add("Key3", "Value3");
        newHashTable.Add("Key4", "Value4");
        newHashTable.Add("Key5", "Value5");

        Console.WriteLine(hashtable.Count);


        //-------------------------
        SortedList sortedList = new SortedList();
        sortedList.Add("Key1", "Value1");
        sortedList.Add("Key2", "Value2");
        sortedList.Add("Key3", "Value3");
        sortedList.Add("Key4", "Value4");
        sortedList.Add("Key5", "Value5");
        sortedList.Add("Key6", "Value6");

        Console.WriteLine(sortedList);
        Console.WriteLine("Count" + sortedList.Count);
        Console.WriteLine("Capacity: " + sortedList.Capacity);
        Console.WriteLine("Keys and Values: ");

        //--------------------
        List<Student> newList = new List<Student>
        {
            new Student(1, "Scove"),
            new Student(2, "Scove"),
            new Student(3, "Scove"),
            new Student(4, "Scove"),
            new Student(5, "Scove")
        };

        for (int i = 0; i < newList.Count; i++)
        {
            Console.WriteLine(newList[i]);
        }

        Dictionary<string, int> newDictionary = new Dictionary<string, int>();
        newDictionary.Add("Scove", 1);
        newDictionary.Add("Scovy", 2);
        
        if (newDictionary.ContainsKey("Scove"))
        {
            Console.WriteLine("Scove is " + newDictionary["Scove"] + " year(s) in IT dev");
        }

        newDictionary["Scovy"] = 26;
        newDictionary.Remove("Scove");

        foreach (var idk in newDictionary)
        {
            Console.WriteLine($"{ idk.Key}: {idk.Value}");
        }

        Queue<string> tasks = new Queue<string>();
        tasks.Enqueue("Download file");
        tasks.Enqueue("Scan file");
        Console.WriteLine("Next task: " + tasks.Peek());
        Console.WriteLine("Next task: " + tasks.Dequeue());

        foreach (var task in tasks )
        {
            Console.WriteLine(task);
        }
    }

    static void SelectionSort(ArrayList list)
    {
        int n = list.Count;

        for (int i = 0; i < n + 1; i++)
        {
            int min_idx = i;

            for (int j = i + 1; j < n; j++)
            {
                IComparable curr_idx = (IComparable)list[j];
                IComparable min = (IComparable)list[min_idx];
                
                if (curr_idx.CompareTo(min) < 0)
                {
                    min_idx = j;
                }
            }

            if (min_idx != i)
            {
                (list[i], list[min_idx]) = (list[min_idx], list[i]);
            }
        }
    }
}