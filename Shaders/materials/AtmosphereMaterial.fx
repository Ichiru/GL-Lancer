#include "includes/Standard.fxh"

float3 Dc;
float3 Ac;
float Alpha;
float Fade;
float Scale;

// ------ PositionNormalTexture -----------------------------------------------------------

PositionNormalTextureOut PositionTextureAtmosphereVS(PositionTextureIn input)
{
    PositionNormalTextureOut output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.Normal = mul(normalize(input.Position), World);
	output.TextureCoordinate = input.TextureCoordinate;
	output.WorldPosition = worldPosition;
	
	return output;
}

float4 PositionTexturePS(PositionNormalTextureOut input) : COLOR0
{
	float4 result = tex2D(DtSampler, input.TextureCoordinate);
	result.rgb *= Dc * Ac;
	result.a *= Alpha;

	//float3 viewDirection = normalize(input.WorldPosition - CameraPosition);
	//float viewAngle = dot(-viewDirection, input.Normal);
	//result *=  Fade - viewAngle;
	
	return lerp(float4(Ac, Alpha), result, Scale);
}

technique PositionTexture
{
    pass Pass1
    {	
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = one;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionTextureAtmosphereVS();
        PixelShader = compile ps_3_0 PositionTexturePS();
    }
}

#include "includes/BasicPosition.fxh"
#include "includes/BasicPositionNormal.fxh"
#include "includes/BasicPositionNormalTexture.fxh"
#include "includes/BasicPositionNormalTextureTwo.fxh"
#include "includes/BasicPositionDiffuseTexture.fxh"
