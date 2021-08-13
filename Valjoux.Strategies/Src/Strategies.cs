using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Analys;
using Valjoux.Utils;
using Veho.Columns;

namespace Valjoux {
  public static class Strategies {
    public static Table<double> Run(
      int iteration,
      Dictionary<string, Action> methods
    ) {
      var table = DataFactory.Init(methods);
      var eta = new Stopwatch();
      foreach (var m in methods)
        try { table[0, m.Key] = eta.Profile(iteration, m.Key, m.Value); }
        catch (Exception) { table[0, m.Key] = double.NaN; }
      return table;
    }

    public static (Crostab<double> elapsed, Crostab<TO> result) Run<T, TO>(
      int iteration,
      Dictionary<string, Func<T, TO>> methods,
      Dictionary<string, T> parameters
    ) {
      var (elapsed, result) = DataFactory.Init(methods, parameters);
      var eta = new Stopwatch();
      foreach (var m in methods)
        foreach (var p in parameters)
          try { (elapsed[p.Key, m.Key], result[p.Key, m.Key]) = eta.Profile(iteration, m, p); }
          catch (Exception) { elapsed[p.Key, m.Key] = double.NaN; }
      elapsed.UnshiftRow("Average", elapsed.Rows.MapColumns(col => col.Average()));
      elapsed = elapsed.Map(x => Math.Round(x, 1));
      return (elapsed, result);
    }
  }
}