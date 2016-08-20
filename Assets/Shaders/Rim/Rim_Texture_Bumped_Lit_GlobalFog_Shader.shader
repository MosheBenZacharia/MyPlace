﻿Shader "Custom/Rim/Texture Bumped Lit Global Fog" {
	Properties {
    	_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Emission", 2D) = "white" {}
		_BumpTex ("Normal", 2D) = "white" {}
    	_EmissionColor ("Emission Color", Color) = (1,1,1,1)
    	_EmissionIntensity ("Emission Intensity", Range(0.0,1.0)) =1.0
    	_RimColor ("Rim Color", Color) = (0.8,0.95,1.0,1.0)
    	_RimAngle ("Rim Angle", Range(0.0,4.0)) = 1.5
    	_RimIntensity ("Rim Intensity", Range(0.0,3.0)) = 1.5
    	_LightAngle ("Light Angle", Range(0.0,2.0)) = 0.6
		_Distance("Distance",  Range(0,1000)) = 200
		_ReflectCube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
	}
	SubShader {
		Pass{
			Tags { "RenderType"="Geometry" "LightMode"="ForwardBase"}
			
		
			CGPROGRAM
			#pragma approxview noambient noshadow nolightmap nodynlightmap nodirlightmap nometa exclude_path:prepass
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			
			//uniform
			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform fixed4 _MainTex_ST;
			uniform sampler2D _BumpTex;
			uniform fixed4 _BumpTex_ST;
			uniform fixed4 _EmissionColor;
			uniform fixed _EmissionIntensity;
			uniform fixed4 _RimColor;
			uniform half _RimAngle;
			uniform half _RimIntensity;
			uniform half _LightAngle;
			
			uniform samplerCUBE _ReflectCube;
		   	uniform  float _Distance;
	    
			float lengthFloatThree(float3 v)
			{
  				return sqrt(dot(v,v));
			}
		
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tex : TEXCOORD0;
				float4 tangent : TANGENT;
			};
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 col : COLOR;
				float4 texcoord : TEXCOORD0;
				float3 tangentSpaceLightDir : TEXCOORD1;
				float4 I : TEXCOORD2;
			};
			
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				
				//Calculate useful variables
				half3 normalDirection = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);
				half3 lightDirection= normalize(_WorldSpaceLightPos0.xyz);
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				
				float3 viewDirection = _WorldSpaceCameraPos.xyz - mul(_Object2World,v.vertex).xyz;//not normalized
				o.I.a = lengthFloatThree(viewDirection);//camera distance stored in alpha
				o.I.rgb = -normalize(viewDirection).rgb;
				
				
                float3 binormal = cross( normalize(v.normal), normalize(v.tangent.xyz) );
                float3x3 rotation = float3x3( v.tangent.xyz, binormal, v.normal );
               
				//Calculate diffuse color
				half3 diffuseColor=saturate(_LightAngle+dot(normalDirection,lightDirection));
				
               	//Send variables to frag
                o.tangentSpaceLightDir = mul(rotation, viewDir);
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.texcoord=v.tex;
				o.col=float4(diffuseColor,1.0);
				
				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{	
				half3 viewDirection=i.tangentSpaceLightDir;
				
				half atten=1.0;
			
				//textures
				fixed4 tex = tex2D(_MainTex,_MainTex_ST.xy*i.texcoord.xy + _MainTex_ST.zw);
				fixed4 texN = tex2D(_BumpTex,_BumpTex_ST.xy*i.texcoord.xy + _BumpTex_ST.zw);
				
				
             	half3 normalDirection = (texN.rgb * 2.0) - 1.0;
				
				//rim
				half rim = 1 - saturate(dot(normalize(viewDirection),normalDirection));
				half3 rimLighting= _RimColor.rgb * pow(rim,_RimAngle);
				rimLighting*=_RimIntensity;
				
				
				fixed4 reflcol = texCUBE( _ReflectCube, i.I );
				float objectDistance = abs(i.I.a);
				objectDistance = pow(objectDistance,2.0);
				half fogStrength = clamp(objectDistance/(_Distance*800.0),0,1);
				
				//final
				half3 texFinal=tex.rgb;
				half3 rimFinal = rimLighting;
				half3 emissionFinal = _EmissionColor*_EmissionIntensity;
				half3 lightFinal=i.col*_Color+emissionFinal;
				
				fixed3 colorFinal=texFinal*lightFinal+rimFinal*i.col;
				
				colorFinal = lerp(colorFinal,reflcol,fogStrength);
				
				return float4(colorFinal,1.0);
			}
				
			
			ENDCG
		} 
	}	
}