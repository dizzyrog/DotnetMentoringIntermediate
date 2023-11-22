using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    public static async Task<long> CalculateAsync(int n , CancellationToken token)
    {
        try
        {
            var sum = await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                long sum = 0;

                for (var i = 0; i < n; i++)
                {
                    // i + 1 is to allow 2147483647 (Max(Int32)) 
                    sum = sum + (i + 1);
                    Thread.Sleep(10);
                }

                return sum;
            }, token);
            return sum;
        }
        catch (Exception)
        {
            // Handle the cancellation
            Console.WriteLine("Calculation was cancelled.");
            throw; 
        }
    }
}
