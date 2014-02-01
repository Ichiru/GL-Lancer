float4x4 World;
float4x4 View;
float4x4 Projection;

float3 CameraPosition;

#include "includes/Light.fxh"

texture Dt;
sampler DtSampler = 
	sampler_state
	{
		texture = <Dt>;
		magfilter = ANISOTROPIC;
		minfilter = ANISOTROPIC;
		mipfilter = LINEAR;
		AddressU = WRAP;
		AddressV = WRAP;
	};


// ------ Position -----------------------------------------------------------

float4 PositionVS(float4 input : POSITION0) : POSITION0
{
    float4 worldPosition = mul(input, World);
    float4 viewPosition = mul(worldPosition, View);
    return mul(viewPosition, Projection);
}


// ------ PositionNormal -----------------------------------------------------------

//struct PositionNormalIn
//{
//   float4 Position : POSITION0;
//	float3 Normal : NORMAL0;
//};

//struct PositionNormalOut
//{
    //float4 Position : POSITION0;
	//float3 Normal : TEXCOORD0;
	//float3 WorldPosition : TEXCOORD1;
//};

void PositionNormalVS(in float4 inputPosition : POSITION0, in float4 inNormal : NORMAL0,
								   out float4 outPosition : POSITION0, out float3 outNormal: TEXCOORD0,
								   out float4 outWorldPosition: TEXCOORD1)
{
    float4 worldPosition = mul(inputPosition, World);
    float4 viewPosition = mul(worldPosition, View);
    outPosition = mul(viewPosition, Projection);

	outNormal = mul(inputNormal, World);
	outWorldPosition = worldPosition;
}


// ------ PositionTexture -----------------------------------------------------------

//struct PositionTextureIn
//{
//    float4 Position : POSITION0;
//	float2 TextureCoordinate : TEXCOORD0;
//};

//struct PositionTextureOut
//{
//    float4 Position : POSITION0;
//	float2 TextureCoordinate : TEXCOORD0;
//};

void PositionTextureVS(in float4 inputPosition : POSITION0, in float2 inputTextureCoordinate : TEXCOORD0,
									 out float4 outputPosition : POSITION0, out float2 outTextureCoordinate: TEXCOORD0)
{
    float4 worldPosition = mul(inputPosition, World);
    float4 viewPosition = mul(worldPosition, View);
    outputPosition = mul(viewPosition, Projection);
	outputTextureCoordinate = inputTextureCoordinate;
}

// ------ PositionNormalTexture -----------------------------------------------------------

//struct PositionNormalTextureIn
//{
//    float4 Position : POSITION0;
//	float3 Normal : NORMAL0;
//	float2 TextureCoordinate : TEXCOORD0;
//};

//struct PositionNormalTextureOut
//{
//    float4 Position : POSITION0;
//	float3 Normal : TEXCOORD0;
//	float2 TextureCoordinate : TEXCOORD1;
//	float3 WorldPosition : TEXCOORD2;
//};

PositionNormalTextureOut PositionNormalTextureVS(in float4 inputPosition : POSITION0, in float3 inputNormal : NORMAL0,
												 in float2 inputTextureCoordinate: TEXCOORD0, out float4 outputPosition : POSITION0,
												 out float3 outputNormal : TEXCOORD0, out float2 outputTextureCoordinate : TEXCOORD1,
												 out float3 outputWorldPosition : TEXCOORD2)
{
    float4 worldPosition = mul(inputPosition, World);
    float4 viewPosition = mul(worldPosition, View);
    outputPosition = mul(viewPosition, Projection);

	outputNormal = mul(inputNormal, World);
	outputTextureCoordinate = inputTextureCoordinate;
	outputWorldPosition = worldPosition;
}


// ------ PositionNormalTextureTwo -----------------------------------------------------------

struct PositionNormalTextureTwoIn
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
	float2 TextureCoordinateTwo : TEXCOORD1;
};

struct PositionNormalTextureTwoOut
{
    float4 Position : POSITION0;
	float3 Normal : TEXCOORD0;
	float2 TextureCoordinate : TEXCOORD1;
	float2 TextureCoordinateTwo : TEXCOORD2;
	float3 WorldPosition : TEXCOORD3;
};

PositionNormalTextureTwoOut PositionNormalTextureTwoVS(PositionNormalTextureTwoIn input)
{
    PositionNormalTextureTwoOut output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.Normal = mul(input.Normal, World);
	output.TextureCoordinate = input.TextureCoordinate;
	output.TextureCoordinateTwo = input.TextureCoordinateTwo;
	output.WorldPosition = worldPosition;
	
	return output;
}


// ------ PositionDiffuseTexture -----------------------------------------------------------

struct PositionDiffuseTextureIn
{
    float4 Position : POSITION0;
	float4 Diffuse : COLOR0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct PositionDiffuseTextureOut
{
    float4 Position : POSITION0;
	float4 Diffuse : COLOR0;
	float2 TextureCoordinate : TEXCOORD0;
};

PositionDiffuseTextureOut PositionDiffuseTextureVS(PositionDiffuseTextureIn input)
{
    PositionDiffuseTextureOut output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.Diffuse = input.Diffuse;
	output.TextureCoordinate = input.TextureCoordinate;
	
	return output;
}
