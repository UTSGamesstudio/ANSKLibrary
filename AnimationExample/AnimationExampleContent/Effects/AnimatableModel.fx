#define SKINNED_EFFECT_MAX_BONES   72

float4x4 worldProj;
int vsArrayIndex;
int psArrayIndex;
float4x3 bones[SKINNED_EFFECT_MAX_BONES];
float ambientIntensity;
float4 diffuseColour;
float diffuseFactor;
float3 diffuseLight;
float4 specColour;
float3 specPos;
float4x4 world;

texture modelTexture;
sampler2D modelTextureSampler = sampler_state
{
	Texture = (modelTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VSBasicInput
{
    float4 pos : SV_Position;
	float4 colour : COLOR0;
	float2 texCoor : TEXCOORD0;
	float3 normal : NORMAL0;
	int4   indices  : BLENDINDICES0;
    float4 weights  : BLENDWEIGHT0;
	int boneCount : BLENDINDICES0;
};

struct VSBasicOutput
{
	float4 pos : SV_Position;
	float4 colour : COLOR0;
	float3 normal : TEXCOORD0;
	float3 view : TEXCOORD1;
	//float2 texCoor: TEXCOORD0;
};

void Skin(inout VSBasicInput vin)
{
    float4x3 skinning = 0;

    [unroll]
    //for (int i = 0; i < vin.boneCount; i++)
	for (int i = 0; i < 1; i++)
    {
        skinning += bones[vin.indices[i]] * vin.weights[i];
    }

    vin.pos.xyz = mul(vin.pos, skinning);
	vin.normal = mul(vin.normal, (float3x3)skinning);
}

VSBasicOutput VSBasic(VSBasicInput input)
{
    VSBasicOutput output;

	Skin(input);

    output.pos = mul(input.pos, worldProj);
	output.normal = normalize(mul(input.normal, world));
	output.colour = input.colour;
	output.view = normalize(float4(specPos, 1.0) - mul(input.pos, world));
	//output.pos = input.pos;
	//output.texCoor = input.texCoor;

    return output;
}

float4 PSBasic(VSBasicOutput input) : COLOR0
{
	//float4 texColour = tex2D(modelTextureSampler, input.texCoor);
	//texColour.a = 1;
	
	//return texColour;
	//return float4(1,1,0,1);
	return input.colour;
}

VSBasicOutput VSOutline(VSBasicInput input)
{
    VSBasicOutput output;

	Skin(input);

    output.pos = mul(input.pos, worldProj);

	float4 normal = mul(input.normal, worldProj);

	output.pos = output.pos + (mul(.03, normal));

	//output.pos = input.pos;
	//output.texCoor = input.texCoor;

    return output;
}

float4 PSOutline(VSBasicOutput input) : COLOR0
{
	//float4 texColour = tex2D(modelTextureSampler, input.texCoor);
	//texColour.a = 1;
	
	//return texColour;
	return float4(0,0,0,1);
	//return input.color;
}

float4 PSLambert(VSBasicOutput input) : COLOR0
{
	float4 norm = float4(input.normal, 1.0);
	float4 diffuse = saturate(dot(-diffuseLight, normalize(norm)));

	input.colour.rgb *= ambientIntensity + diffuseFactor * diffuseColour.rgb * diffuse;
	return input.colour;
}

float4 PSBlinn(VSBasicOutput input) : COLOR0
{
	float4 norm = float4(input.normal, 1.0);
	float4 diffuse = saturate(dot(-diffuseLight, normalize(norm)));
	float4 reflect = normalize(2*diffuse*norm-float4(diffuseLight,1.0));
	float4 specular = pow(saturate(dot(reflect, input.view)),15);

	input.colour.rgb *= ambientIntensity + diffuseFactor * diffuseColour.rgb * diffuse + specColour * specular;
	return input.colour;
}

VertexShader VSArray[1]=
{
	compile vs_2_0 VSBasic(),
};

PixelShader PSArray[2]=
{
	compile ps_2_0 PSLambert(),
	compile ps_2_0 PSBlinn(),
};

technique ANSK
{
	/*pass Pass0
	{
		VertexShader = compile vs_2_0 VSOutline();
		PixelShader = compile ps_2_0 PSOutline();
	}*/

    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = VSArray[vsArrayIndex];
        PixelShader = PSArray[psArrayIndex];
		//VertexShader = compile vs_2_0 VSBasic();
        //PixelShader = compile ps_2_0 PSBasic();
    }
}

technique ANSKLambert
{
	pass Pass1
	{
		VertexShader = VSArray[vsArrayIndex];
		PixelShader = PSArray[psArrayIndex];
	}
}
