using System;
using System.Collections.Generic;
using System.Linq;
using Analys;
using Veho;
using Veho.Vector;

namespace Valjoux.Utils {
  public static class DataFactory {
    public static Table<double> Init(Dictionary<string, Action> methods) => Table<double>.Build(
      methods.Keys.ToArray(),
      Vec.Iso<double>(methods.Count, 0).ToRow()
    );
    public static (Crostab<double> elapsed, Crostab<TO> result) Init<T, TO>(
      Dictionary<string, Func<T, TO>> methods,
      Dictionary<string, T> parameters
    ) {
      string[] m = methods.Keys.ToArray(), p = parameters.Keys.ToArray();
      return (Crostab<double>.Build(p, m), Crostab<TO>.Build(p, m));
    }
  }
}