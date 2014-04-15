#include '../standard/light.glsl'
uniform vec4 Dc;
uniform vec4 Ec;
uniform sampler2D DtSampler;
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;
varying vec2 texcoord;

void main(void)
{
	vec4 dc = texture(DtSampler, texcoord);
	gl_FragColor = light(Ec, dc, worldPosition.xyz, normal.xyz);
}
