using UnityEngine;
using System;

public static class HeliocentricPositioner {
  static Vector3 RotZ(Vector3 v, double deg) {
    double r = deg * Math.PI/180.0;
    double c = Math.Cos(r), s = Math.Sin(r);
    return new Vector3((float)(c*v.x - s*v.y), (float)(s*v.x + c*v.y), v.z);
  }
  static Vector3 RotX(Vector3 v, double deg) {
    double r = deg * Math.PI/180.0;
    double c = Math.Cos(r), s = Math.Sin(r);
    return new Vector3(v.x, (float)(c*v.y - s*v.z), (float)(s*v.y + c*v.z));
  }

  public static Vector3 ToUnity(OrbitalElements el, double daysSinceJ2000) {
    double M_deg = el.M0_deg + el.n_deg_per_day * daysSinceJ2000;
    double M = M_deg * Math.PI/180.0;
    double E = KeplerSolver.SolveE(M, el.e);
    var (nu, r_km) = KeplerSolver.TrueAnomalyAndRadius(E, el.e, el.a_km);

    // Perifocal coords (orbital plane, periapsis at +x)
    Vector3 p = new Vector3((float)(r_km * Math.Cos(nu)), (float)(r_km * Math.Sin(nu)), 0f);

    // Rotate to ecliptic (standard XY plane, +Z up): Rz(Ω) * Rx(i) * Rz(ω)
    Vector3 eclStd = RotZ(RotX(RotZ(p, el.omega_deg), el.i_deg), el.Omega_deg);

    // Map to our Unity ecliptic (XZ plane, +Y up): (x, y, z_std) -> (x, z_std, y)
    Vector3 unity = new Vector3(eclStd.x, eclStd.z, eclStd.y);

    // Scale: 1 unit = 1,000,000 km
    return unity / 1_000_000f;
  }
}
