float4 PositionTexturePS(in float4 inputPosition : POSITION0, in float2 inputTextureCoordinate : TEXCOORD0) : COLOR0
{
	float4 result = tex2D(DtSampler, inputTextureCoordinate);
	result.rgb *= AmbientColor * float3(1, 0, 0);
	
	return result;
}

technique PositionTexture
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionTextureVS();
        PixelShader = compile ps_3_0 PositionTexturePS();
    }
}
