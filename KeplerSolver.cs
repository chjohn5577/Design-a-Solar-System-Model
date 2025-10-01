using System;

public static class KeplerSolver {
  public static double SolveE(double M, double e) {
    // M, E in radians
    double E = M + e * Math.Sin(M) * (1 + e * Math.Cos(M));
    for (int k = 0; k < 8; k++) {
      double f = E - e * Math.Sin(E) - M;
      double fp = 1 - e * Math.Cos(E);
      E -= f / fp;
    }
    return E;
  }

  public static (double nu, double r_km) TrueAnomalyAndRadius(double E, double e, double a_km) {
    double r = a_km * (1 - e * Math.Cos(E));
    double nu = 2 * Math.Atan2(Math.Sqrt(1+e)*Math.Sin(E/2), Math.Sqrt(1-e)*Math.Cos(E/2));
    return (nu, r);
  }
}
