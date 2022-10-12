using System.Numerics;

namespace VRCFTNeos.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        

        public static float AlphaF = 2f;
        public static float BetaF = 2f;

        public static double AlphaD = 2;
        public static double BetaD = 2;

        private static Vector3 Project2DTo3D(float x, float y)
        {
            var v = new Vector3((float) Math.Tan(AlphaF * x),
                                (float) Math.Tan(BetaF * y),
                                1f);
            Vector3.Normalize(v);
            return v;
        }

        private static Vector3 Project2DTo3D(double x, double y)
        {
            var v = new Vector3(Math.Tan(AlphaD * x),
                                Math.Tan(BetaD * y),
                                1);

            Vector3.Normalize(v);
            return v;
        }
    }
}