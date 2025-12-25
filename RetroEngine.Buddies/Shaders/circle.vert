#version 330 core

layout (location = 0) in vec2 in_pos;

uniform mat4 u_model;
uniform mat4 u_view;
uniform mat4 u_projection;

out vec2 v_pos;

void main()
{
    v_pos = in_pos;
    gl_Position = u_projection * u_view * u_model * vec4(in_pos, 0.0, 1.0);
}