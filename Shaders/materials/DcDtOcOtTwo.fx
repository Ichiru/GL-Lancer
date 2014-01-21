#include "includes/Standard.fxh"

float4 Dc;
float Oc;

// ------ PositionNormal -----------------------------------------------------------

float4 PositionNormalPS(PositionNormalOut input) : COLOR0
{
	float4 dc = Dc * Oc;
	return light(0, dc, input.WorldPosition, input.Normal);
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

float4 PositionNormalTexturePS(PositionNormalTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	clip(dc.a <= Oc ? -1 : 1);

	return light(0, dc, input.WorldPosition, input.Normal);
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

float4 PositionNormalTextureTwoPS(PositionNormalTextureTwoOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	//dc *= tex2D(DtSampler, input.TextureCoordinateTwo);
	clip(dc.a <= Oc ? -1 : 1);

	return light(0, dc, input.WorldPosition, input.Normal);
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
