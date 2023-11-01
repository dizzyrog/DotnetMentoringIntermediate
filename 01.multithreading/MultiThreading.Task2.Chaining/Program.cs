/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {

            var random = new Random();

            // First Task - Creates an array of 10 random integers
            var firstTask = Task.Run(() =>
            {
                var numbers = new int[10];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = random.Next(1, 100); // Random integers between 1 and 99
                }

                Console.WriteLine("First Task: Array of 10 random integers:");
                Console.WriteLine(string.Join(", ", numbers));
                return numbers;
            });

            // Second Task - Multiplies this array with another random integer
            var secondTask = firstTask.ContinueWith(previousTask =>
            {
                int multiplier = random.Next(1, 10); // Random integer between 1 and 9
                var multipliedNumbers = previousTask.Result.Select(n => n * multiplier).ToArray();

                Console.WriteLine($"\nSecond Task: Multiplied by {multiplier}:");
                Console.WriteLine(string.Join(", ", multipliedNumbers));
                return multipliedNumbers;
            });

            // Third Task - Sorts this array by ascending
            var thirdTask = secondTask.ContinueWith(previousTask =>
            {
                var sortedNumbers = previousTask.Result.OrderBy(n => n).ToArray();
                Console.WriteLine("\nThird Task: Sorted array by ascending:");
                Console.WriteLine(string.Join(", ", sortedNumbers));
                return sortedNumbers;
            });

            // Fourth Task - Calculates the average value
            var fourthTask = thirdTask.ContinueWith(previousTask =>
            {
                double average = previousTask.Result.Average();
                Console.WriteLine($"\nFourth Task: Average value = {average}");
            });

            fourthTask.Wait();  // Ensure the program doesn't exit before the tasks are complete
        }
    }
}
