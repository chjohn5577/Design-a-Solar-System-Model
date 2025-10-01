using System;

public static class DateTimeProvider {
  // Convert a UTC DateTime to Julian Day
  public static double ToJulianDay(DateTime utc) {
    int Y = utc.Year, M = utc.Month;
    double D = utc.Day + (utc.Hour + (utc.Minute + utc.Second/60.0)/60.0)/24.0;
    if (M <= 2) { Y -= 1; M += 12; }
    int A = Y / 100;
    int B = 2 - A + A / 4;
    double JD = Math.Floor(365.25*(Y+4716)) + Math.Floor(30.6001*(M+1)) + D + B - 1524.5;
    return JD;
  }
  public static double DaysSinceJ2000(double jd) => jd - 2451545.0;
}
