#include "includes/Standard.fxh"

float4 Dc;

Texture Et;
sampler EtSampler = 
	sampler_state
	{
		texture = <Et>;
		magfilter = ANISOTROPIC;
		minfilter = ANISOTROPIC;
		mipfilter = LINEAR;
		AddressU = WRAP;
		AddressV = WRAP;
	};

// ------ PositionNormalTexture -----------------------------------------------------------

float4 PositionNormalTexturePS(PositionNormalTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	float4 ec = tex2D(EtSampler, input.TextureCoordinate);

	return light(ec, dc, input.WorldPosition, input.Normal);
}

technique PositionNormalTexture
{
    pass Pass1
    {		
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionNormalTextureVS();
        PixelShader = compile ps_3_0 PositionNormalTexturePS();
    }
}

// ------ PositionNormalTextureTwo -----------------------------------------------------------

float4 PositionNormalTextureTwoPS(PositionNormalTextureTwoOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	//dc *= tex2D(DtSampler, input.TextureCoordinateTwo);
	float ec = tex2D(EtSampler, input.TextureCoordinate);

	return light(ec, dc, input.WorldPosition, input.Normal);
}

technique PositionNormalTextureTwo
{
    pass Pass1
    {		
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionNormalTextureTwoVS();
        PixelShader = compile ps_3_0 PositionNormalTextureTwoPS();
    }
}

#include "includes/BasicPosition.fxh"
#include "includes/BasicPositionNormal.fxh"
#include "includes/BasicPositionTexture.fxh"
#include "includes/BasicPositionDiffuseTexture.fxh"
