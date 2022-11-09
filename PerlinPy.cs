using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiWF
{
    public class PerlinPy
    {
        /*
        public static double Smoothstep(double t)
        {
            return t * t * (3 - 2 * t);
        }

        public static double Lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }
        int dimension;
        int octaves;
        double tile;
        bool unbias;
        double scaleFactor;
        double gradient;
        double[] randomPoint;

        public PerlinPy(int dimension, double tile, int octaves = 1, bool unbias= false)
        {
            this.dimension = dimension;
            this.octaves = octaves;
            this.tile = tile;
            this.unbias = unbias;

        }
        public double GenerateGradient(PerlinPy perlin)
        {
            if (perlin.dimension == 1)
            {
                return (new Random().Next(-1, 1));
            }
            else return 0; //?
            for(int i = 0; i < dimension; i++)
            {
                double 
            }

        } */

        //256, 256, 3, 3
        int outputSize = 256;
        float[] noiseSeed1D;
        float[] perlinNoise1D;
        
        public void Init()
        {
            Random rand = new Random();
            noiseSeed1D = new float[outputSize];
            perlinNoise1D = new float[outputSize];
            for (int i = 0; i < outputSize; i++) noiseSeed1D[i] = (float)rand.NextDouble();
        }
        void PerlinNoise1D(int nCount, float[] fSeed, int nOctaves, float[] fOutput)
        {
            for (int x = 0; x<nCount; x++)
            {
                float fNoise = 0.0f;
                float fScale = 1.0f;
                float fScaleAcc = 0.0f;
                for(int o =0; o<nOctaves; o++)
                {
                    int nPitch = nCount >> o; // int nPitch = nCount / Math.Pow(2, o);
                    int nSample1 = (x / nPitch) * nPitch;
                    int nSample2 = (nSample1 + nPitch) % nCount;
                    float fBlend = (float)(x - nSample1) / (float)nPitch;
                    float fSample = (1.0f - fBlend) * fSeed[nSample1] + fBlend * fSeed[nSample2];
                    fNoise += fSample * fScale;
                    fScaleAcc += fScale;
                    fScale = fScale / 2.0f;
                }
                fOutput[x] = fNoise/fScaleAcc;
            }
        }
            

    }
}
