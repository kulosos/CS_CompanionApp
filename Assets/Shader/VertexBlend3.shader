Shader "WbShader/Blend/Vertex Detail Blend 3" {

    Properties {
    
  		_Color   ("Diffuse Color", Color) = (1,1,1,1)
		
		_MainTex ("Splat Tex 01 (Base)", 2D) = "white" {}
		_MainTex2 ("Splat Tex 02 (R)", 2D) = "white" {}
		_MainTex3 ("Splat Tex 03 (R)", 2D) = "white" {}
		
		_scale ("UV Scale", Float) = 1.0
		_scaleSplat1 ("Splat1 Scale", Float) = 1.0
		_scaleSplat2 ("Splat2 Scale", Float) = 1.0
		_scaleSplat3 ("Splat3 Scale", Float) = 1.0

		_sharpness01 ("Splat1 Sharpness", Float) = 1.0
		_sharpness02 ("Splat2 Sharpness", Float) = 1.0
    }

    SubShader {
		  Tags { "RenderType" = "Opaque" }
		  Fog {Mode Off}
		CGPROGRAM
		#include "UnityCG.cginc"
	
		#pragma surface surf Lambert
		#pragma exclude_renderers xbox360 ps3 d3d11 d3d11_9x flash glesdesktop gles gles3
		#pragma glsl
		#pragma target 3.0


		uniform float4 _Color;
      
     
		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;

		float _scale;
		
		float _scaleSplat1;
		float _scaleSplat2;
		float _scaleSplat3;
		float _sharpness01;   
		float _sharpness02; 

			
	struct Input {
		half2 uv_MainTex;
		fixed4 color : COLOR;
	};	  
		    
      void surf (Input IN, inout SurfaceOutput o) {
      
      	fixed4 tex0 = tex2D(_MainTex, IN.uv_MainTex * _scaleSplat1);
      	fixed4 tex1 = tex2D(_MainTex2, IN.uv_MainTex * _scaleSplat2);
		fixed4 tex2 = tex2D(_MainTex3, IN.uv_MainTex * _scaleSplat3);
		
      	fixed blendFactor1;
     	fixed blendFactor2;
     	
      	blendFactor1 = saturate(pow(IN.color.r + (IN.color.r * tex1.a), _sharpness01));
      	blendFactor2 = saturate(pow(IN.color.g + (IN.color.g * tex2.a), _sharpness02));
      	
      	fixed3 tex0Blended = lerp(tex0.rgb, tex1.rgb, blendFactor1);
      	fixed3 tex1Blended = lerp(tex0Blended, tex2.rgb, blendFactor2);

		fixed4 diff = fixed4(tex1Blended, 1);
		// Diffuse Albedo
		o.Albedo = diff.rgb * _Color.rgb;
		o.Alpha = diff.a * _Color.a;
      }
      ENDCG
    } 
    FallBack "Specular"
}