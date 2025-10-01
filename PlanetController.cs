using UnityEngine;
using System;

public class PlanetController : MonoBehaviour {
  public OrbitalElements elements;
  [Tooltip("YYYY-MM-DD (UTC midnight)")]
  public string dateIso = "2000-01-01";

  void Update() {
    if (!TryParseDate(dateIso, out DateTime utc)) return;
    double jd = DateTimeProvider.ToJulianDay(DateTime.SpecifyKind(utc, DateTimeKind.Utc));
    jd = ClampJD(jd); // enforce 1900–2100
    double d = DateTimeProvider.DaysSinceJ2000(jd);
    transform.position = HeliocentricPositioner.ToUnity(elements, d);
  }

  bool TryParseDate(string iso, out DateTime dtUtc) {
    if (DateTime.TryParse(iso, out var dt)) { dtUtc = new DateTime(dt.Year,dt.Month,dt.Day,0,0,0,DateTimeKind.Utc); return true; }
    dtUtc = DateTime.UtcNow.Date; return false;
  }

  double ClampJD(double jd) {
    double lo = DateTimeProvider.ToJulianDay(new DateTime(1900,1,1,0,0,0,DateTimeKind.Utc));
    double hi = DateTimeProvider.ToJulianDay(new DateTime(2100,12,31,0,0,0,DateTimeKind.Utc));
    if (jd < lo) return lo; if (jd > hi) return hi; return jd;
  }
}
