/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        static CancellationToken token = cancellationTokenSource.Token;

        static void Main(string[] args)
        {
            RunTaskA();
            RunTaskB();
            RunTaskC();
            RunTaskD();
        }

        static void RunTaskA()
        {
            Task taskA = Task.Run(() =>
            {
                Console.WriteLine("Task A running...");
                throw new Exception("Exception in Task A");
            }).ContinueWith(previousTask => { Console.WriteLine("Continuation for Task A (Regardless of result)"); });

        }

        static void RunTaskB()
        {
            Task taskB = Task.Run(() =>
            {
                Console.WriteLine("Task B running...");
                throw new Exception("Exception in Task B");
            });

            Task continuationB =
                taskB.ContinueWith(previousTask => { Console.WriteLine("Continuation for Task B (On Faulted)"); },
                    TaskContinuationOptions.OnlyOnFaulted);

        }

        static void RunTaskC()
        {
            Task taskC = Task.Run(() =>
            {
                Console.WriteLine("Task C running on thread " + Thread.CurrentThread.ManagedThreadId);
                throw new Exception("Exception in Task C");
            });

            Task continuationC = taskC.ContinueWith(
                previousTask =>
                {
                    Console.WriteLine("Continuation for Task C (On Faulted, Same thread) on thread " +
                                      Thread.CurrentThread.ManagedThreadId);
                }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

        }

        static void RunTaskD()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task taskD = Task.Run(() =>
            {
                Console.WriteLine("Task D running...");
                cts.Cancel();
                cts.Token.ThrowIfCancellationRequested();
            }, cts.Token);

            Task continuationD = taskD.ContinueWith(previousTask =>
            {
                Console.WriteLine("Continuation for Task D (On Canceled, Long Running)");
                Console.WriteLine("Is ThreadPool thread: " + Thread.CurrentThread.IsThreadPoolThread);
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

        }
    }
}