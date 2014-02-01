float4 PositionDiffuseTexturePS(PositionDiffuseTextureOut input) : COLOR0
{
	float4 result = tex2D(DtSampler, input.TextureCoordinate);
	result.rgb *= input.Diffuse * float3(1, 0, 0);

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
