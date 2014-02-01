#include "includes/Standard.fxh"

float4 Dc;
float4 Ec;
float Oc;

// ------ PositionTexture -----------------------------------------------------------

float4 PositionTexturePS(in float4 inputPosition : POSITION0, in float2 inputTextureCoordinate : TEXCOORD0) : COLOR0
{
	float4 result = tex2D(DtSampler, inputTextureCoordinate);
	result.rgb *= Ec * Oc;
	result.rgb += Dc;

	return result;
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

// ------ PositionDiffuseTexture -----------------------------------------------------------

float4 PositionDiffuseTexturePS(in float4 inputPosition : POSITION0, in float4 inputDiffuse : COLOR0,
							    in float2 inputTextureCoordinate : TEXCOORD0) : COLOR0
{
	float4 result = tex2D(DtSampler, inputTextureCoordinate);
	result.rgb *= Ec * Oc * inputDiffuse;
	result.rgb += Dc;

	return result;
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
#include "includes/BasicPositionNormalTexture.fxh"
#include "includes/BasicPositionNormalTextureTwo.fxh"
