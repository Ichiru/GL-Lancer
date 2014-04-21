#include '../standard/light.glsl'
uniform sampler2D DtSampler;
uniform vec4 Dc;
uniform vec4 Ec;
uniform float Oc;
varying vec4 position;
varying vec2 texcoord;
varying vec4 diffuse;

void main(void)
{
	vec4 dc = texture2D(DtSampler, texcoord);
	dc = dc * Ec * Oc * diffuse;
	dc += Dc;
	gl_FragColor = dc;
}
