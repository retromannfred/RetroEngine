#version 330 core
            
in vec2 pass_texCoord;
in vec4 pass_color;
            
uniform sampler2D texture0;
            
out vec4 out_color;

void main()
{
    out_color = texture(texture0, pass_texCoord) * pass_color;
}