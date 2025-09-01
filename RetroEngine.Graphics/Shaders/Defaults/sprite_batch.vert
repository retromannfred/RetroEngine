#version 330 core

layout(location = 0) in vec3 in_pos;
layout(location = 1) in vec2 in_tex;

layout(location = 2) in mat4 in_model;
layout(location = 6) in vec4 in_color;
layout(location = 7) in vec4 in_tex_coords;

uniform mat4 u_projection;
uniform mat4 u_view;

out vec2 v_tex_coord;
out vec4 v_color;

void main()
{
    gl_Position = u_projection * u_view * in_model * vec4(in_pos, 1.0);
    v_color = in_color;
    v_tex_coord = mix(in_tex_coords.xy, in_tex_coords.zw, in_tex);
}