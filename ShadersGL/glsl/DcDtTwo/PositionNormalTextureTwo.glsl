#include '../standard/light.glsl'
uniform sampler2D DtSampler;
uniform vec4 Dc;
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;
varying vec2 texcoord;
varying vec2 texcoord2;

void main(void)
{
	vec4 dc = texture2D(DtSampler, texcoord);
	dc *= Dc;
	gl_FragColor = light(vec4(0.0,0.0,0.0,0.0), dc, worldPosition.xyz, normal.xyz);
}
