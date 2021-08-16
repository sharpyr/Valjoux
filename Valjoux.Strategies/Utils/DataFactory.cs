using System;
using System.Collections.Generic;
using Analys;
using Veho;
using Veho.List;
using Veho.Vector;

namespace Valjoux.Utils {
  public static class DataFactory {
    public static Table<double> Init(List<(string name, Action action)> methods) => Table<double>.Build(
      methods.Map(kv => kv.name).ToArray(),
      Vec.Iso<double>(methods.Count, 0).ToRow()
    );
    public static (Crostab<double> elapsed, Crostab<TO> result) Init<T, TO>(
      List<(string name, Func<T, TO> func)> methods,
      List<(string key, T value)> parameters
    ) {
      string[]
        m = methods.Map(kv => kv.name).ToArray(),
        p = parameters.Map(kv => kv.key).ToArray();
      return (Crostab<double>.Build(p, m), Crostab<TO>.Build(p, m));
    }
  }
}