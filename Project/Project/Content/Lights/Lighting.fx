sampler SceneSampler;

texture LightMask;
sampler lightSampler = sampler_state
{
    Texture = <LightMask>;
};


struct VertexShaderOutput
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
    float2 texCoords : TEXCOOR;
};

// Input color (which comes from vertex shader output) - color specified in SpriteBatch.Draw() method.
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    const float4 sceneColor = tex2D(SceneSampler, input.texCoords);
    const float4 lightColor = tex2D(lightSampler, input.texCoords);

    return (sceneColor * 0.05) + (sceneColor * lightColor*0.7);
}
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}