#include '../standard/light.glsl'
varying vec4 position;
uniform vec4 Dc;
void main(void)
{
	gl_FragColor = Dc + AmbientColor;
}
