uniform vec4 AmbientColor;
uniform vec4 Dc;
uniform vec4 Ac;
uniform int FlipU;
uniform int FlipV;
uniform float TileRate;
uniform sampler2D DtSampler;
uniform sampler2D Dm1Sampler;

varying vec4 position;
varying vec2 texcoord;
void main(void)
{
	vec2 tex = texcoord;
	if(FlipU > 0)
	    tex.x = 1 - tex.x;
	if(FlipV > 0)
	    tex.y = 1 - tex.y;
	vec4 dc = texture2D(DtSampler, tex);
	dc *= Dc;
	tex *= vec2(TileRate, TileRate);
	dc *= mix(texture2D(Dm1Sampler, tex), vec4(1,1,1,1), dc.w);
	gl_FragColor = Ac * dc;
}
