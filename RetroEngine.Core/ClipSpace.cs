using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core
{
    public struct ClipSpace
    {
        public Matrix4 View { get; set; }

        public Matrix4 Projection { get; set; }
    }
}
