float4 PositionNormalPS(float4 inputPosition : POSITION0, float3 inputNormal: TEXCOORD0, float3 inputWorldPosition: TEXCOORD1) : COLOR0
{
	float4 dc = float4(1, 0, 0, 1);
	return light(0, dc, inputWorldPosition, inputNormal);
}

technique PositionNormal
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

		VertexShader = compile vs_3_0 PositionNormalVS();
        PixelShader = compile ps_3_0 PositionNormalPS();
    }
}
