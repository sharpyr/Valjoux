using System;
using System.Collections.Generic;
using Analys;
using NUnit.Framework;
using Palett.Presets;
using Spare.Logger;
using Typen.Numeral;
using Veho.Dictionary;
using Veho.Types;

namespace Valjoux.Test {
  [TestFixture]
  public class ElPrimeroTest {
    [Test]
    public void TestFunc() {
      var candidates = new Dictionary<string, string> {
        {"alpha", "5"},
        {"beta", "-"},
        {"gamma", ""},
      };
      var (elapsed, result) = Strategies.Run(
        (int) 1E+6,
        Dict.From<string, Func<string, double>>(
          ("CastDouble", Num.CastDouble),
          ("CastDouble2", Num.CastDouble)
        ),
        candidates
      );
      "\nElapsed".Logger();
      elapsed.Deco(orient: Operated.Rowwise, presets: (PresetCollection.Subtle, PresetCollection.Fresh)).Logger();
      "\nResult".Logger();
      result.Deco().Logger();
      // ElPrimero.Profile("some", (int) 1E+6, () => "5".CastDouble());
    }
  }
}