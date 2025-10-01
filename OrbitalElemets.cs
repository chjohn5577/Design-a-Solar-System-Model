using UnityEngine;

[System.Serializable]
public class OrbitalElements {
  // Distances in km; angles in degrees; rates in deg/day
  public double a_km;      // semi-major axis
  public double e;         // eccentricity
  public double i_deg;     // inclination
  public double Omega_deg; // longitude of ascending node
  public double omega_deg; // argument of perihelion
  public double M0_deg;    // mean anomaly at J2000
  public double n_deg_per_day; // mean motion
}
