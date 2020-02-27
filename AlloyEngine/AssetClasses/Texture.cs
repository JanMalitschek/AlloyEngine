using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Alloy.Assets
{
    class Texture : Asset
    {
        public int Handle { get; private set; }
        private Image<Rgba32> pixels;

        public enum Filter
        {
            Linear,
            Nearest
        }
        public Filter filter = Filter.Linear;
        public enum Wrapping
        {
            Repeat,
            Clamp,
            Mirror
        }
        public Wrapping wrapping = Wrapping.Repeat;

        public Texture (string path, Filter filterMode = Filter.Linear, Wrapping wrapMode = Wrapping.Repeat, bool generateMipMaps = true) : base(path)
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
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, pixels.Width, pixels.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bytePixels.ToArray());

            int minFilter;
            if (generateMipMaps)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                if (filterMode == Filter.Linear)
                    minFilter = (int)TextureMinFilter.LinearMipmapLinear;
                else
                    minFilter = (int)TextureMinFilter.NearestMipmapLinear;
            }
            else
            {
                if (filterMode == Filter.Linear)
                    minFilter = (int)TextureMinFilter.Linear;
                else
                    minFilter = (int)TextureMinFilter.Nearest;
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 
                            (int)(filterMode == Filter.Linear ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
            int wrap;
            if (wrapMode == Wrapping.Repeat)
                wrap = (int)TextureWrapMode.Repeat;
            else if (wrapMode == Wrapping.Clamp) 
                wrap = (int)TextureWrapMode.Clamp;
            else if (wrapMode == Wrapping.Mirror)
                wrap = (int)TextureWrapMode.MirroredRepeat;
            else
                wrap = (int)TextureWrapMode.Repeat;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, wrap);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Bind(int texIdx)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texIdx);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        protected override void SaveMetaData(out List<MetaDataEntry> metaData)
        {
            metaData = new List<MetaDataEntry>();
            metaData.Add(new MetaDataEntry("Filter", (int)filter));
            metaData.Add(new MetaDataEntry("Wrapping", (int)wrapping));
        }
    }
}
