#include '../standard/light.glsl'
//varyings
uniform sampler2D DtSampler;
uniform vec4 Dc;
uniform float Oc;
varying vec4 position;
varying vec2 texcoord;
void main(void)
{
	vec4 dc = texture2D(DtSampler, texcoord);
	dc *= vec4(Oc, Oc, Oc, Oc);
	gl_FragColor = dc;
}
