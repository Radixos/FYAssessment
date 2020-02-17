#version 330 core
out vec4 FragColor;

in vec3 gNormals;
in vec3 gWorldPos_FS_in;

void main()
{
    FragColor = vec4(0.2,0.8,0.3,1.0);
}