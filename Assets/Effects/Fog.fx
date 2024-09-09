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

float uColorMapSection;

//Extra Variables
float time;
float edgePower = 1.5;
float progressPower = 2.5;


float rand(float2 coords)
{
    return frac(sin(dot(coords, float2(56.3456f, 78.3456f)) * 5.0f) * 10000.0f);
}

float noise(float2 coords)
{
    float2 i = floor(coords);
    float2 f = frac(coords);

    float a = rand(i);
    float b = rand(i + float2(1.0f, 0.0f));
    float c = rand(i + float2(0.0f, 1.0f));
    float d = rand(i + float2(1.0f, 1.0f));

    float2 cubic = f * f * (3.0f - 2.0f * f);

    return lerp(a, b, cubic.x) + (c - a) * cubic.y * (1.0f - cubic.x) + (d - b) * cubic.x * cubic.y;
}

float fbm(float2 coords)
{
    float value = 0.0f;
    float scale = 0.5f;

    for (int i = 0; i < 5; i++)
    {
        value += noise(coords) * scale;
        coords *= 4.0f;
        scale *= 0.5f;
    }

    return value;
}

float value(float2 uv)
{
    float Pixels = 2048.0;
 //   Pixels *= 500.0;
    float dx = 10.0 * (1.0 / Pixels);
    float dy = 10.0 * (1.0 / Pixels);
  

    float final = 0.0f;
    
    float2 uvc = uv;
    
    float2 Coord = float2(dx * floor(uvc.x / dx),
                          dy * floor(uvc.y / dy));

    
    for (int i = 0; i < 3; i++)
    {
        float f = fbm(Coord + time * 0.05f + float2(i, i));
        float2 motion = float2(f, f);
        final += fbm(Coord + motion + float2(i, i));
    }

    return final / 3.0f;
}


float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    //Base Color
    /*
    float3 col = float3(0.0, 0.0, 0.0);
    float increment = 0.2;
    
    //Combine the texture several times over, move in sin/cos for circle-ish blending
    for (float f = 0.0; f < 1.0; f += increment)
    {
        float p = f / 1.0;
        float rot = p * 6.28;
        float2 offsetCoords = coords + float2(
            sin(rot) * time,
            cos(rot) * time);
        
        float colorMap = tex2D(uImage1, offsetCoords).r;
        col += colorMap;
    }
    col /= (1.0 / increment);
    col *= sampleColor.rgb;
   */
    
       
    float opacity = 0.6f;
    float black = -1.0;
    float white = 2.0;
    float3 col = float3(lerp(float3(black, black, black), float3(0.45, 0.4f, 0.7f) + float3(white, white, white), value(coords)));
  
    // Fade out the Edges
    float2 diff = coords - float2(0.5, 0.5);
    float l = length(diff);
    l = pow(l, edgePower);
    if (l > 0.5)
    {
        l = 0.5;
    }
    
    float p = l / 0.5;
    p = pow(p, progressPower);
    if (p > 1.0)
    {
        p = 1.0;
    }

    // Output to screen
    float4 targetCol = float4(col, 1.0);
    float4 transparentCol = float4(0.0, 0.0, 0.0, 0.0);
    float4 fragColor = targetCol * (1.0 - p) * sampleColor.a;
    return fragColor;
}

technique Technique1
{
    pass PixelPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}