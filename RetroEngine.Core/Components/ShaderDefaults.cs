using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core.Components
{
    internal class ShaderDefaults
    {
        public const string DEFAULT_VERTEX_SHADER = @"
            #version 330 core
            
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            layout (location = 2) in vec3 aColor;
            
            out vec2 texCoord;
            out vec4 color;

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;
            
            void main()
            {
                texCoord = aTexCoord;
                color = vec4(aColor.rgb, 1.0);
                gl_Position = vec4(aPosition.xyz, 1.0) * model * view * projection;
            }";

        public const string DEFAULT_FRAGMENT_SHADER = @"
            #version 330 core
            out vec4 outColor;
            in vec2 texCoord;
            in vec4 color;
            uniform sampler2D texture0;
            
            void main()
            {
                outColor = texture(texture0, texCoord) * color;
            }";
    }
}
