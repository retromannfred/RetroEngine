using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Graphics.Shaders
{
    /// <summary>
    /// Defines default shader behaviours to render sprites.
    /// </summary>
    internal class ShaderDefaults
    {
        /// <summary>
        /// Gets the default shader to transform a sprite.
        /// </summary>
        public const string DEFAULT_VERTEX_SHADER = @"
            #version 330 core
            
            layout (location = 0) in vec3 in_position;
            layout (location = 1) in vec2 in_texCoord;
            layout (location = 2) in vec4 in_color;

            uniform mat4 mvp;
            
            out vec2 pass_texCoord;
            out vec4 pass_color;
            
            void main()
            {
                pass_texCoord = in_texCoord;
                pass_color = in_color;
                gl_Position = vec4(in_position.xyz, 1.0) * mvp;
            }";

        /// <summary>
        /// Gets the default shader to color a sprite.
        /// </summary>
        public const string DEFAULT_FRAGMENT_SHADER = @"
            #version 330 core
            
            in vec2 pass_texCoord;
            in vec4 pass_color;
            
            uniform sampler2D texture0;
            
            out vec4 out_color;

            void main()
            {
                out_color = texture(texture0, pass_texCoord) * pass_color;
            }";
    }
}
