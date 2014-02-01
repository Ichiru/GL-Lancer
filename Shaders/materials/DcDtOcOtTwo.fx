#include "includes/Standard.fxh"

float4 Dc;
float Oc;

// ------ PositionNormal -----------------------------------------------------------

float4 PositionNormalPS(in float4 inputPosition : POSITION0, in float3 inputNormal: TEXCOORD0, in float3 inputWorldPosition: TEXCOORD1) : COLOR0
{
	float4 dc = Dc * Oc;
	return light(0, dc, inputWorldPosition, inputNormal);
}

technique PositionNormal
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = none;

		VertexShader = compile vs_3_0 PositionNormalVS();
        PixelShader = compile ps_3_0 PositionNormalPS();
    }
}


// ------ PositionNormalTexture -----------------------------------------------------------

float4 PositionNormalTexturePS(in float4 inputPosition : POSITION0, in float3 inputNormal : TEXCOORD0,
							   in float2 inputTextureCoordinate : TEXCOORD1, in float3 inputWorldPosition : TEXCOORD2) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	clip(dc.a <= Oc ? -1 : 1);

	return light(0, dc, inputWorldPosition, inputNormal);
}

technique PositionNormalTexture
{
    pass Pass1
    {
		AlphaBlendEnable = false;
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = none;

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
	clip(dc.a <= Oc ? -1 : 1);

	return light(0, dc, inputWorldPosition, inputNormal);
}

technique PositionNormalTextureTwo
{
    pass Pass1
    {
		AlphaBlendEnable = false;
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = none;

		VertexShader = compile vs_3_0 PositionNormalTextureTwoVS();
        PixelShader = compile ps_3_0 PositionNormalTextureTwoPS();
    }
}

#include "includes/BasicPosition.fxh"
#include "includes/BasicPositionTexture.fxh"
#include "includes/BasicPositionDiffuseTexture.fxh"
