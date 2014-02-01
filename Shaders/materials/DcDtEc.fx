#include "includes/Standard.fxh"

float4 Dc;
float4 Ec;

// ------ PositionTexture -----------------------------------------------------------

float4 PositionTexturePS(in float4 inputPosition : POSITION0, in float2 inputTextureCoordinate : TEXCOORD0) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	return Ec + dc;
}

technique PositionTexture
{
    pass Pass1
    {		
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = one;
		ZEnable = false;
		ZWriteEnable = false;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionTextureVS();
        PixelShader = compile ps_3_0 PositionTexturePS();
    }
}


// ------ PositionNormalTexture -----------------------------------------------------------

float4 PositionNormalTexturePS(in float4 inputPosition : POSITION0, in float3 inputNormal : TEXCOORD0,
							   in float2 inputTextureCoordinate : TEXCOORD1, in float3 inputWorldPosition : TEXCOORD2 ) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	return light(Ec, dc, inputWorldPosition, inputNormal);
}

technique PositionNormalTexture
{
    pass Pass1
    {		
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = one;
		ZEnable = false;
		ZWriteEnable = false;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionNormalTextureVS();
        PixelShader = compile ps_3_0 PositionNormalTexturePS();
    }
}


// ------ PositionNormalTextureTwo -----------------------------------------------------------

float4 PositionNormalTextureTwoPS(in float4 inputPosition : POSITION0, in float3 inputNormal : TEXCOORD0,
								in float2 inputTextureCoordiante : TEXCOORD1, in float2 inputTextureCoordinateTwo : TEXCOORD2,
								in float3 inputWorldPosition : TEXCOORD3) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	//dc *= tex2D(DtSampler, input.TextureCoordinateTwo);
	return light(Ec, dc, inputWorldPosition, inputNormal);
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

// ------ PositionDiffuseTexture -----------------------------------------------------------

float4 PositionDiffuseTexturePS(in float4 inputPosition : POSITION0, in float4 inputDiffuse : COLOR0,
							    in float2 inputTextureCoordinate : TEXCOORD0) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	return Ec + dc;
}

technique PositionDiffuseTexture
{
    pass Pass1
    {		
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = one;
		ZEnable = false;
		ZWriteEnable = false;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionDiffuseTextureVS();
        PixelShader = compile ps_3_0 PositionDiffuseTexturePS();
    }
}

#include "includes/BasicPosition.fxh"
#include "includes/BasicPositionNormal.fxh"
