#include 'standard/light.glsl'
uniform sampler2D DtSampler;
varying vec4 position;
varying vec4 normal;
varying vec4 worldPosition;
varying vec2 texcoord;
uniform vec3 Dc;
uniform vec3 Ac;
uniform float Alpha;
uniform float Fade;
uniform float Scale;
void main(void)
{
	vec4 result = texture(DtSampler, texcoord);
    result.xyz *= Dc * Ac;
    result.w = Alpha;
    gl_FragColor = mix(vec4(Ac, Alpha), result, Scale);
}
