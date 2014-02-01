#include "includes/Standard.fxh"

float3 Dc;
float3 Ac;
float Alpha;
float Fade;
float Scale;

// ------ PositionNormalTexture -----------------------------------------------------------

void PositionTextureAtmosphereVS(in float4 inputPosition : POSITION0, in float3 inputNormal : NORMAL0,
								 in float2 inputTextureCoordinate: TEXCOORD0, out float4 outputPosition : POSITION0,
								 out float3 outputNormal : TEXCOORD0, out float2 outputTextureCoordinate : TEXCOORD1,
								 out float3 outputWorldPosition : TEXCOORD2)
{

    float4 worldPosition = mul(inputPosition, World);
    float4 viewPosition = mul(worldPosition, View);
    outputPosition = mul(viewPosition, Projection);

	outputNormal = mul(normalize(inputPosition), World);
	outputTextureCoordinate = inputTextureCoordinate;
	outputWorldPosition = worldPosition;
	
}

float4 PositionTexturePS(in float4 inputPosition : POSITION0, in float3 inputNormal : TEXCOORD0,
					     in float2 inputTextureCoordinate : TEXCOORD1, in float3 inputWorldPosition : TEXCOORD2) : COLOR0
{
	float4 result = tex2D(DtSampler, inputTextureCoordinate);
	result.rgb *= Dc * Ac;
	result.a *= Alpha;

	//float3 viewDirection = normalize(inputWorldPosition - CameraPosition);
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
