float4 PositionPS(float4 input : POSITION0) : COLOR0
{
    return float4(float3(1, 0, 0) * AmbientColor, 1);
}

technique Position
{
    pass Pass1
    {
		SrcBlend = one;
		DestBlend = zero;
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = cw;

        VertexShader = compile vs_3_0 PositionVS();
        PixelShader = compile ps_3_0 PositionPS();
    }
}
