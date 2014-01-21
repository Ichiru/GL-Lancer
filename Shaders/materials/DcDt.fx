#include "includes/Standard.fxh"

float4 Dc;

// ------ Position -----------------------------------------------------------

float4 PositionPS(float4 input : POSITION0) : COLOR0
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
		CullMode = cw;

        VertexShader = compile vs_3_0 PositionVS();
        PixelShader = compile ps_3_0 PositionPS();
    }
}


// ------ PositionTexture -----------------------------------------------------------

float4 PositionTexturePS(PositionTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	return dc * AmbientColor;
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


// ------ PositionNormalTexture -----------------------------------------------------------

float4 PositionNormalTexturePS(PositionNormalTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	return light(0, dc, input.WorldPosition, input.Normal);
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
	//dc += tex2D(DtSampler, input.TextureCoordinateTwo);;
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

#include "includes/BasicPositionNormal.fxh"
#include "includes/BasicPositionDiffuseTexture.fxh"
