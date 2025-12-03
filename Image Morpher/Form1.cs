using System.Collections.Generic;
using System.Numerics;

namespace Image_Morpher
{
    public partial class FormMorpher : Form
    {
        int renderScale = 1;
        int width = 0;
        int height = 0;
        int ticks = 0;
        int targetTicks = 200;
        int waitingTicks = 100;
        Color[,] colors;
        Vector2[,] positions;
        Bitmap[] bitmaps;

        float progress = 0.0f;
        public FormMorpher()
        {
            InitializeComponent();

            bitmaps = [new Bitmap("images/christmastree.bmp"), new Bitmap("images/surfer.bmp")];
            width = bitmaps[0].Width;
            height = bitmaps[0].Height;

            colors = new Color[width * height, bitmaps.Length];
            positions = new Vector2[width * height, bitmaps.Length];

            List<Vector2> targets = new List<Vector2>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    targets.Add(new Vector2(i, j));
                }
            }
            for (int img = 0; img < bitmaps.Length; img++)
            {
                Random rnd = new Random();
                targets = targets.OrderBy(item => rnd.Next()).ToList();
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Color color = bitmaps[img].GetPixel((int)targets[i * width + j].X, (int)targets[i * width + j].Y);
                        colors[i * width + j, img] = color;
                        positions[i * width + j, img] = targets[i * width + j];
                    }
                }
            }
        }

        public void FormMorpher_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            for (int i = 0; i < width * height; i++)
            {
                Vector2 pos = Vector2.Lerp(positions[i, (int)Math.Floor(progress)], positions[i, (int)Math.Ceiling(progress)], (progress % 1));
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
            progress = Math.Clamp((float)(ticks - waitingTicks) / targetTicks, 0, 0.9999999f);
            this.Refresh();
        }
    }
}
