#include '../standard/light.glsl'

uniform samplerCube PlanetTexture;
varying vec4 position;
varying vec3 texcoord;
varying vec4 normal;
varying vec4 worldPosition;

void main(void)
{
    vec4 dc = texture(PlanetTexture, texcoord);
    gl_FragColor = light(vec4(0,0,0,0), dc, worldPosition.xyz, normal.xyz);
}
