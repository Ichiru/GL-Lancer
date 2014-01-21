float4 AmbientColor = float4(1.0f, 1.0f, 1.0f, 1.0f);

//struct LightSource
//{
//	 float3 Pos;
//     float3 Rotate;
//     float4 Color;
//     int Range;
     //bool DynamicDirection;
//     float3 Attenuation;
     //float3 Direction;
//};

int LightCount = 0;
//LightSource Lights[9];
float3 LightsPos[9];
float3 LightsRotate[9];
float4 LightsColor[9];
int LightsRange[9];
float3 LightsAttenuation[9];
float4 light(float4 ec, float4 dc, float3 position, float3 normal)
{
	//float4 light = 0;
	float4 light = AmbientColor;
	
	for (int i = 0; i < LightCount; i++)
	{
		int dist = distance(LightsPos[i], position);
		if (LightsRange[i] >= dist)
		{
			float lightAttenuation = clamp(0, 1, 1 / (LightsAttenuation[i].x + LightsAttenuation[i].y * dist + LightsAttenuation[i].z * dist * dist));
			float3 lightDirection = normalize(LightsPos[i] - position);
			float lightAngle = max(0, dot(lightDirection, normal));
			light += lightAttenuation * lightAngle * LightsColor[i];
		}
	}

	//return ec + (dc * AmbientColor) + (dc * saturate(light));
	return ec + (dc * saturate(light));
}
