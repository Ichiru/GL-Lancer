float4 PositionDiffuseTexturePS(in float4 inputPosition : POSITION0, in float4 inputDiffuse : COLOR0,
							    in float2 inputTextureCoordinate : TEXCOORD0) : COLOR0
{
	float4 result = tex2D(DtSampler, inputTextureCoordinate);
	result.rgb *= inputDiffuse * float3(1, 0, 0);

	return result;
}

technique PositionDiffuseTexture
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionDiffuseTextureVS();
        PixelShader = compile ps_3_0 PositionDiffuseTexturePS();
    }
}
