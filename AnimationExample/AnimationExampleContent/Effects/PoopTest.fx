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
    float4 pos : POSITION0;
	float4 texCoor : COLOR0;
};

struct VSBasicOutput
{
	float4 pos : POSITION0;
	float4 color : COLOR0;
};

VSBasicOutput VSBasic(VSBasicInput input)
{
    VSBasicOutput output;

    output.pos = mul(input.pos, worldProj);
	output.color = input.texCoor;

    return output;
}

float4 PSBasic(VSBasicOutput input) : COLOR0
{
	return float4(input.color);
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
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = VSArray[vsArrayIndex];
        PixelShader = PSArray[psArrayIndex];
		//VertexShader = compile vs_2_0 VSBasic();
        //PixelShader = compile ps_2_0 PSBasic();
    }
}
