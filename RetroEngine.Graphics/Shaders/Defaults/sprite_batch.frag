#version 330 core
            
in vec2 v_tex_coord;
in vec4 v_color;
            
uniform sampler2D u_texture0;
            
out vec4 out_color;

void main()
{
    if (texture(u_texture0, v_tex_coord).a < 0.1)
        discard;

    out_color = texture(u_texture0, v_tex_coord) * v_color;
}