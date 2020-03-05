using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Alloy.Assets
{
    public class Texture : Asset
    {
        public int Handle { get; protected set; }

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

        public Texture(string path, bool generate = true) : base(path)
        {
            if(generate)
                Handle = GL.GenTexture();
        }

        public void SetFilter(Filter filterMode, bool generateMipMaps = true)
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            filter = filterMode;
            int minFilter;
            if (generateMipMaps)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                if (filter == Filter.Linear)
                    minFilter = (int)TextureMinFilter.LinearMipmapLinear;
                else
                    minFilter = (int)TextureMinFilter.NearestMipmapLinear;
            }
            else
            {
                if (filter == Filter.Linear)
                    minFilter = (int)TextureMinFilter.Linear;
                else
                    minFilter = (int)TextureMinFilter.Nearest;
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                            (int)(filter == Filter.Linear ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void SetWrapping(Wrapping wrapMode)
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            wrapping = wrapMode;
            int wrap;
            if (wrapping == Wrapping.Repeat)
                wrap = (int)TextureWrapMode.Repeat;
            else if (wrapping == Wrapping.Clamp)
                wrap = (int)TextureWrapMode.Clamp;
            else if (wrapping == Wrapping.Mirror)
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
