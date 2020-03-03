using System;
using System.Collections.Generic;
using System.Text;
using Alloy.Assets;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Alloy.Components
{
    public class ModelRenderer : Renderer
    {
        public Model model;
        private int meshIdx = 0;
        public int MeshIndex
        {
            set
            {
                meshIdx = MathHelper.Clamp(value, 0, model.meshes.Count);
            }
        }
        public Material[] materials;

        public ModelRenderer()
        {
            model = null;
            materials = new Material[0];
        }
        public ModelRenderer(Model model = null)
        {
            SetModel(model);
        }

        public void SetModel(Model model)
        {
            
            this.model = model;
            if (this.model == null)
                return;
            materials = new Material[model.meshes[0].subMeshes.Count];
        }

        public void SetMaterial(Material mat, int subMeshIdx)
        {
            if (subMeshIdx >= 0 && subMeshIdx < materials.Length)
                materials[subMeshIdx] = mat;
        }

        public override void Render()
        {
            if (model == null) return;
            for(int i = 0; i < model.meshes[meshIdx].subMeshes.Count; i++)
            {
                if(i < materials.Length)
                {
                    RenderPipeline.PassPredefinedUniforms(materials[i].PredefinedUniforms, this, materials[i].shader);
                    materials[i].Pass();
                    model.meshes[meshIdx].subMeshes[i].vao.Bind();
                    GL.DrawElements(BeginMode.Triangles, model.meshes[meshIdx].subMeshes[i].NumIndices, DrawElementsType.UnsignedInt, 0);
                }
            }
        }

    }
}
