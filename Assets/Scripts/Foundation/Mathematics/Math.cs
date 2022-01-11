using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace DarkFrontier.Foundation.Mathematics {
    [Il2CppEagerStaticClassConstruction]
    public static class Math {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Min(sbyte a, sbyte b) => a < b ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Min(byte a, byte b) => a < b ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Min(short a, short b) => a < b ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Min(ushort a, ushort b) => a < b ? a : b;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int a, int b) => a < b ? a : b;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Min(uint a, uint b) => a < b ? a : b;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Min(long a, long b) => a < b ? a : b;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Min(ulong a, ulong b) => a < b ? a : b;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float a, float b) => a < b ? a : b;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double a, double b) => a < b ? a : b;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Min(decimal a, decimal b) => a < b ? a : b;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Min(sbyte a, sbyte b, sbyte c) => a < b ? a < c ? a : c : b < c ? b : c;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Min(byte a, byte b, byte c) => a < b ? a < c ? a : c : b < c ? b : c;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Min(short a, short b, short c) => a < b ? a < c ? a : c : b < c ? b : c;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Min(ushort a, ushort b, ushort c) => a < b ? a < c ? a : c : b < c ? b : c;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int a, int b, int c) => a < b ? a < c ? a : c : b < c ? b : c;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Min(uint a, uint b, uint c) => a < b ? a < c ? a : c : b < c ? b : c;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Min(long a, long b, long c) => a < b ? a < c ? a : c : b < c ? b : c;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Min(ulong a, ulong b, ulong c) => a < b ? a < c ? a : c : b < c ? b : c;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float a, float b, float c) => a < b ? a < c ? a : c : b < c ? b : c;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double a, double b, double c) => a < b ? a < c ? a : c : b < c ? b : c;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Min(decimal a, decimal b, decimal c) => a < b ? a < c ? a : c : b < c ? b : c;
    }
}