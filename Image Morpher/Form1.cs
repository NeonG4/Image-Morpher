using System.Collections.Generic;
using System.Numerics;
using System.IO;

namespace Image_Morpher
{
    public partial class FormMorpher : Form
    {
        int renderScale = 1;
        Vector2 offset = new Vector2(500, 500);
        int width = 0;
        int height = 0;
        int ticks;
        int targetTicks = 100;
        Color[,] colors;
        Vector2[,] targetPositions;
        Vector2[] acceleration;
        Vector2[] velocity;
        Vector2[] positions;
        Bitmap[] bitmaps;
        List<Bitmap> video;
        bool renderNewTick = true;

        float progress = 0.0f;
        public FormMorpher()
        {
            ticks = 0;
            InitializeComponent();

            bitmaps = [new Bitmap("images/image1.png"), new Bitmap("images/image2.png")];
            width = bitmaps[0].Width;
            height = bitmaps[0].Height;
            video = new List<Bitmap>();

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
                    velocity[i * width + j] = new Vector2(rnd.Next(-2, 2), rnd.Next(-2, 2));
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

                            velocity[i * width + j] = new Vector2(rnd.Next(-5, 5), rnd.Next(-5, 5));

                            acceleration[i * width + j] = 2 * (targetPositions[i * width + j, 1] - targetPositions[i * width + j, 0] - velocity[i * width + j] * ((float)targetTicks)) / (float)(targetTicks * targetTicks);

                        }
                    }
                }
            }
        }

        public void FormMorpher_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(2000, 2000);
            e.Graphics.Clear(Color.Black);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector2 pos = positions[i * width + j];
                    Color col = Color.FromArgb(
                        (int)(colors[i * width + j, (int)Math.Floor(progress)].R * (1 - (progress % 1)) + colors[i * width + j, 1 + (int)Math.Floor(progress)].R * (progress % 1)),
                        (int)(colors[i * width + j, (int)Math.Floor(progress)].G * (1 - (progress % 1)) + colors[i * width + j, 1 + (int)Math.Floor(progress)].G * (progress % 1)),
                        (int)(colors[i * width + j, (int)Math.Floor(progress)].B * (1 - (progress % 1)) + colors[i * width + j, 1 + (int)Math.Floor(progress)].B * (progress % 1))
                     );
                    if (ticks > 0)
                    {
                        bmp.SetPixel((int)(pos + offset).X, (int)(pos + offset).Y, col);
                    }
                    using (Brush brush = new SolidBrush(col))
                    {
                        e.Graphics.FillRectangle(brush, pos.X * renderScale + offset.X, pos.Y * renderScale + offset.Y, renderScale, renderScale);
                    }

                }
            }
            renderNewTick = false;
            if (ticks != 0)
            {
                video.Add(bmp);
            }
        }
        private void timerTick_Tick(object sender, EventArgs e)
        {
            if (!renderNewTick)
            {
                return;
            }
            progress = Math.Clamp((float)(ticks) / targetTicks, 0, 0.9999999f);

            float t = progress * targetTicks;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    positions[i * width + j] = targetPositions[i * width + j, (int)Math.Floor(progress)] + velocity[i * width + j] * t + .5f * t * t * acceleration[i * width + j];
                }
            }
            this.Refresh();
        }

        private void FormMorpher_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                renderNewTick = true;
                ticks++;
                if (ticks > targetTicks)
                {
                    ticks = 0;
                }
            }

        }

        private void FormMorpher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Directory.Exists("savedimages"))
            {
                Directory.CreateDirectory("savedimages");
            }
            //Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory() + "/savedimages"));
            for (int i = 0; i < video.Count; i++)
            {
                if (video[i] == null)
                {
                    video[i] = new Bitmap(1, 1);
                    video[i].SetPixel(0, 0, Color.Magenta);
                }
                //Directory.CreateDirectory($"img{i}.bmp");
                video[i].Save(Path.Combine("savedimages\\", $"img{i}.bmp"), System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }
    }
}
