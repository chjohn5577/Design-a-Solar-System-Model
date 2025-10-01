using UnityEngine;
using System;

public class MoonController : MonoBehaviour
{
    [Header("Scene References")]
    public Transform earth;
    public Transform sun;                      // optional, for phase alignment
    public PlanetController earthPlanet;       // optional, to read dateIso

    [Header("Orbit (Unity units)")]
    [Tooltip("Mean distance Earth↔Moon in uu; 1uu = 1e6 km. 0.3844 uu ≈ 384,400 km")]
    public float radiusUU = 0.3844f;
    [Tooltip("Sidereal period (days)")]
    public float periodDays = 27.321661f;
    [Tooltip("Starting phase angle (deg) at J2000)")]
    public float theta0Deg = 0f;
    [Tooltip("Tilt Moon's orbit out of XZ plane (deg). Keep 0 for now.")]
    public float inclinationDeg = 0f;

    [Header("Visuals")]
    public bool rotateLitSideTowardSun = true;

    void Reset()
    {
        // Handy auto-assign when you add the component
        if (earth == null && transform.parent != null) earth = transform.parent;
        if (earthPlanet == null && earth != null) earthPlanet = earth.GetComponent<PlanetController>();
    }

    void LateUpdate()
    {
        if (earth == null) return;

        double dDays = GetDaysSinceJ2000FromEarthDate();
        float theta = Mathf.Deg2Rad * (theta0Deg + (float)(360.0 * (dDays % periodDays) / periodDays));

        // Base circular orbit in Earth's XZ plane
        Vector3 rel = new Vector3(Mathf.Cos(theta), 0f, Mathf.Sin(theta)) * radiusUU;

        // Optional small tilt out of the plane
        if (Mathf.Abs(inclinationDeg) > 0.001f)
        {
            Quaternion tilt = Quaternion.AngleAxis(inclinationDeg, Vector3.right);
            rel = tilt * rel;
        }

        transform.position = earth.position + rel;

        // Orient so the bright side faces the Sun (becomes obvious once you add lighting)
        if (rotateLitSideTowardSun)
        {
            Vector3 sunDir = (sun != null) ? (sun.position - transform.position) : (Vector3.zero - transform.position);
            if (sunDir.sqrMagnitude > 1e-8f)
            {
                sunDir.Normalize();
                // Forward points away from Sun so the lit hemisphere faces the Sun
                transform.rotation = Quaternion.LookRotation(-sunDir, Vector3.up);
            }
        }
    }

    double GetDaysSinceJ2000FromEarthDate()
    {
        if (earthPlanet != null && !string.IsNullOrEmpty(earthPlanet.dateIso))
        {
            if (DateTime.TryParse(earthPlanet.dateIso, out var dt))
            {
                var utc = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, DateTimeKind.Utc);
                double jd = DateTimeProvider.ToJulianDay(utc);
                return DateTimeProvider.DaysSinceJ2000(jd);
            }
        }
        // Fallback animation if no date available: 10 seconds = 1 day
        return Time.time / 10.0;
    }
}
