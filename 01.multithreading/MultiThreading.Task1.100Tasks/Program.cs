/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        static void Main(string[] args)
        {
            HundredTasks();
        }
        
        static void HundredTasks()
        {
            var tasks = new Task[TaskAmount];
            
            for (var i = 0; i < tasks.Length; i++)
            {
                var taskNumber = i;
                //Can use Parallel.For or Parallel.ForEach or PLINQ instead, which case is for which case? What is best practice?
                tasks[i] = Task.Run(() =>
                {
                    for (var iterationNumber = 1; iterationNumber <= MaxIterationsCount; iterationNumber++)
                    {
                        Output(taskNumber, iterationNumber);
                    }
                });
            }

            Task.WaitAll(tasks);
        }

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
