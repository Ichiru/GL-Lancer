uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
varying vec4 position;
void main(void)
{
	vec4 worldPosition = World * gl_Vertex;
	vec4 viewPosition = worldPosition * View;
	position = viewPosition * Projection;
	gl_Position = position;
}
