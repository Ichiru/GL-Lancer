#include 'light.glsl'
varying vec4 position;
void main(void)
{
	gl_FragColor = vec4(vec3(1, 0, 0) * AmbientColor.xyz, 1);
}
