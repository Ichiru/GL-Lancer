#include 'standard/light.glsl'
uniform sampler2D DtSampler;
varying vec4 position;
varying vec2 texcoord;

void main(void)
{
	vec4 dc = texture(DtSampler, texcoord);
	gl_FragColor = dc * AmbientColor;
}
