#include '../standard/light.glsl'
uniform sampler2D DtSampler;
uniform vec4 Ec;
varying vec4 position;
varying vec2 texcoord;

void main(void)
{
	vec4 dc = texture2D(DtSampler, texcoord);
	gl_FragColor = dc;
}
