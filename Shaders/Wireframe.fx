float4x4 World;
float4x4 View;
float4x4 Projection;

float3 Dc = 1;

float4 VertexShaderFunction(in float4 input : POSITION0)
{
    float4 output;

    float4 worldPosition = mul(input, World);
    float4 viewPosition = mul(worldPosition, View);
    output = mul(viewPosition, Projection);

    return output;
}

float4 PixelShaderFunction(float4 input) : COLOR0
{
    return float4(Dc, 1);
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
