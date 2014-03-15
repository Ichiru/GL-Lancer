#include 'standard/light.glsl'
varying vec4 position;
varying float4 Dc;

void main(void)
{
	gl_FragColor = Dc + AmbientColor;
}
