#include '../standard/light.glsl'
//uniforms
uniform vec4 Dc;
uniform float Oc;
//varyings
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;

void main(void)
{
	vec4 dc = Dc * vec4(Oc,Oc,Oc,Oc);
	gl_FragColor = light (vec4(0.0,0.0,0.0,0.0), dc, worldPosition.xyz, normal.xyz);
}
