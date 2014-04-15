#include '../standard/light.glsl'
uniform sampler2D DtSampler;
uniform vec4 Dc;
uniform vec4 Ec;
uniform float Oc;
varying vec4 position;
varying vec2 texcoord;

void main(void)
{
	vec4 dc = texture(DtSampler, texcoord);
	dc = dc * (Ec * Oc);
	dc += Dc;
	gl_FragColor = dc;
}
