#include 'light.glsl'
uniform sampler2D DtSampler;
varying vec4 position;
varying vec2 texcoord;
varying vec4 diffuse;

void main(void)
{
	vec4 result = texture(DtSampler, texcoord);
	result *= diffuse * vec4(1,0,0,1);
	gl_FragColor = result;
}
