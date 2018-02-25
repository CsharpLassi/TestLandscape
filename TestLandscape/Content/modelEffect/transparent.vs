uniform mat4 World;
uniform mat4 View;
uniform mat4 Proj;


in vec3 position;






void main()
{
    mat4 worldViewProj = Proj * View * World;

    
	gl_Position = worldViewProj*vec4(position,1);
}