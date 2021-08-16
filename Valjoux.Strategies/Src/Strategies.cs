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
      List<(string name, Action action)> methods
    ) {
      var table = DataFactory.Init(methods);
      var eta = new Stopwatch();
      foreach (var m in methods)
        try { table[0, m.name] = eta.Profile(iteration, m.name, m.action); }
        catch (Exception) { table[0, m.name] = double.NaN; }
      return table;
    }

    public static (Crostab<double> elapsed, Crostab<TO> result) Run<T, TO>(
      int iteration,
      List<(string name, Func<T, TO> func)> methods,
      List<(string key, T value)> parameters
    ) {
      var (elapsed, result) = DataFactory.Init(methods, parameters);
      var eta = new Stopwatch();
      foreach (var m in methods)
        foreach (var p in parameters)
          try { (elapsed[p.key, m.name], result[p.key, m.name]) = eta.Profile(iteration, m, p); }
          catch (Exception) { elapsed[p.key, m.name] = double.NaN; }
      elapsed.UnshiftRow("Average", elapsed.Rows.MapColumns(col => col.Average()));
      elapsed = elapsed.Map(x => Math.Round(x, 1));
      return (elapsed, result);
    }
  }
}