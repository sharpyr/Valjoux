using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Valjoux.Utils {
  public static class Profilers {
    public static double Profile(
      this Stopwatch eta,
      int iterations,
      string label,
      Action method
    ) {
      //Run at highest priority to minimize fluctuations caused by other processes/threads
      Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
      Thread.CurrentThread.Priority = ThreadPriority.Highest;
      // warm up 
      method();
      // clean up
      GC.Collect();
      GC.WaitForPendingFinalizers();
      GC.Collect();
      eta.Restart();
      for (var i = 0; i < iterations; i++) method();
      eta.Stop();
      var elapsed = eta.Elapsed.TotalMilliseconds;
      Console.WriteLine($"Elapsed {elapsed} ms: {label}()");
      return elapsed;
    }

    public static (double elapsed, TO result) Profile<T, TO>(
      this Stopwatch eta,
      int iterations,
      (string name, Func<T, TO> func) method,
      (string key, T value) parameter
    ) {
      var v = parameter.value;
      var f = method.func;
      //Run at highest priority to minimize fluctuations caused by other processes/threads
      Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
      Thread.CurrentThread.Priority = ThreadPriority.Highest;
      // warm up 
      var result = f(v);
      // clean up
      GC.Collect();
      GC.WaitForPendingFinalizers();
      GC.Collect();
      eta.Restart();
      for (var i = 0; i < iterations; i++) f(v);
      eta.Stop();
      var elapsed = eta.Elapsed.TotalMilliseconds;
      Console.WriteLine($"Elapsed {elapsed} ms: {method.name}({parameter.key})");
      return (elapsed, result);
    }
  }
}