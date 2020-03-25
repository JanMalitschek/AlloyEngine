using System;
using System.Collections.Generic;
using System.Text;
using Alloy.Assets;

namespace Alloy
{
    #region Calls
    public abstract class ExternalCall
    {
        public abstract void Execute();
    }

    public class TextureFilterCall : ExternalCall
    {
        private int id;
        private Texture.Filter filter;
        public TextureFilterCall(int id, Texture.Filter filter)
        {
            this.id = id;
            this.filter = filter;
        }

        public override void Execute()
        {
            var tex = AssetDatabase.GetAsset<Texture2D>(id);
            tex.SetFilter(filter);
            tex.Apply();
        }
    }
    public class TextureWrappingCall : ExternalCall
    {
        private int id;
        private Texture.Wrapping wrapping;
        public TextureWrappingCall(int id, Texture.Wrapping wrapping)
        {
            this.id = id;
            this.wrapping = wrapping;
        }

        public override void Execute()
        {
            var tex = AssetDatabase.GetAsset<Texture2D>(id);
            tex.SetWrapping(wrapping);
            tex.Apply();
        }
    }
    #endregion

    public static class ExternalCallQueue
    {
        private static List<ExternalCall> externalCalls = new List<ExternalCall>();
        public static void AddCall(ExternalCall call)
        {
            externalCalls.Add(call);
        }
        public static void Run()
        {
            if(externalCalls.Count > 0)
            {
                foreach (ExternalCall c in externalCalls)
                    c.Execute();
                externalCalls.Clear();
            }
        }
    }
}
