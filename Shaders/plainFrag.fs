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
uniform bool shadowBool;
uniform mat4 lightSpaceMatrix;
uniform sampler2D shadowMap;

float calcShadow(vec4 fragPosLightSpace, float bias);

void main()
{
    vec4 fragPosLightSpace = lightSpaceMatrix * vec4(gWorldPos_FS_in, 1.0f);
    float bias = max(0.005 * (1.0 - dot(gnorms, dirLight.direction)), 0.001); //max(0.005 * (1.0 - dot(tessNormals, dirLight.direction)), 0.001);

    vec3 col = vec3(0.2,0.2,0.2);

    float height = gWorldPos_FS_in.y / gScale;
    vec4 darkBlue = vec4(0.05f, 0.15f, 0.2f, 0.5f);
    vec4 blue = vec4(0.25f, 0.35f, 0.6f, 0.5f);
    vec4 yellow = vec4(0.75f, 0.7f, 0.3f, 0.5f);
    vec4 green = vec4(0.25f, 0.55f, 0.25f, 0.5f);
    vec4 brown = vec4(0.5f, 0.5f, 0.2f, 0.5f);
    vec4 gray = vec4(0.5f, 0.4f, 0.5f, 0.5f);
    vec4 fogAir = vec4(0.4f, 0.4f, 0.4f, 1.0f);
    vec4 fogWater = vec4(0.05f, 0.15f, 0.2f, 0.5f);
  
    vec3 viewDir = normalize(eyePos - gWorldPos_FS_in);
    vec3 norm = normalize(gnorms);
    vec3 lightDir = normalize(-dirLight.direction);

    // diffuse shading
    float diff = max(dot(norm, dirLight.direction), 0.0f);

    // specular shading
    float spec = 0.f;

    if(blinn)
    {
        vec3 helfwayDir = normalize(dirLight.direction + viewDir);
        spec = pow(max(dot(norm, helfwayDir), 0.0f), mat.shininess*2.5f);
    }
    else
    {
        vec3 reflectDir = reflect(-dirLight.direction, norm);
        spec = pow(max(dot(viewDir, reflectDir), 0.0f), mat.shininess);
    }
    
    vec3 ambient = dirLight.ambient * mat.ambient;     
    vec3 diffuse = dirLight.diffuse * (diff * mat.diffuse);
    vec3 specular = dirLight.specular * (spec * mat.specular);

    if (height < 0.0f)
        col = vec3(mix(darkBlue, blue, smoothstep(-0.25f, 0.025f, height)).rgb);
    else if(height < 0.1f)
        col = vec3(mix(blue, yellow, smoothstep(0.01f, 0.12f, height)).rgb);
    else if(height < 0.15f)
        col = vec3(mix(yellow, green, smoothstep(0.08f, 0.16f, height)).rgb);
    else if(height < 0.5f)
        col = vec3(mix(green, brown, smoothstep(0.1f, 0.6f, height)).rgb);
    else if(height < 0.85f)
        col = vec3(mix(brown, gray, smoothstep(0.5f, 0.9f, height)).rgb);
    else
        col = vec3(gray.rgb);
    
    float shadow;

    if(shadowBool)
    {
        //if (height > 0.07f)
        //{
            shadow = calcShadow(fragPosLightSpace, bias);
        //}
    }
    else
    {
        shadow = 0.0f;
    }

    vec4 result = vec4((ambient + (1.0f - shadow)*(diffuse + specular)) * col, 1.0f);

    if(fog)
    {
        FragColor = mix(vec4(0.4f, 0.4f, 0.4f, 1.0f), result, gVisibility);

        //if(height > 0.0f)
        //    FragColor = mix(fogAir, result, gVisibility);
        //else
        //    FragColor = mix(fogWater, result, gVisibility);
    }
    else
    {
        FragColor = result;
    }
}

float calcShadow(vec4 fragPosLightSpace, float bias)
{
    float shadow = 0.0f;
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5f + 0.5f;
    float closestDepth = texture(shadowMap, projCoords.xy).r;
    float currentDepth = projCoords.z;
    vec2 texelSize = 1.0f / textureSize(shadowMap, 0);

    for(int i = -1; i < 2; i++)
    {
        for(int j = -1; j < 2; j++)
        {
            float pcf = texture(shadowMap, projCoords.xy + vec2(i, j) * texelSize).r;
            if(currentDepth - bias > pcf)
            {
                shadow += 1.0f;
            }
        }
    }
    shadow = shadow/9;
    //float shadow = currentDepth > closestDepth ? 1.0 : 0.0;
    if(projCoords.z > 1.0f)
    {
        shadow = 0.0f;
    }
    return shadow * 0.65f;
}