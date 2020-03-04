#version 330 core
out vec4 FragColor;
//out vec3 colour;

in vec2 TexCoords;
in vec3 gWorldPos_FS_in;
in vec3 gnorms;

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
//uniform vec3 lightPos;
uniform vec3 eyePos;
uniform bool blinn;
uniform int scale;
//uniform vec3 colour;

void main()
{
    //vec3 lightDir   = normalize(lightPos - FragPos);
    //vec3 viewDir    = normalize(eyePos - FragPos);
    //vec3 halfwayDir = normalize(lightDir + viewDir);

    //float spec = pow(max(dot(normal, halfwayDir), 0.0), shininess);
    //vec3 specular = lightColor * spec;

    vec3 col = vec3(0.8,0.8,0.8);
  
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
    
    // combine results
   
    float height = gWorldPos_FS_in.y/scale;
    vec4 green = vec4(0.3, 0.35, 0.15, 0.5);
    vec4 gray = vec4(0.5, 0.4, 0.5, 0.5);
    vec4 brown = vec4(0.5, 0.0, 0.0, 0.5);

    if(height > 0.4)
        vec3 col = (mix(green, gray, smoothstep(0.3, 1.0, height)).rgb);
    else if(height > 0.8)
        vec3 col = (mix(gray, brown, smoothstep(0.3, 1.0, height)).rgb);

	vec3 ambient = dirLight.ambient * mat.ambient;     
    vec3 diffuse  = dirLight.diffuse  * (diff * mat.diffuse);
    vec3 specular = dirLight.specular * (spec * mat.specular);
    FragColor = vec4((ambient + diffuse + specular), 1.0f);
    //FragColor = vec4((ambient+diffuse),1.0f);
    //FragColor =  texture(texture1, TexCoords);
    //FragColor = vec4(norms,1.0f);


    //FragColor = vec4(0.2,0.8,0.3,1.0);
    
    

}
	