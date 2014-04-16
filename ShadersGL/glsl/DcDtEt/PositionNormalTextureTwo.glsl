#include '../standard/light.glsl'
uniform sampler2D DtSampler;
uniform sampler2D EtSampler;
uniform vec4 Dc;
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;
varying vec2 texcoord;
varying vec2 texcoord2;

void main(void)
{
	vec4 dc = texture(DtSampler, texcoord);
	vec4 ec = texture(EtSampler, texcoord);
	gl_FragColor = light(ec, dc, worldPosition.xyz, normal.xyz);
}
