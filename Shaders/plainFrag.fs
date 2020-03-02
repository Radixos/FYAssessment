#version 330 core
out vec4 FragColor;

in vec2 TexCoords;
in vec3 FragPos;

//uniform vec3 lightPos;
//uniform vec3 eyePos;

void main()
{
    //vec3 lightDir   = normalize(lightPos - FragPos);
    //vec3 viewDir    = normalize(eyePos - FragPos);
    //vec3 halfwayDir = normalize(lightDir + viewDir);

    //float spec = pow(max(dot(normal, halfwayDir), 0.0), shininess);
    //vec3 specular = lightColor * spec;

    FragColor = vec4(0.2,0.8,0.3,1.0);
}
	