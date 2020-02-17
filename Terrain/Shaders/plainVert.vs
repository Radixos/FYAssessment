#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoords;

uniform mat4 model;
//uniform vec3 eyePos;

out vec2 TexCoords;
out vec3 FragPos;

//TODO: check in and out variables names, compare to TessellationLab1

void main()
{
    TexCoords = aTexCoords;  
    FragPos = aPos;	//... = vec3(model * vec4(aPos, 1.0));
    gl_Position = model* vec4(FragPos, 1.0);
}