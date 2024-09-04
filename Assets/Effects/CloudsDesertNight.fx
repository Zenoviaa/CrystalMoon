sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

texture uColorMap;
float uColorMapSection;
sampler colorMap = sampler_state
{
    texture = <uColorMap>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};

float4 ColorMap(float strength)
{
    return tex2D(colorMap, float2(clamp(strength, 0.01, 0.99), uColorMapSection));
}


float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float time = uTime;
    float noise1 = tex2D(uImage1, coords * 4 + float2(time * -0.03, 0.0) + uImageOffset);
    float noise2 = tex2D(uImage1, coords * 3 + float2(time * -0.007 - 0.51, 0.0) + uImageOffset);
    float noise3 = tex2D(uImage1, coords * 2 + float2(time * -0.0039 + 0.83, 0.0) + uImageOffset);
    float noise = (noise1 + noise2 + noise3) * 0.4;
    
    float4 color = tex2D(uImage1, coords) * noise * float4(0.5, 0.5, 1.0, 1.0) * 0.4;
    float2 dist = coords - float2(0.5f, 0.0f);
    float magnitude = length(dist);
    
    float4 finalColor = float4(color.r, color.g, color.b, 0.0);
    float4 finalCColor = float4(ColorMap(noise).rgb, length(finalColor) / 3) * 0.28;
    
    return lerp(float4(0.0, 0.0, 0.0, 0.0), finalCColor, pow(abs(magnitude), 1.25));
}

technique Technique1
{
    pass ScreenPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}