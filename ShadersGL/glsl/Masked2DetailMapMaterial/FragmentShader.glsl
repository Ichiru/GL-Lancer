uniform sampler2D DtSampler;
uniform sampler2D Dm0Sampler;
uniform sampler2D Dm1Sampler;
uniform int FlipU;
uniform int FlipV;
uniform float TileRate0;
uniform float TileRate1;
uniform vec4 Dc;
uniform vec4 Ac;
uniform vec4 AmbientColor;

varying vec4 position;
varying vec2 texcoord;
void main(void)
{
    vec2 result_texcoord = texcoord;
    if (FlipU == 1) result_texcoord.x = 1 - result_texcoord.x;
    if (FlipV == 1) result_texcoord.y = 1 - result_texcoord.y;
    vec4 dc = texture(DtSampler, result_texcoord);
    dc *= Dc;
    vec2 texcoord0 = result_texcoord * TileRate0;
    vec4 detail0 = texture(Dm0Sampler, texcoord0);
    vec2 texcoord1 = result_texcoord * TileRate1;
    vec4 detail1 = texture(Dm1Sampler, texcoord1);
    vec4 detail = mix(detail0, detail1, dc.w);
    dc *= detail;
    gl_FragColor = Ac * dc;
}
