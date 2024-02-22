using OpenTK.Mathematics;
using RetroEngine.Graphics.Batching;
using RetroEngine.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Ecs.Components
{
    public struct SpriteRenderer : IComponent
    {
        public int TextureId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Color4 Color { get; set; }

        public Flip Flip { get; set; }

        public int LayerDepth { get; set; }

        public SpriteRenderer()
        {
            TextureId = 0;
            Width = 0;
            Height = 0;
            Color = Color4.White;
            Flip = Flip.None;
            LayerDepth = 0;
        }
    }

    public enum Flip
    {
        None = 0,
        X = 1,
        Y = 2,
        Both = 3
    }
}
