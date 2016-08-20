﻿Shader "Custom/Rim/Texture Unlit Hologram" {
	Properties {
		_MainTex ("Main Texture", 2d) = "white" {}
    	_EmissionColor ("Emission Color", Color) = (1,1,1,1)
    	_EmissionIntensity ("Emission Intensity", Range(0.0,1.0)) =0.05
    	_RimColor ("Rim Color", Color) = (0.8,0.95,1.0,1.0)
    	_RimAngle ("Rim Angle", Range(0.0,4.0)) = 1.5
    	_RimIntensity ("Rim Intensity", Range(0.0,6.0)) = 1.5
	}
	SubShader {
		Pass {
		Tags { "RenderType"="Geometry"}
			
			CGPROGRAM
			#pragma noambient noforwardadd nofog
			#pragma vertex vert
			#pragma fragment frag
			
			//uniform
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _EmissionColor;
			uniform fixed _EmissionIntensity;
			uniform fixed4 _RimColor;
			uniform half _RimAngle;
			uniform half _RimIntensity;
			
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tex : TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 col : COLOR;
				float4 texcoord : TEXCOORD0;
			};
			
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				//Set position
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.texcoord=v.tex;
				
				//Calculate useful variables
				half3 normalDirection = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);
				half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - mul(_Object2World,v.vertex).xyz);
		
				//Calculate emission color
				half3 emissionColor=_EmissionColor*_EmissionIntensity;
				
				//Calculate diffuse color
				half3 diffuseColor=max(0.0,0.0+dot(normalDirection,half3(0,-1.0,0)));
				
				//Calculate rim color by doing dot with up vecotr
    			half3 rimAngleColor = 1.0 - saturate(dot (normalize(viewDirection), normalDirection));
    			rimAngleColor = diffuseColor*rimAngleColor;
    			
				rimAngleColor=pow(rimAngleColor,_RimAngle);
				half3 rimColor=_RimColor*_RimIntensity*rimAngleColor*_RimColor.xyzw;
				
				//Calculate final color
				o.col =float4(emissionColor+saturate(rimColor),1.0);
				return o;
			}
			
			float4 frag(vertexOutput o) : COLOR
			{	
				half4 mainTex = tex2D(_MainTex,_MainTex_ST.xy*o.texcoord.xy + _MainTex_ST.zw);
				return o.col*mainTex;
			}
			ENDCG
		}
	}
}