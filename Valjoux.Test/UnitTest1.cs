using System;
using Analys;
using NUnit.Framework;
using Palett.Presets;
using Spare.Logger;
using Veho.Dictionary;
using Veho.Types;

namespace Valjoux.Test {
  [TestFixture]
  public class ElPrimeroTest {
    [Test]
    public void TestFunc() {
      var (elapsed, result) = Strategies.Run(
        (int) 1E+6,
        Dict.From<string, Func<object, double>>(
          ("Generic", Methods.GenericCastDouble),
          ("Generic1", Methods.GenericCastDouble1),
          ("Generic2", Methods.GenericCastDouble2),
          ("Generic3", Methods.GenericCastDouble3)
        ),
        Dict.From<string, object>(
          ("alpha", "5"),
          ("beta", "-"),
          ("gamma", ""),
          ("delta", null),
          ("omega", 5),
          ("yelp", 0.618D),
          ("zeta", 0.618F)
        )
      );
      "\nElapsed".Logger();
      elapsed.Deco(orient: Operated.Rowwise, presets: (PresetCollection.Subtle, PresetCollection.Fresh)).Logger();
      "\nResult".Logger();
      result.Deco().Logger();
    }
  }

  public static class Methods {
    public static double GenericCastDouble<T>(this T o) {
      if (o == null) return double.NaN;
      switch (o) {
        case double n: return n;
        case string s: return s.CastDouble();
        case bool n: return n ? 1 : 0;
        case int n: return n;
        case long n: return n;
        case decimal n: return (double) n;
        case float n: return n;
        case byte n: return n;
        // case sbyte n: return n;
        // case short n: return n;
        // case ushort n: return n;
        // case uint n: return n;
        // case ulong n: return n;
        // case char n: return n;
        default: return o.ToString().CastDouble();
      }
    }

    public static double GenericCastDouble1<T>(this T o) {
      if (o == null) return double.NaN;
      switch (o) {
        case double n: return n;
        case string s: return s.CastDouble();
        // case sbyte n: return n;
        // case short n: return n;
        case int n: return n;
        case long n: return n;
        case decimal n: return (double) n;
        case float n: return n;
        case bool n: return n ? 1 : 0;
        // case byte n: return n;
        // case ushort n: return n;
        // case uint n: return n;
        // case ulong n: return n;
        default: return o.ToString().CastDouble();
      }
    }

    public static double GenericCastDouble2<T>(this T o) {
      if (o == null) return double.NaN;
      switch (o) {
        case string s: return s.CastDouble();
        default: return o.ToString().CastDouble();
      }
    }

    public static double GenericCastDouble3<T>(this T o) {
      double PrimitiveToDouble(T v) {
        switch (v) {
          case bool n: return n ? 1 : 0;
          case byte n: return n;
          case sbyte n: return n;
          case short n: return n;
          case ushort n: return n;
          case int n: return n;
          case uint n: return n;
          case long n: return n;
          case ulong n: return n;
          // case IntPtr n: return n;
          // case UIntPtr n: return n;
          case char n: return n;
          case double n: return n;
          case float n: return n;
          default: return o.ToString().CastDouble();
        }
      }
      if (o == null) return double.NaN;
      if (typeof(T).IsPrimitive) return PrimitiveToDouble(o);
      if (o is string s) return s.CastDouble();
      return o.ToString().CastDouble();
    }

    public static double CastDouble(this string t) => double.TryParse(t, out var n) ? n : double.NaN;
  }
}