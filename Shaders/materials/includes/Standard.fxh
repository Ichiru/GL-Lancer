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

void PositionNormalVS(in float4 inputPosition : POSITION0, in float3 inNormal : NORMAL0,
								   out float4 outPosition : POSITION0, out float3 outNormal: TEXCOORD0,
								   out float4 outWorldPosition: TEXCOORD1)
{
    float4 worldPosition = mul(inputPosition, World);
    float4 viewPosition = mul(worldPosition, View);
    outPosition = mul(viewPosition, Projection);

	outNormal = mul(inNormal, World);
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
									 out float4 outputPosition : POSITION0, out float2 outputTextureCoordinate: TEXCOORD0)
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

void PositionNormalTextureVS(in float4 inputPosition : POSITION0, in float3 inputNormal : NORMAL0,
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

//struct PositionNormalTextureTwoIn
//{
//    float4 Position : POSITION0;
//	float3 Normal : NORMAL0;
//	float2 TextureCoordinate : TEXCOORD0;
//	float2 TextureCoordinateTwo : TEXCOORD1;
//};

//struct PositionNormalTextureTwoOut
//{
//  float4 Position : POSITION0;
//	float3 Normal : TEXCOORD0;
//	float2 TextureCoordinate : TEXCOORD1;
//	float2 TextureCoordinateTwo : TEXCOORD2;
//	float3 WorldPosition : TEXCOORD3;
//};

void PositionNormalTextureTwoVS(in float4 inputPosition : POSITION0, in float3 inputNormal : NORMAL0,
								in float2 inputTextureCoordinate : TEXCOORD0, in float2 inputTextureCoordinateTwo : TEXCOORD1,
								out float4 outputPosition : POSITION0, out float3 outputNormal : TEXCOORD0,
								out float2 outputTextureCoordinate : TEXCOORD1, out float2 outputTextureCoordinateTwo : TEXCOORD2,
								out float3 outputWorldPosition : TEXCOORD3)
{
    float4 worldPosition = mul(inputPosition, World);
    float4 viewPosition = mul(worldPosition, View);
    outputPosition = mul(viewPosition, Projection);

	outputNormal = mul(inputNormal, World);
	outputTextureCoordinate = inputTextureCoordinate;
	outputTextureCoordinateTwo = inputTextureCoordinateTwo;
	outputWorldPosition = worldPosition;
}


// ------ PositionDiffuseTexture -----------------------------------------------------------

//struct PositionDiffuseTextureIn
//{
//    float4 Position : POSITION0;
//	float4 Diffuse : COLOR0;
//	float2 TextureCoordinate : TEXCOORD0;
//};

//struct PositionDiffuseTextureOut
//{
//    float4 Position : POSITION0;
//	float4 Diffuse : COLOR0;
//	float2 TextureCoordinate : TEXCOORD0;
//};

void PositionDiffuseTextureVS(in float4 inputPosition : POSITION0, in float4 inputDiffuse : COLOR0,
							 in float2 inputTextureCoordinate : TEXCOORD0, out float4 outputPosition : POSITION0,
							 out float4 outputDiffuse : COLOR0, out float2 outputTextureCoordinate : TEXCOORD0)
{
    float4 worldPosition = mul(inputPosition, World);
    float4 viewPosition = mul(worldPosition, View);
    outputPosition = mul(viewPosition, Projection);
	outputDiffuse = inputDiffuse;
	outputTextureCoordinate = inputTextureCoordinate;
}
