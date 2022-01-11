using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    [Il2CppEagerStaticClassConstruction]
    public static class Navigation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dist(Vector3 delta) => delta.magnitude;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dist(Vector3 a, Vector3 b) => Dist(b - a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistSq(Vector3 delta) => delta.sqrMagnitude;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistSq(Vector3 a, Vector3 b) => DistSq(b - a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Manhattan(Vector3 delta) => Mathf.Abs(delta.x) + Mathf.Abs(delta.y) + Mathf.Abs(delta.z);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Manhattan(Vector3 a, Vector3 b) => Manhattan(b - a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Chebyshev(Vector3 delta) => Mathf.Max(Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y)), Mathf.Abs(delta.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Chebyshev(Vector3 a, Vector3 b) => Chebyshev(b - a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Azimuth(Transform t, Vector3 p) => SignedAngleOnAxis(t.forward, p - t.position, t.up);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Altitude(Transform t, Vector3 p) => SignedAngleOnAxis(t.forward, p - t.position, -t.right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AzimuthDir(Transform t, Vector3 p) => Vector3.Dot(p - t.position, t.right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AltitudeDir(Transform t, Vector3 p) => Vector3.Dot(p - t.position, t.up);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AzimuthSign(Transform t, Vector3 p) => Sign(AzimuthDir(t, p));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AltitudeSign(Transform t, Vector3 p) => Sign(AltitudeDir(t, p));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleOnAxis(Vector3 from, Vector3 to, Vector3 axis) => Mathf.Abs(SignedAngleOnAxis(from, to, axis));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignedAngleOnAxis(Vector3 from, Vector3 to, Vector3 axis) => Vector3.SignedAngle(Vector3.Cross(axis, from), Vector3.Cross(axis, to), axis);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(float v) => v == 0 ? 0 : v > 0 ? 1 : -1;
    }
}