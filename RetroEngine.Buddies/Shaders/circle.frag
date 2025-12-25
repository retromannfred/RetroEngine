#version 330 core

in vec2 v_pos;

uniform mat4 u_model;
uniform vec4 u_color;
uniform float u_sq_radius;
uniform float u_thickness;

out vec4 out_frag_color;

void main()
{
    float r2 = dot(v_pos, v_pos);
    float sx = length(u_model[0].xyz);
    float sy = length(u_model[1].xyz);
    float s  = max(sx, sy);

    float t = u_thickness / s;

    float inner = u_sq_radius - t;
    float outer = u_sq_radius + t;

    if (r2 >= inner && r2 <= outer)
        out_frag_color = u_color;
    else
        discard;
}
