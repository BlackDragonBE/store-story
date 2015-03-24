// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

// Shader created with Shader Forge v1.03 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.03;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:2,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:True;n:type:ShaderForge.SFN_Final,id:3475,x:32719,y:32712,varname:node_3475,prsc:2|emission-9260-OUT;n:type:ShaderForge.SFN_Tex2d,id:2155,x:32276,y:32700,ptovrint:False,ptlb:Base ,ptin:_Base,varname:node_2155,prsc:2,tex:e0c704b69021fa74d9e2add768cbbe75,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:4320,x:32196,y:32958,ptovrint:False,ptlb:Fresnel,ptin:_Fresnel,varname:node_4320,prsc:2,glob:False,v1:3;n:type:ShaderForge.SFN_Add,id:9260,x:32521,y:32717,varname:node_9260,prsc:2|A-2155-RGB,B-2282-OUT;n:type:ShaderForge.SFN_Fresnel,id:9560,x:32338,y:32958,varname:node_9560,prsc:2|EXP-4320-OUT;n:type:ShaderForge.SFN_Multiply,id:2282,x:32468,y:32992,varname:node_2282,prsc:2|A-9560-OUT,B-8626-RGB;n:type:ShaderForge.SFN_Color,id:8626,x:32338,y:33167,ptovrint:False,ptlb:Fresnel Color,ptin:_FresnelColor,varname:node_8626,prsc:2,glob:False,c1:0.8014706,c2:0.8014706,c3:0.8014706,c4:1;proporder:2155-4320-8626;pass:END;sub:END;*/

Shader "Shader Forge/People" {
    Properties {
        _Base ("Base ", 2D) = "white" {}
        _Fresnel ("Fresnel", Float ) = 3
        _FresnelColor ("Fresnel Color", Color) = (0.8014706,0.8014706,0.8014706,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 2.0
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 3x3 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither3x3( float value, float2 sceneUVs ) {
                float3x3 mtx = float3x3(
                    float3( 3,  7,  4 )/10.0,
                    float3( 6,  1,  9 )/10.0,
                    float3( 2,  8,  5 )/10.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,3);
                int ySmp = fmod(px.y,3);
                float3 xVec = 1-saturate(abs(float3(0,1,2) - xSmp));
                float3 yVec = 1-saturate(abs(float3(0,1,2) - ySmp));
                float3 pxMult = float3( dot(mtx[0],yVec), dot(mtx[1],yVec), dot(mtx[2],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            // float4 unity_LightmapST;
            #ifdef DYNAMICLIGHTMAP_ON
                // float4 unity_DynamicLightmapST;
            #endif
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform float _Fresnel;
            uniform float4 _FresnelColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                #ifndef LIGHTMAP_OFF
                    float4 uvLM : TEXCOORD4;
                #else
                    float3 shLight : TEXCOORD4;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = float4( o.pos.xy / o.pos.w, 0, 0 );
                o.screenPos.y *= _ProjectionParams.x;
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _Base_var = tex2D(_Base,TRANSFORM_TEX(i.uv0, _Base));
                float3 emissive = (_Base_var.rgb+(pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel)*_FresnelColor.rgb));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
