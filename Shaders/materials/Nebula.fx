#include "includes/Standard.fxh"

float4 Ec;

// ------ PositionTexture -----------------------------------------------------------

float4 PositionTexturePS(PositionTextureOut input) : COLOR0
{
	float4 result = tex2D(DtSampler, input.TextureCoordinate);
	//result.rgb += Ec;

	return result;
}

technique PositionTexture
{
    pass Pass1
    {
		AlphaBlendEnable = true;
		SrcBlend = srcalpha;
		DestBlend = one;
		ZEnable = false;
		ZWriteEnable = false;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionTextureVS();
        PixelShader = compile ps_3_0 PositionTexturePS();
    }
}

// ------ PositionDiffuseTexture -----------------------------------------------------------

float4 PositionDiffuseTexturePS(PositionDiffuseTextureOut input) : COLOR0
{
	float4 result = tex2D(DtSampler, input.TextureCoordinate);
	//result.rgb += Ec;
	result *= input.Diffuse;

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
