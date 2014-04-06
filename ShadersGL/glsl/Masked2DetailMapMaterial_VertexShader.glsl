//standard uniforms
uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
uniform vec3 CameraPosition;
//standard attributes
attribute vec3 vertex_position;
attribute vec2 vertex_texcoord0;
//varyings
varying vec4 position;
varying vec2 texcoord;
void main(void)
{
	position = vec4(vertex_position,0);
	texcoord = vertex_texcoord0;
	gl_Position = position;
}
