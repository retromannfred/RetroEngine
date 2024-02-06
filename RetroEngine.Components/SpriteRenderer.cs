using OpenTK.Mathematics;
using RetroEngine.Core.Batching;
using RetroEngine.ECS.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Components
{
    public struct SpriteRenderer : IComponent
    {
        public int SpriteId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Color4 Color { get; set; }

        public Flip Flip { get; set; }

        public int LayerDepth { get; set; }

        public SpriteRenderer()
        {
            SpriteId = 0;
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
