using System.Collections.Generic;
using System.Numerics;

namespace Image_Morpher
{
    public partial class FormMorpher : Form
    {
        int renderScale = 16;
        int width = 0;
        int height = 0;
        int ticks = 0;
        int targetTicks = 100;
        Color[,] colors;
        Vector2[,] targetPositions;
        Vector2[] acceleration;
        Vector2[] velocity;
        Vector2[] positions;
        Bitmap[] bitmaps;

        float progress = 0.0f;
        public FormMorpher()
        {
            InitializeComponent();

            bitmaps = [new Bitmap("images/christmastree.bmp"), new Bitmap("images/surfer.bmp")];
            width = bitmaps[0].Width;
            height = bitmaps[0].Height;

            acceleration = new Vector2[width * height];
            velocity = new Vector2[width * height];
            colors = new Color[width * height, bitmaps.Length];
            targetPositions = new Vector2[width * height, bitmaps.Length];
            positions = new Vector2[width * height];

            List<Vector2> targets = new List<Vector2>();
            Random rnd = new Random();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    targets.Add(new Vector2(i, j));
                    velocity[i * width + j] = new Vector2(rnd.Next(-5, 5), rnd.Next(-5, 5));
                }
            }
            for (int img = 0; img < bitmaps.Length; img++)
            {
                targets = targets.OrderBy(item => rnd.Next()).ToList();
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Color color = bitmaps[img].GetPixel((int)targets[i * width + j].X, (int)targets[i * width + j].Y);
                        colors[i * width + j, img] = color;
                        targetPositions[i * width + j, img] = targets[i * width + j];
                        if (img == 1)
                        {
                            positions[i * width + j] = targets[i * width + j];

                            velocity[i * width + j] = new Vector2(rnd.Next(-1, 1), rnd.Next(-1, 1));

                            acceleration[i * width + j] = 2 * (targetPositions[i * width + j, 1] - targetPositions[i * width + j, 0] - velocity[i * width + j] * ((float)targetTicks)) / (float)(targetTicks * targetTicks);
                            
                        }
                    }
                }
            }
        }

        public void FormMorpher_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            for (int i = 0; i < width * height; i++)
            {
                Vector2 pos = positions[i];
                Color col = Color.FromArgb(
                    (int)(colors[i, (int)Math.Floor(progress)].R * (1 - (progress % 1)) + colors[i, 1 + (int)Math.Floor(progress)].R * (progress % 1)),
                    (int)(colors[i, (int)Math.Floor(progress)].G * (1 - (progress % 1)) + colors[i, 1 + (int)Math.Floor(progress)].G * (progress % 1)),
                    (int)(colors[i, (int)Math.Floor(progress)].B * (1 - (progress % 1)) + colors[i, 1 + (int)Math.Floor(progress)].B * (progress % 1))
                );
                using (Brush brush = new SolidBrush(col))
                {
                    e.Graphics.FillRectangle(brush, pos.X * renderScale, pos.Y * renderScale, renderScale, renderScale);
                }
            }
        }
        private void timerTick_Tick(object sender, EventArgs e)
        {
            ticks++;
            progress = Math.Clamp((float)(ticks) / targetTicks, 0, 0.9999999f);
            float t =  progress * targetTicks;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    positions[i * width + j] = targetPositions[i * width + j, (int)Math.Floor(progress)] + velocity[i * width + j] * t + .5f * t * t * acceleration[i * width + j];
                    
                    
                    //positions[i * width + j] = (progress * targetPositions[i * width + j, (int)Math.Ceiling(progress)]) + ((1 - progress) * targetPositions[i * width + j, (int)Math.Floor(progress)]);
                }
            }
            this.Refresh();
        }
    }
}
