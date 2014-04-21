#include '../standard/light.glsl'
uniform sampler2D DtSampler;
uniform vec4 Dc;
uniform vec4 Ec;
varying vec4 position;
varying vec2 texcoord;
varying vec4 diffuse;

void main(void)
{
	vec4 dc = texture2D(DtSampler, texcoord);
	gl_FragColor = dc + Ec;
}
