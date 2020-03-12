#version 330 core
out vec4 FragColor;

in vec2 TexCoords;
in vec3 gWorldPos_FS_in;
in vec3 gnorms;
in float gScale;
in float gVisibility;

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;    
    float shininess;
};

struct DirLight {
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform DirLight dirLight;
uniform Material mat;
uniform vec3 eyePos;
uniform bool blinn;
uniform bool fog;

void main()
{
    vec3 col = vec3(0.2,0.2,0.2);

    float height = gWorldPos_FS_in.y / gScale;
    vec4 green = vec4(0.25f, 0.55f, 0.25f, 0.5f);
    vec4 gray = vec4(0.5f, 0.4f, 0.5f, 0.5f);
    vec4 brown = vec4(0.5f, 0.5f, 0.2f, 0.5f);
    vec4 blue = vec4(0.25f, 0.25f, 0.55f, 0.5f);
  
    vec3 viewDir = normalize(eyePos - gWorldPos_FS_in);
    vec3 norm = normalize(gnorms);
    vec3 lightDir = normalize(-dirLight.direction);

    // diffuse shading
    float diff = max(dot(norm, dirLight.direction), 0.0);

    // specular shading
    float spec = 0.f;

    if(blinn)
    {
        vec3 helfwayDir = normalize(dirLight.direction + viewDir);
        spec = pow(max(dot(norm, helfwayDir), 0.0), mat.shininess*2.5f);    //2.f
    }
    else
    {
        vec3 reflectDir = reflect(-dirLight.direction, norm);
        spec = pow(max(dot(viewDir, reflectDir), 0.0), mat.shininess);
    }
    
    vec3 ambient = dirLight.ambient * mat.ambient;     
    vec3 diffuse  = dirLight.diffuse  * (diff * mat.diffuse);
    vec3 specular = dirLight.specular * (spec * mat.specular);

    if(height < 0.1f)
        col = vec3(mix(blue, green, smoothstep(0.05f, 0.1f, height)).rgb);
    else if(height < 0.5f)
        col = vec3(mix(green, gray, smoothstep(0.15f, 0.5f, height)).rgb);
    else if(height < 0.8f)
        col = vec3(mix(gray, brown, smoothstep(0.55f, 0.8f, height)).rgb);
    else
        col = vec3(gray.rgb);

    vec4 result = vec4((ambient + diffuse + specular) * col, 1.0f);

    if(fog)
    {
        FragColor = mix(vec4(0.4f, 0.4f, 0.4f, 0.5f), result, gVisibility);
    }
    else
    {
        FragColor = result;
    }
}