/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static readonly List<int> sharedCollection = new ();
        static readonly object lockObject = new ();
        static readonly ManualResetEvent canPrint = new (false);
        static readonly ManualResetEvent canAdd = new (true);
        
        static void Main(string[] args)
        {
            Thread addThread = new Thread(AddItems);
            Thread printThread = new Thread(PrintItems);

            addThread.Start();
            printThread.Start();

            addThread.Join();
            printThread.Join();
        }

        static void AddItems()
        {
            for (int i = 1; i <= 10; i++)
            {
                canAdd.WaitOne();
                lock (lockObject)
                {
                    sharedCollection.Add(i);
                    Console.WriteLine($"Added: {i}");
                }
                canAdd.Reset();
                canPrint.Set();
            }
        }

        static void PrintItems()
        {
            for (int i = 0; i < 10; i++)
            {
                canPrint.WaitOne();
                lock (lockObject)
                {
                    Console.Write("Collection: ");
                    foreach (var item in sharedCollection)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                }
                canPrint.Reset();
                canAdd.Set();
            }
        }
    }
}
