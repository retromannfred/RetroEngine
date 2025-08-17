#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTex;
layout(location = 2) in mat4 instanceMatrix;
layout(location = 6) in vec4 instanceColor;
layout(location = 7) in vec4 instanceTexCoords;

out vec2 pass_texCoord;
out vec4 pass_color;

uniform mat4 projection;
uniform mat4 view;

void main()
{
    gl_Position = projection * view * instanceMatrix * vec4(aPos, 1.0);
    pass_color = instanceColor;
    pass_texCoord = mix(instanceTexCoords.xy, instanceTexCoords.zw, aTex);
}