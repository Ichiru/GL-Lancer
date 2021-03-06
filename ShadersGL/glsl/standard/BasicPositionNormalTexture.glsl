#include 'light.glsl'
//varyings
uniform sampler2D DtSampler;
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;
varying vec2 texcoord;
void main(void)
{
	vec4 dc = texture2D(DtSampler, texcoord);
	gl_FragColor = light (vec4(0.0,0.0,0.0,0.0), dc, worldPosition.xyz, normal.xyz) * vec4(1,0,0,1);
}
