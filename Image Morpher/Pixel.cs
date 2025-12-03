using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Image_Morpher
{
    public class Pixel
    {
        
        public Vector2 lastPosition;
        public Vector2 currentPosition;
        public Vector2 targetPosition;
        public Color color;
        public Pixel(Vector2 position, Color color)
        {
            this.currentPosition = position;
            this.color = color;
        }
        public void Update(float progress)
        {
            currentPosition = Vector2.Lerp(lastPosition, targetPosition, progress);
        }
        public void Draw(ref PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new System.Drawing.SolidBrush(color), currentPosition.X, currentPosition.Y, 1, 1);
        }
    }
}
