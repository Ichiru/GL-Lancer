float4 PositionNormalPS(PositionNormalOut input) : COLOR0
{
	float4 dc = float4(1, 0, 0, 1);
	return light(0, dc, input.WorldPosition, input.Normal);
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
