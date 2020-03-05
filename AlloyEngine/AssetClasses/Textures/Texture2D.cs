using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Alloy.Assets
{
    public class Texture2D : Texture
    {
        private Image<Rgba32> pixels;

        public Texture2D(string path, Filter filterMode = Filter.Linear, Wrapping wrapMode = Wrapping.Repeat, bool generateMipMaps = true) : base(path)
        {
            pixels = (Image<Rgba32>)Image.Load(path);

            var tempPixels = pixels.GetPixelSpan();
            List<byte> bytePixels = new List<byte>();
            foreach (Rgba32 c in tempPixels)
            {
                bytePixels.Add(c.R);
                bytePixels.Add(c.G);
                bytePixels.Add(c.B);
                bytePixels.Add(c.A);
            }
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, pixels.Width, pixels.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bytePixels.ToArray());

            var metaData = LoadMetaData();

            SetFilter(metaData.Count >= 1 ? (Filter)Convert.ToInt32(metaData[0].value) : filterMode, generateMipMaps);

            SetWrapping(metaData.Count >= 2 ? (Wrapping)Convert.ToInt32(metaData[1].value) : wrapMode);
        }
    }
}
