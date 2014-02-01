#include "includes/Standard.fxh"

float4 Dc;

Texture Bt;
sampler BtSampler = 
	sampler_state
	{
		texture = <Bt>;
		magfilter = ANISOTROPIC;
		minfilter = ANISOTROPIC;
		mipfilter = LINEAR;
		AddressU = WRAP;
		AddressV = WRAP;
	};

// ------ PositionNormalTextureTwo -----------------------------------------------------------

float4 PositionNormalTextureTwoPS(PositionNormalTextureTwoOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	dc *= tex2D(BtSampler, input.TextureCoordinateTwo);

	return light(0, dc, input.WorldPosition, input.Normal);
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
#include "includes/BasicPositionNormalTexture.fxh"
#include "includes/BasicPositionDiffuseTexture.fxh"
