float4x4 World;
float4x4 View;
float4x4 Projection;

float3 CameraPosition;

float4 AmbientColor = float4(1.0f, 1.0f, 1.0f, 1.0f);

texture Dt;
sampler DtSampler = 
	sampler_state
	{
		texture = <Dt>;
		magfilter = ANISOTROPIC;
		minfilter = ANISOTROPIC;
		mipfilter = LINEAR;
		AddressU = CLAMP;
		AddressV = CLAMP;
	};

float4 Dc;
float4 Ac;

Texture Dm;
sampler DmSampler = 
	sampler_state
	{
		texture = <Dm>;
		magfilter = ANISOTROPIC;
		minfilter = ANISOTROPIC;
		mipfilter = LINEAR;
		AddressU = WRAP;
		AddressV = WRAP;
	};

int FlipU;
int FlipV;
float TileRate;


struct PositionTextureIn
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct PositionTextureOut
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
};

PositionTextureOut PositionTextureVS(PositionTextureIn input)
{
    PositionTextureOut output;

    output.Position = input.Position;
	output.TextureCoordinate = input.TextureCoordinate;
	
	return output;
}

float4 PositionTexturePS(PositionTextureOut input) : COLOR0
{
	float2 texcoord = input.TextureCoordinate;
	if (FlipU) texcoord.x = 1 - texcoord.x;
	if (FlipV) texcoord.y = 1 - texcoord.y;
	
	float4 dc = tex2D(DtSampler, texcoord);
	dc *= Dc;

	texcoord *= TileRate;
	dc *= tex2D(DmSampler, texcoord);

	return Ac * dc;
}

technique PositionTexture
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionTextureVS();
        PixelShader = compile ps_3_0 PositionTexturePS();
    }
}
