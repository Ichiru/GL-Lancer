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

float4 PositionNormalPixelShaderFunction(PositionNormalOut input) : COLOR0
{
	return light(0, Dc, input.WorldPosition, input.Normal);
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

float4 PositionTexturePixelShaderFunction(PositionTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
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

float4 PositionNormalTexturePixelShaderFunction(PositionNormalTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	dc *= Dc;

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
		CullMode = none;

		VertexShader = compile vs_3_0 PositionNormalTextureVS();
        PixelShader = compile ps_3_0 PositionNormalTexturePixelShaderFunction();
    }
}

// ------ PositionNormalTextureTwo -----------------------------------------------------------

float4 PositionNormalTextureTwoPixelShaderFunction(PositionNormalTextureTwoOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	//dc *= tex2D(DtSampler, input.TextureCoordinateTwo);
	dc *= Dc;

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
		CullMode = none;

		VertexShader = compile vs_3_0 PositionNormalTextureTwoVS();
        PixelShader = compile ps_3_0 PositionNormalTextureTwoPixelShaderFunction();
    }
}

// ------ PositionDiffuseTexture -----------------------------------------------------------

float4 PositionDiffuseTexturePixelShaderFunction(PositionDiffuseTextureOut input) : COLOR0
{
	float4 dc = tex2D(DtSampler, input.TextureCoordinate);
	dc *= Dc;
	dc *= input.Diffuse;

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
