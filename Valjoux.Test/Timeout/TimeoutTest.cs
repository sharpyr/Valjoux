using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Spare;

namespace Valjoux.Test.Timeout {
  [TestFixture]
  public class TimeoutTest {
    [Test]
    public static void WaitTest() {
      var task = Task.Run(() => {
        var random = new Random();
        long sum = 0;
        const int n = 5000000;
        for (var ctr = 1; ctr <= n; ctr++) {
          var number = random.Next(0, 101);
          sum += number;
        }
        Console.WriteLine("Total:   {0:N0}", sum);
        Console.WriteLine("Mean:    {0:N2}", sum / n);
        Console.WriteLine("N:       {0:N0}", n);
      });
      var timeSpan = TimeSpan.FromMilliseconds(50);
      if (task.Wait(timeSpan)) {
        Console.WriteLine("Task executed within timeSpan");
      } else {
        Console.WriteLine("The timeout interval elapsed.");
      }
    }
    [Test]
    public static void DelayTest() {
      var eta = new Stopwatch();
      var source = new CancellationTokenSource();
      var task = Task.Run(function: async () => {
        await Task.Delay(1000, source.Token);
        return 42;
      });
      // source.Cancel();
      try {
        eta.Start();
        Console.WriteLine($"[>>> begin] {eta.Elapsed}");
        task.Wait();
        Console.WriteLine($"[>>> end] {eta.Elapsed}");
      }
      catch (AggregateException ae) {
        foreach (var e in ae.InnerExceptions)
          Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
      }
      Console.Write("Task t Status: {0}", task.Status);
      if (task.Status == TaskStatus.RanToCompletion) Console.Write(", Result: {0}", task.Result);
      source.Dispose();
    }
    [Test]
    public static void SimpleDelayTest() {
      var eta = new Stopwatch();
      var taskAlpha = Task.Run(function: async () => {
        await Task.Delay(1500);
        return "alpha";
      });
      var taskBeta = Task.Run(function: async () => {
        await Task.Delay(1000);
        return "beta";
      });
      eta.Start();
      Console.WriteLine($"[>>> begin] {eta.Elapsed}");
      taskAlpha.Wait();
      taskAlpha.Result.Logger();
      taskBeta.Wait();
      taskBeta.Result.Logger();
      Console.WriteLine($"[>>> end] {eta.Elapsed}");
    }
  }
}