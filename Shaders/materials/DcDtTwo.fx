#include "includes/Standard.fxh"

float4 Dc;

// ------ Position -----------------------------------------------------------

float4 PositionPixelShaderFunction(float4 input : POSITION0) : COLOR0
{
    return Dc + AmbientColor;
}

technique Position
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = none;

        VertexShader = compile vs_3_0 PositionVS();
        PixelShader = compile ps_3_0 PositionPixelShaderFunction();
    }
}

// ------ PositionNormal -----------------------------------------------------------

float4 PositionNormalPixelShaderFunction(in float4 inputPosition : POSITION0, in float3 inputNormal: TEXCOORD0, in float3 inputWorldPosition: TEXCOORD1) : COLOR0
{
	return light(0, Dc, inputWorldPosition, inputNormal);
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
        PixelShader = compile ps_3_0 PositionNormalPixelShaderFunction();
    }
}

// ------ PositionTexture -----------------------------------------------------------

float4 PositionTexturePixelShaderFunction(in float4 inputPosition : POSITION0, in float2 inputTextureCoordinate : TEXCOORD) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	dc *= Dc * AmbientColor;
	
	return dc;
}

technique PositionTexture
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = none;

		VertexShader = compile vs_3_0 PositionTextureVS();
        PixelShader = compile ps_3_0 PositionTexturePixelShaderFunction();
    }
}

// ------ PositionNormalTexture -----------------------------------------------------------

float4 PositionNormalTexturePixelShaderFunction(in float4 inputPosition : POSITION0, in float3 inputNormal : TEXCOORD0,
							   in float2 inputTextureCoordinate : TEXCOORD1, in float3 inputWorldPosition : TEXCOORD2) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	dc *= Dc;

	return light(0, dc, inputWorldPosition, inputNormal);
}

technique PositionNormalTexture
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = none;

		VertexShader = compile vs_3_0 PositionNormalTextureVS();
        PixelShader = compile ps_3_0 PositionNormalTexturePixelShaderFunction();
    }
}

// ------ PositionNormalTextureTwo -----------------------------------------------------------

float4 PositionNormalTextureTwoPixelShaderFunction(in float4 inputPosition : POSITION0, in float3 inputNormal : TEXCOORD0,
								in float2 inputTextureCoordinate : TEXCOORD1, in float2 inputTextureCoordinateTwo : TEXCOORD2,
								in float3 inputWorldPosition : TEXCOORD3) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	//dc *= tex2D(DtSampler, input.TextureCoordinateTwo);
	dc *= Dc;

	return light(0, dc, inputWorldPosition, inputNormal);
}

technique PositionNormalTextureTwo
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = none;

		VertexShader = compile vs_3_0 PositionNormalTextureTwoVS();
        PixelShader = compile ps_3_0 PositionNormalTextureTwoPixelShaderFunction();
    }
}

// ------ PositionDiffuseTexture -----------------------------------------------------------

float4 PositionDiffuseTexturePixelShaderFunction(in float4 inputPosition : POSITION0, in float4 inputDiffuse : COLOR0,
							    in float2 inputTextureCoordinate : TEXCOORD0) : COLOR0
{
	float4 dc = tex2D(DtSampler, inputTextureCoordinate);
	dc *= Dc;
	dc *= inputDiffuse;

	return dc;
}

technique PositionDiffuseTexture
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = none;

		VertexShader = compile vs_3_0 PositionDiffuseTextureVS();
        PixelShader = compile ps_3_0 PositionDiffuseTexturePixelShaderFunction();
    }
}
