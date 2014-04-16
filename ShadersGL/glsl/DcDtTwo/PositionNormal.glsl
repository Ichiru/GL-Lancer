#include '../standard/light.glsl'
//uniforms
uniform vec4 Dc;
//varyings
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;

void main(void)
{
	gl_FragColor = light (vec4(0.0,0.0,0.0,0.0), Dc, worldPosition.xyz, normal.xyz);
}
