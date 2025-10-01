using UnityEngine;
using System;

public class EarthOrientation : MonoBehaviour
{
    [Header("Scene References")]
    public Transform earth; // parent that moves along the orbit
    public Transform sun;   // Sun transform (at world origin)

    [Header("Behavior")]
    public bool yawOnly = true;
    [Tooltip("If your texture’s 0° longitude points along +Z when rotation=0, leave 0. Adjust to align your prime meridian to noon.")]
    [Range(-180f, 180f)] public float textureLongitudeAtForwardDeg = 0f;

    void LateUpdate()
    {
        if (earth == null) earth = transform.parent; // convenience
        if (earth == null) return;

        // Sun direction from Earth
        Vector3 sunDirWorld = (sun != null ? (sun.position - earth.position) : (-earth.position));
        if (sunDirWorld.sqrMagnitude < 1e-8f) return;
        sunDirWorld.Normalize();

        // Face the Sun; optionally remove pitch so we only yaw (keep planet upright)
        Vector3 faceDir = yawOnly ? Vector3.ProjectOnPlane(sunDirWorld, Vector3.up).normalized
                                  : sunDirWorld;

        if (faceDir.sqrMagnitude < 1e-8f) return;

        Quaternion look = Quaternion.LookRotation(faceDir, Vector3.up);
        // Offset so your texture’s prime meridian is exactly at local noon if you care
        transform.rotation = look * Quaternion.Euler(0f, -textureLongitudeAtForwardDeg, 0f);
    }
}
