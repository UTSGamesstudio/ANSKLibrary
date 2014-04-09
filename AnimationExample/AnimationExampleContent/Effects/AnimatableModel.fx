#define SKINNED_EFFECT_MAX_BONES   72

float4x4 worldProj;
int vsArrayIndex;
int psArrayIndex;
float4x3 bones[SKINNED_EFFECT_MAX_BONES];

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
	//float4 color : COLOR0;
	float2 texCoor : TEXCOORD0;
	float3 normal : NORMAL0;
	int4   indices  : BLENDINDICES0;
    float4 weights  : BLENDWEIGHT0;
	int boneCount : BLENDINDICES5;
};

struct VSBasicOutput
{
	float4 pos : SV_Position;
	//float2 texCoor: TEXCOORD0;
};

void Skin(inout VSBasicInput vin)
{
    float4x3 skinning = 0;

    [unroll]
    //for (int i = 0; i < vin.boneCount; i++)
	for (int i = 0; i < 1; i++)
    {
        skinning += bones[vin.indices[i]]; //* vin.weights[i];
    }

    vin.pos.xyz = mul(vin.pos, skinning);
}

VSBasicOutput VSBasic(VSBasicInput input)
{
    VSBasicOutput output;

	Skin(input);

    output.pos = mul(input.pos, worldProj);
	//output.pos = input.pos;
	//output.texCoor = input.texCoor;

    return output;
}

float4 PSBasic(VSBasicOutput input) : COLOR0
{
	//float4 texColour = tex2D(modelTextureSampler, input.texCoor);
	//texColour.a = 1;
	
	//return texColour;
	return float4(1,1,0,1);
	//return input.color;
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

VertexShader VSArray[1]=
{
	compile vs_2_0 VSBasic(),
};

PixelShader PSArray[1]=
{
	compile ps_2_0 PSBasic(),
};

technique AnimatableModelTechnique
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
