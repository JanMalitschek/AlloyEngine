using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace Alloy.Assets
{
    public class Shader : Asset
    {
        public int Program { get; private set; }

        private string vertexCode = string.Empty;
        private string fragmentCode = string.Empty;

        private enum ShaderCodeTarget
        {
            Vertex,
            Geometry,
            Fragment
        }

        public Shader(string path) : base(path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                {
                    ShaderCodeTarget target = ShaderCodeTarget.Vertex;
                    while (reader.Peek() >= 0)
                    {
                        var line = reader.ReadLine();
                        if (line == "#vertex")
                        {
                            target = ShaderCodeTarget.Vertex;
                            continue;
                        }
                        else if(line == "#geometry")
                        {
                            target = ShaderCodeTarget.Geometry;
                            continue;
                        }
                        else if(line == "#fragment")
                        {
                            target = ShaderCodeTarget.Fragment;
                            continue;
                        }
                        switch (target)
                        {
                            case ShaderCodeTarget.Vertex:
                                vertexCode += line + "\n";
                                break;
                            case ShaderCodeTarget.Geometry:
                                
                                break;
                            case ShaderCodeTarget.Fragment:
                                fragmentCode += line + "\n";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Logging.LogError(this, e.Message);
                return;
            }
            Compile();
        }

        ~Shader()
        {
            GL.DeleteProgram(Program);
        }

        public void Compile()
        {
            int vert = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vert, vertexCode);
            int frag = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(frag, fragmentCode);

            GL.CompileShader(vert);
            string log = GL.GetShaderInfoLog(vert);
            if (log != string.Empty)
                Logging.LogError(this, log);

            GL.CompileShader(frag);
            log = GL.GetShaderInfoLog(frag);
            if (log != string.Empty)
                Logging.LogError(this, log);

            Program = GL.CreateProgram();
            GL.AttachShader(Program, vert);
            GL.AttachShader(Program, frag);
            GL.LinkProgram(Program);

            GL.DetachShader(Program, vert);
            GL.DetachShader(Program, frag);
            GL.DeleteShader(frag);
            GL.DeleteShader(vert);
        }

        public void Use()
        {
            GL.UseProgram(Program);
        }
    }
}
