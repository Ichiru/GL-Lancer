#include "includes/Standard.fxh"

float4 Dc;
float4 Ec;

// ------ PositionTexture -----------------------------------------------------------

float4 PositionTexturePS(PositionTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
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

float4 PositionNormalTexturePS(PositionNormalTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	return light(Ec, dc, input.WorldPosition, input.Normal);
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

float4 PositionNormalTextureTwoPS(PositionNormalTextureTwoOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	//dc *= tex2D(DtSampler, input.TextureCoordinateTwo);
	return light(Ec, dc, input.WorldPosition, input.Normal);
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

float4 PositionDiffuseTexturePS(PositionDiffuseTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
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
