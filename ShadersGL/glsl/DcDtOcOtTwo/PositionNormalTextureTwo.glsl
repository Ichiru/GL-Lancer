#include '../standard/light.glsl'
//varyings
uniform sampler2D DtSampler;
uniform vec4 Dc;
uniform float Oc;
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;
varying vec2 texcoord;
void main(void)
{
	vec4 dc = texture(DtSampler, texcoord);
	if(dc.w <= Oc) {
	    discard;
	}
	gl_FragColor = light (vec4(0.0,0.0,0.0,0.0), dc, worldPosition.xyz, normal.xyz);
}
