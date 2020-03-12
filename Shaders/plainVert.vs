#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoords;

uniform mat4 model;

out vec2 TexCoords;
out vec3 FragPos;

void main()
{
    TexCoords = aTexCoords;  
    FragPos = vec3(model * vec4(aPos, 1.0));
}