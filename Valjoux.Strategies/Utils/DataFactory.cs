using System;
using System.Collections.Generic;
using Analys;
using Veho;
using Veho.Vector;

namespace Valjoux.Utils {
  public static class DataFactory {
    public static Table<double> Init(List<(string name, Action action)> methods) => Table<double>.Build(
      methods.Map(kv => kv.name),
      Vec.Iso<double>(methods.Count, 0).ToRow()
    );
    public static (Crostab<double> elapsed, Crostab<TO> result) Init<T, TO>(
      List<(string name, Func<T, TO> func)> methods,
      List<(string key, T value)> parameters
    ) {
      string[]
        m = methods.Map(kv => kv.name),
        p = parameters.Map(kv => kv.key);
      return (Crostab<double>.Build(p, m), Crostab<TO>.Build(p, m));
    }
  }
}