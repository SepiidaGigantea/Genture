using System.Diagnostics;

namespace VoronoiWF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "0 ms";
            /*
            int width = 1024;
            int height = 1024;
            int seed = 100;
            int degree = 0;
            Random random = new Random();
            Bitmap image = new Bitmap(width, height);
            

            List<int> seedsX = new List<int>();
            List<int> seedsY = new List<int>();
            for (int x = 0; x < seed; x++) seedsX.Add(random.Next(0, width));
            for (int y = 0; y < seed; y++) seedsY.Add(random.Next(0, height));

            //find max distance
            float maxDistance = 0;
            
            for(int y = 0; y<height; y++)
            {
                for(int x=0; x<width; x++)
                {
                    List<float> dists = new List<float>();
                    for (int i=0; i<seed; i++)
                    {
                        dists.Add(Hypot(seedsY[i]-y, seedsX[i]-x));

                    }
                    dists.Sort();
                    if (dists[degree] > maxDistance) maxDistance = dists[degree];
                }
            }
            maxDistance = (float)Math.Sqrt(maxDistance);
            //paint
            
            for (int y = 0; y < height; y++)
            {
                for( int x = 0; x < width; x++)
                {
                    List<float> dists = new List<float>();
                    for (int i = 0; i < seed; i++)
                    {
                        dists.Add(Hypot(seedsY[i] - y, seedsX[i] - x));

                    }
                    dists.Sort();
                    int c = (int)Math.Round(255 * Math.Sqrt( dists[degree]) / maxDistance);
                    image.SetPixel(x, y, Color.FromArgb(0, 0, c));
                }
            }
            pictureBox1.Image = image;
            

            */

        }
        public static float Hypot(float x, float y)
        {
            //return (float)Math.Sqrt((x * x) + (y * y));
            return x * x + y * y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int width = 1024;
            int height = 1024;
            int seed = 300;
            int degree = 0;
            Random random = new Random();
            Bitmap image = new Bitmap(width, height);


            int[] seedsX = new int[width];
            int[] seedsY = new int[height];
            for (int x = 0; x < seed; x++) seedsX[x] = (random.Next(0, width));
            for (int y = 0; y < seed; y++) seedsY[y] = (random.Next(0, height));

            //find max distance
            float maxDistance = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float[] dists = new float[seed];
                    for (int i = 0; i < seed; i++)
                    {
                        dists[i] = Hypot(seedsY[i] - y, seedsX[i] - x);
                    }
                    Array.Sort(dists);
                    if (dists[degree] > maxDistance) maxDistance = dists[degree];
                }
            }
            //paint
            maxDistance = (float)Math.Sqrt(maxDistance);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float[] dists = new float[seed];
                    for (int i = 0; i < seed; i++)
                    {
                        dists[i] = Hypot(seedsY[i] - y, seedsX[i] - x);

                    }
                    Array.Sort(dists);
                    int c = (int)Math.Round(255.0 * (float)Math.Sqrt(dists[degree]) / maxDistance);
                    image.SetPixel(x, y, Color.FromArgb(c, c, c));
                }
            }
            pictureBox1.Image = image;

            stopwatch.Stop();

            button1.Text = string.Format("{0}ms", stopwatch.ElapsedMilliseconds);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PerlinNoise pisos = new PerlinNoise();
            pisos.GenerateImage();
            pictureBox1.Image = pisos.GetImage();
        }
    }

}