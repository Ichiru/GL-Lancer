#include 'standard/light.glsl'
uniform sampler2D DtSampler;
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;
varying vec2 texcoord;

void main(void)
{
	vec4 dc = texture(DtSampler, texcoord);
	gl_FragColor = light(vec4(0.0,0.0,0.0,0.0), dc, worldPosition.xyz, normal.xyz);
}
