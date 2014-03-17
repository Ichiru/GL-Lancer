#include 'light.glsl'
//varyings
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;

void main(void)
{
	vec4 dc = vec4(1,0,0,1);
	gl_FragColor = light (vec4(0.0,0.0,0.0,0.0), dc, worldPosition.xyz, normal.xyz);
}
