//-----------------------------------------------------------------
//
//	@brief		Vertex Color Select Shader
//
//	@author		Benedikt Niesen (benedikt@weltenbauer-se.com)
//
//	@date		June 2013
//
//-----------------------------------------------------------------
//
// Switch between 4 Textures based on Vertex Colors
//
//-----------------------------------------------------------------

Shader "WbMobileDiffuseVertexColorSelectNoLightmap" {

//-----------------------------------------------------------------
// Properties
//-----------------------------------------------------------------
	
	Properties {
		
		// - Color
		_MainColor ("Diffuse Color", Color) = (1,1,1,1)
		
		// - Textures
		_MainTex ("Main Tex", 2D) = "white" {}
		
		// - Controls
	}

//-----------------------------------------------------------------
// SubShader
//-----------------------------------------------------------------

	SubShader {
	
	//-----------------------------------------------------------------
	// SubShader Tags

		Tags {	"Queue"="Geometry"
				"RenderType"="Opaque"
				"IgnoreProjector"="True"
				"ForceNoShadowCasting"="True"
				"LightMode"="ForwardBase" }
				// Possible to set LightMode to : Vertex
				
		LOD 250 // Maybe different LODs for different Devices, can be set in Script
		
	//-----------------------------------------------------------------
	// First Pass
	//-----------------------------------------------------------------
		
		Pass {
		
		//-----------------------------------------------------------------
		// Per Pass Tags and Lighting, Cull, ...
			
			Tags {} 
			Lighting On
			Fog { Mode Off }
			ZWrite On
			
		//-----------------------------------------------------------------
		// CG Program with renderer excludes
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma glsl_no_auto_normalization
			
		//-----------------------------------------------------------------
		// Render Targets
			
			// - For development use this line
			#pragma exclude_renderers xbox360 ps3 d3d11 d3d11_9x flash glesdesktop
			
			// - For final Build use this line
			//#pragma only_renderers gles
			
		//-----------------------------------------------------------------
		// Multi Compile for different Plattforms
			
			#pragma multi_compile NO_LIGHTMAP IS_LIGHTMAP
			#pragma multi_compile IS_WEAK IS_STRONG

		//-----------------------------------------------------------------
		// CG Includes
			
			#include "WbShaderInclude.cginc"
			
		//-----------------------------------------------------------------
		// INPUTS

			// Color Inputs
			fixed4 _MainColor;
			
			// Texture Inputs
			sampler2D _MainTex;
			
			// Texture to UV Inputs
			fixed4 _MainTex_ST;
			
			// Control Inputs
		
	//-----------------------------------------------------------------
	// Structs
	//-----------------------------------------------------------------
			
		//-----------------------------------------------------------------
		// App To Vertex Struct
										
			struct a2v {
			
				float4 vertex	: POSITION;
				fixed3 normal	: NORMAL;
				half2 texcoord	: TEXCOORD0;
				fixed4 color	: COLOR;
			};
			
		//-----------------------------------------------------------------
		// Vertex to Fragment Struct
			
			struct v2f {
			
				float4 pos			: POSITION;
				half2 uv1			: TEXCOORD0;
				fixed3 vertexlight	: TEXCOORD3;
				fixed4 color		: COLOR;
			};
			
	//-----------------------------------------------------------------
	// Shader
	//-----------------------------------------------------------------
			
		//-----------------------------------------------------------------
		// Vertex Shader
			
			v2f vert (a2v v) {
			
				v2f output;
				
				// - Output Position
				output.pos = mul(UNITY_MATRIX_MVP,v.vertex);

		        // - Output UV Coordinates
				output.uv1 = v.texcoord.xy;
				
				// - Output Vertex Light
				output.vertexlight = _MainColor.xyz * VertexLight(v.normal) * v.color.a;
				
				// - Output Vertex Color
				output.color = v.color;
				
				// - Return to Fragment Shader
				return output;
			};
				
		//-----------------------------------------------------------------
		// Fragment Shader
				
			fixed4 frag (v2f input) : COLOR	{
			
				// - Read Textures
				fixed4 tex1 = tex2D (_MainTex, input.uv1);
				
				// - Define Variables
				fixed4 c;
				
                c.rgb = tex1.r * input.color.r  + tex1.g * input.color.g  + tex1.b * input.color.b;	
                
                c.rgb *= input.vertexlight;

				c.a = 0.0;
				
				// - Output
				return c;
			};
			
			ENDCG 
			
		} // Pass End
		
	} // SubShader End
	
	//Commented out during Development
	//Fallback "Mobile/VertexLit" 
	
} // Shader End