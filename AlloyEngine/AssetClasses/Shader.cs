using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK;

namespace Alloy.Assets
{
    public class Shader : Asset
    {
        public int Program { get; private set; }

        public struct Uniform
        {
            public struct Invalid { }
            public Type type;
            public string name;
        }
        public List<Uniform> uniforms { get; private set; } = new List<Uniform>();
        public int uniformCount
        {
            get
            {
                return uniforms.Count;
            }
        }

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
                        //Find Shader Uniforms
                        var splitLine = line.Split(' ');
                        if(splitLine.Length > 0)
                        {
                            if(splitLine.Contains("uniform"))
                            {
                                string name = splitLine.Last().Remove(splitLine.Last().Length - 1);
                                Type type = typeof(Uniform.Invalid);
                                if (splitLine.Contains("float"))
                                    type = typeof(float);
                                else if (splitLine.Contains("int"))
                                    type = typeof(int);
                                else if (splitLine.Contains("vec2"))
                                    type = typeof(Vector2);
                                else if (splitLine.Contains("vec3"))
                                    type = typeof(Vector3);
                                else if (splitLine.Contains("vec4"))
                                    type = typeof(Vector4);
                                else if (splitLine.Contains("mat4"))
                                    type = typeof(Matrix4);
                                else if (splitLine.Contains("sampler2D"))
                                    type = typeof(Texture);
                                uniforms.Add(new Uniform { name = name, type = type });
                            }
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

        public Uniform GetUniform(int idx)
        {
            if (idx >= 0 && idx < uniforms.Count)
                return uniforms[idx];
            return new Uniform { name = "Invalid Index!", type = typeof(Uniform.Invalid) };
        }
        public Uniform GetUniform(string name)
        {
            foreach (Uniform u in uniforms)
                if (u.name == name)
                    return u;
            return new Uniform { name = "Invalid Name!", type = typeof(Uniform.Invalid) };
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(Program, name);
        }

        protected override void SaveMetaData(out List<MetaDataEntry> metaData)
        {
            metaData = new List<MetaDataEntry>();
        }
    }
}
