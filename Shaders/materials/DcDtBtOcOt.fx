#include "includes/Standard.fxh"

float3 Dc;

Texture Bt;
sampler BtSampler = 
	sampler_state
	{
		texture = <Bt>;
		magfilter = ANISOTROPIC;
		minfilter = ANISOTROPIC;
		mipfilter = LINEAR;
		AddressU = WRAP;
		AddressV = WRAP;
	};

float Oc;

#include "includes/BasicPosition.fxh"
#include "includes/BasicPositionNormal.fxh"
#include "includes/BasicPositionTexture.fxh"
#include "includes/BasicPositionNormalTexture.fxh"
#include "includes/BasicPositionNormalTextureTwo.fxh"
#include "includes/BasicPositionDiffuseTexture.fxh"
