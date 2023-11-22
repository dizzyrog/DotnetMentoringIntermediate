using System;
using System.Linq;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        [DataRow(70)] // fails
        [DataRow(87)] // sometimes passes, sometimes fails
        [DataRow(120)] //passes
        public void ParallelEfficiencyTest(int sizeOfMatrix)
        {
            var stopwatch = new Stopwatch();

            var firstMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            var secondMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            
             // Measure synchronous multiplication
             stopwatch.Start();
             var resultSync = new MatricesMultiplier().Multiply(firstMatrix, secondMatrix);
             stopwatch.Stop();
             Console.WriteLine($"Synchronous multiplication took: {stopwatch.ElapsedMilliseconds}ms");
             Console.WriteLine($"Synchronous multiplication took: {GetMicroseconds(stopwatch)} microseconds");
             var syncTime = GetMicroseconds(stopwatch);
             stopwatch.Reset();
            
             // Measure parallel multiplication
             stopwatch.Start();
             var resultParallel = new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix);
             stopwatch.Stop();
             Console.WriteLine($"Parallel multiplication took: {stopwatch.ElapsedMilliseconds}ms");
             Console.WriteLine($"Parallel multiplication took: {GetMicroseconds(stopwatch)} microseconds");
             var parallelTime = GetMicroseconds(stopwatch);
             
             Assert.IsTrue(parallelTime < syncTime, "Parallel multiplication should be faster than synchronous multiplication.");
        }
        
        #region private methods
        
        private static double GetMicroseconds(Stopwatch stopwatch)
        {
            return (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1_000_000;
        }
        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        #endregion
    }
}
