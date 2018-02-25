uniform mat4 World;
uniform mat4 View;
uniform mat4 Proj;

uniform vec2 MapCenter;
uniform sampler2D Map;

in vec3 position;
in vec3 normal;
in vec4 color;


out vec3 psNormal;
out vec4 psColor;
out vec2 psTexCoord;


void main()
{
    mat4 worldViewProj = Proj * View * World;

    psColor = color;
    psNormal = normal;
    
	gl_Position = worldViewProj*vec4(position,1);
}