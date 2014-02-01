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

float4 PositionNormalTexturePS(in float4 inputPosition : POSITION0, in float3 inputNormal : TEXCOORD0,
							   in float2 inputTextureCoordinate : TEXCOORD1, in float3 inputWorldPosition : TEXCOORD2) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	float4 ec = tex2D(EtSampler, inputTextureCoordinate);

	return light(ec, dc, inputWorldPosition, inputNormal);
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

float4 PositionNormalTextureTwoPS(in float4 inputPosition : POSITION0, in float3 inputNormal : TEXCOORD0,
								in float2 inputTextureCoordinate : TEXCOORD1, in float2 inputTextureCoordinateTwo : TEXCOORD2,
								in float3 inputWorldPosition : TEXCOORD3) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	//dc *= tex2D(DtSampler, input.TextureCoordinateTwo);
	float ec = tex2D(EtSampler, inputTextureCoordinate);

	return light(ec, dc, inputWorldPosition, inputNormal);
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
