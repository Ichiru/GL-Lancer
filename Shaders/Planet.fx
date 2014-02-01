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


struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float3 TextureCoordinate : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float3 WorldPosition : TEXCOORD2;
};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.TextureCoordinate = input.Position.xyz;
	output.Normal = mul(normalize(input.Position.xyz), World);
	output.WorldPosition = worldPosition.xyz;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 dc = texCUBE(PlanetSampler, input.TextureCoordinate);

	return light(0, dc, input.WorldPosition, input.Normal);
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
