#include 'light.glsl'
//varyings
uniform sampler2D DtSampler;
varying vec4 position;
varying vec2 texcoord;
void main(void)
{
	vec4 dc = texture(DtSampler, texcoord);
	dc = vec4(dc.xyz * AmbientColor.xyz * vec3(1,0,0),dc.w);
}
