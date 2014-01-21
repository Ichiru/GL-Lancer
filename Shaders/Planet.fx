/* The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * The Original Code is Starchart code (http://flapi.sourceforge.net/).
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

float4x4 World;
float4x4 View;
float4x4 Projection;

#include "materials/includes/Light.fxh"

Texture PlanetTexture;
sampler PlanetSampler =
	sampler_state
	{
		texture = <PlanetTexture>;
		magfilter = LINEAR;
		minfilter = LINEAR;
		mipfilter = LINEAR;
		AddressU = CLAMP;
		AddressV = CLAMP;
	};


void VertexShaderFunction(in float4 Position : POSITION0, in float2 TextureCoordinate : TEXCOORD0, out float4 OutPosition : POSITION0, out float3 OutTextureCoordinate : TEXCOORD0, out float3 OutNormal : TEXCOORD1, out float3 OutWorldPosition : TEXCOORD2)
{
    float4 worldPosition = mul(Position, World);
    float4 viewPosition = mul(worldPosition, View);
    OutPosition = mul(viewPosition, Projection);

	OutTextureCoordinate = Position.xyz;
	OutNormal = mul(normalize(Position.xyz), World);
	OutWorldPosition = worldPosition.xyz;
}

float4 PixelShaderFunction(in float4 Position : POSITION0, in float3 TextureCoordinate : TEXCOORD0, in float3 Normal : TEXCOORD1, in float3 WorldPosition : TEXCOORD2) : COLOR0
{
	float4 dc = texCUBE(PlanetSampler, TextureCoordinate);

	return light(0, dc, WorldPosition, Normal);
}

technique Technique1
{
	pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
