using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiWF
{
    class PerlinNoise
    {
        private static int size = 1024;
        private static int res = 40;
        private int degree = 0;
        private int spacerange = size / res;
        private int dim = 3;
        private int octaves = 4;
        private Random random = new Random();
        private static Bitmap image = new Bitmap(size, size);
        int z;

        Random rand = new System.Random();
        byte[] permutationTable = new byte[1024];
        

        public PerlinNoise()
        {
            hashdata();
            z = random.Next(256);
            //z = 0;
            rand.NextBytes(permutationTable);
            
        }

        public void GenerateImage()
        {
            int freq = 32;
            float fx = size / freq;
            float fy = size / freq;
            System.Console.WriteLine($"{fx}, {fy}");
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float n = OctavePerlin(x / fx, y / fy, 2, 0.5f);
                    //float n = perlin(x, y, 0);
                    System.Console.WriteLine($"gen{n}");
                    int c = (int)Math.Round(255.0 * n + 0.5f) % 255;
                    c = Math.Abs(c);
                    image.SetPixel(x, y, Color.FromArgb(c, c, c));
                }
            }
        }

        public Bitmap GetImage()
        {
            return image;
        }

        public float OctavePerlin(float x, float y, int octaves, float persistence)
        {
            float total = 0.0f;
            float amplitude = 1.0f;
            float frequency = 1.0f;
            float maxValue = 0.0f;
            for (int i = 0; i < octaves; i++)
            {
                float per = perlin(x * frequency, y * frequency, z) * amplitude;
                total += per;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= 2;
            }
            //System.Console.WriteLine($"oct{total}, {maxValue}");
            return total / maxValue;
        }

        private int[] permutation = { 151,160,137,91,90,15,					// Hash lookup table as defined by Ken Perlin.  This is a randomly
		131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,	// arranged array of all numbers from 0-255 inclusive.
		190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
    };

        private int[] p;

        public void hashdata()
        {
            p = new int[512];
            for (int x = 0; x < 256; x++)
            {
                p[x] = permutation[x];
            }

            for (int i = 0; i < 256; i++)
                p[256 + i] = p[i] = permutation[i];
            /*
            for(int x = 0; x < size; x++)
            {
                p[x] = permutation[x % 256];
            }
            */
        }

        public float perlin(float x, float y, float z)
        {
            /*
            int xi = (int)(Math.Floor(x)) & 255;
            int yi = (int)(Math.Floor(y)) & 255;

            x -= (int)(x);
            y -= (int)(y);
            //x = x - xi;
            //y = y - yi;


            float sx = fade(x);
            float sy = fade(y);


            int aa, ab, ba, bb;
            aa = p[p[xi] + yi];
            ab = p[p[xi] + yi + 1];
            ba = p[p[xi + 1] + yi];
            bb = p[p[xi + 1] + yi + 1];
            */

            int xi = (int)(Math.Floor(x)) & 255;
            int yi = (int)(Math.Floor(y)) & 255;
            x = x - xi;
            y = y - yi;
            float[] topleftgrad = GetPseudoRandomGradientVector(xi, yi);
            float[] toprightgrad = GetPseudoRandomGradientVector(xi+1, yi);
            float[] bottomLeftGrad = GetPseudoRandomGradientVector(xi, yi+1);
            float[] bottomRightGrad = GetPseudoRandomGradientVector(xi+1, yi+1);

            float[] distanceToTopLeft = new float[] { x, y };
            float[] distanceToTopRight = new float[] { x - 1, y };
            float[] distanceToBottomLeft = new float[] { x, y - 1 };
            float[] distanceToBottomRight = new float[] { x - 1, y - 1 };

            float tx1 = Dot(distanceToTopLeft, topleftgrad);
            float tx2 = Dot(distanceToTopRight, toprightgrad);
            float bx1 = Dot(distanceToBottomLeft, bottomLeftGrad);
            float bx2 = Dot(distanceToBottomRight, bottomRightGrad);

            x = fade1(x);
            y = fade1(y);

            float tx = lerp(tx1, tx2, x);
            float bx = lerp(bx1, bx2, x);
            float tb = lerp(tx, bx, y);

            return tb;





            /*
            float avg = lerp(lerp(grad1(aa, x, y, z), grad1(ab, x - 1, y, z), sx),
                                 lerp(grad1(ba, x, y - 1, 0), grad1(bb, x - 1, y - 1, z), sy), sy);
            //return avg;
            float avg2 = (avg + 1.0f) / 2.0f;
            avg2 = lerp(0.0f, 1.0f, avg2);
            //System.Console.WriteLine($"per {avg}, {avg2}");
            return avg2;
            */
        }

        public float grad(int hash, float x, float y, float z)
        {
            switch (hash & 0xF)
            {
                case 0x0: return x + y;
                case 0x1: return -x + y;
                case 0x2: return x - y;
                case 0x3: return -x - y;
                case 0x4: return x + z;
                case 0x5: return -x + z;
                case 0x6: return x - z;
                case 0x7: return -x - z;
                case 0x8: return y + z;
                case 0x9: return -y + z;
                case 0xA: return y - z;
                case 0xB: return -y - z;
                case 0xC: return y + x;
                case 0xD: return -y + z;
                case 0xE: return y - x;
                case 0xF: return -y - z;
                default: return 0; // never happens
            }
        }

        public float fade(float t)
        {
            return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);         // 6t^5 - 15t^4 + 10t^3
        }
        public float fade1(float t)
        {
            return (float)((1-Math.Cos(t * Math.PI))/2);
        }

        public float lerp(float a, float b, float x)
        {
            return a + x * (b - a);
        }

        float Dot(float[]a, float[]b)
        {
            return a[0] * b[0] + a[1] * b[1];
        }
        
        
        public static float grad1(int hash, float x, float y, float z)
        {
            int h = hash & 15;                                  // Take the hashed value and take the first 4 bits of it (15 == 0b1111)
            float u = h < 8 /* 0b1000 */ ? x : y;              // If the most significant bit (MSB) of the hash is 0 then set u = x.  Otherwise y.

            float v;                                           // In Ken Perlin's original implementation this was another conditional operator (?:).  I
                                                               // expanded it for readability.

            if (h < 4 /* 0b0100 */)                             // If the first and second significant bits are 0 set v = y
                v = y;
            else if (h == 12 /* 0b1100 */ || h == 14 /* 0b1110*/)// If the first and second significant bits are 1 set v = x
                v = x;
            else                                                // If the first and second significant bits are not equal (0/1, 1/0) set v = z
                v = z;

            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        private float[] GetPseudoRandomGradientVector(int x, int y)
        {
            int v = (int)(((x * 1836311903) ^ (y * 2971215073) + 4807526976) & 1023);
            v = permutationTable[v] & 3;

            switch (v)
            {
                case 0: return new float[] { 1, 0 };
                case 1: return new float[] { -1, 0 };
                case 2: return new float[] { 0, 1 };
                default: return new float[] { 0, -1 };
            }
        }
    }
    
    
}
