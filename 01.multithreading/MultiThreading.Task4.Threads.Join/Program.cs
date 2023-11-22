/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static Semaphore semaphore = new (0, 1);
        static void Main(string[] args)
        {
            CreateThread(10);

            Console.WriteLine("a option done, press any key to continue to b option");
            Console.ReadKey();
            
            ThreadPool.QueueUserWorkItem(ThreadBodySemaphore, 10);
            semaphore.WaitOne();
        }
        
        static void CreateThread(int n)
        {
            if (n <= 0) return;

            Thread thread = new Thread(ThreadBody);
            thread.Start(n);
            thread.Join();
        }

        static void ThreadBody(object state)
        {
            int n = (int)state;
            Console.WriteLine(n);
            CreateThread(n - 1);
        }
        
        static void ThreadBodySemaphore(object state)
        {
            int n = (int)state;
            Console.WriteLine(n);
        
            if (n > 1)
            {
                ThreadPool.QueueUserWorkItem(ThreadBodySemaphore, n - 1);
            }
            else
            {
                semaphore.Release();
            }
        }
    }
}
