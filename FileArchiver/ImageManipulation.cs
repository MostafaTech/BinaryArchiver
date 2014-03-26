using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace MTech.FileArchiver
{
    public static class ImageManipulation
    {

        public static Point RotateXY(this Point source, double degrees, int offsetX, int offsetY)
        {
            Point result = new Point();
            result.X = (int)(Math.Round((source.X - offsetX) *
                       Math.Cos(degrees) - (source.Y - offsetY) *
                       Math.Sin(degrees))) + offsetX;
            result.Y = (int)(Math.Round((source.X - offsetX) *
                       Math.Sin(degrees) + (source.Y - offsetY) *
                       Math.Cos(degrees))) + offsetY;
            return result;
        }

        public static Image ImageTranslate(Image image, int x, int y, int width, int height, float angle)
        {
            var bitmap = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.TranslateTransform(bitmap.Width / 2.0f, bitmap.Height / 2.0f);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-bitmap.Width / 2.0f, -bitmap.Height / 2.0f);
                graphics.DrawImage(image, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            }
            image.Dispose();
            return (Image)bitmap;
        }

        public static ImageAttributes GetTransparencyAttributes(float value)
        {
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            float[][] colorMatrixElements = { 
                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                   new float[] {0.0f,  0.0f,  0.0f,  value, 0.0f},
                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                };
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAttributes;
        }

    }
}
