Shader "Hidden/Stylized Fog"
{
    HLSLINCLUDE
    
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
    float4 FogColor;
    int UseStylizedFogTexture;
    TEXTURE2D_SAMPLER2D(StylizedFogTexture, sampler_StylizedFogTexture);
    float FogMinDistance;
    float FogMaxDistance;
    int IsExponential;
    float ExponentialDensity;
    float FogIntensity;
    float FogOpacityMax;
    
    float4 Frag(VaryingsDefault i): SV_Target
    {
        // Color of pixel to render
        float4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        
        // Get pixel depth and make linear fog calculations from it
        float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord);
        depth = Linear01Depth(depth);
        depth = clamp(depth * (_ProjectionParams.z / FogMaxDistance), 0, 1);
        
        // Get fog color from stylized texture based on depth for UV
        if (UseStylizedFogTexture)
            FogColor = SAMPLE_TEXTURE2D(StylizedFogTexture, sampler_StylizedFogTexture, float2(min(0.9, depth), 0));
        
        // Exponential fog
        if (IsExponential)
            depth = 1 / exp((1 - depth) * ExponentialDensity);
        
        return lerp(baseColor, FogColor, min(depth * FogIntensity, FogOpacityMax) * FogColor.a);
    }
    
    ENDHLSL
    
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            HLSLPROGRAM
            
            #pragma vertex VertDefault
            #pragma fragment Frag
            
            ENDHLSL
            
        }
    }
}