//-----------------------------------------------------------------
//
//	@brief		Mobile Diffuse Vertex Lit Shader
//
//	@author		Benedikt Niesen (benedikt@weltenbauer-se.com)
//
//	@date		June 2013
//
//-----------------------------------------------------------------
//
// Standard Mobile Diffuse Vertex Lit Shader 
//
//-----------------------------------------------------------------

Shader "WbMobileColoredVertexLit" {

//-----------------------------------------------------------------
// Properties
//-----------------------------------------------------------------
	
	Properties {
		
		// - Color
		_MainColor ("Diffuse Color", Color) = (1,1,1,1)
		
		// - Textures
		
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
				
		LOD 100 // Maybe different LODs for different Devices, can be set in Script
		
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
			
		//-----------------------------------------------------------------
		// CG Includes
			
			#include "WbShaderInclude.cginc"
			
		//-----------------------------------------------------------------
		// INPUTS

			// Color Inputs
			uniform fixed4 _MainColor;
			
			// Texture Inputs

			// Texture to UV Inputs
			
			// Control Inputs

	//-----------------------------------------------------------------
	// Structs
	//-----------------------------------------------------------------
			
		//-----------------------------------------------------------------
		// App To Vertex Struct
										
			struct a2v {
			
				float4 vertex	: POSITION;
				fixed3 normal	: NORMAL;
				fixed4 color	: COLOR;
			};

			//-----------------------------------------------------------------
			// Vertex to Fragment Struct
			
			struct v2f {
			
				float4 pos			: POSITION;
				fixed3 vertexlight	: TEXCOORD1;
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
				
				// - Output Vertex Light
				output.vertexlight.xyz = VertexLight(v.normal) * _MainColor.xyz * v.color.xyz;
				
				// - Return to Fragment Shader
				return output;
			};

			//-----------------------------------------------------------------
			// Fragment Shader
			
			fixed4 frag (v2f input) : COLOR	{

				// - Read Textures
				
				// - Define Variables
				fixed4 c;
                
                // - Compute Stuff
                c.rgb = input.vertexlight.rgb;
				c.a = 1.0;
				
				// - Output
				return c;
			};
			
			ENDCG 
			
		} // Pass End
	} // SubShader End
	
	//Commented out during Development
	//Fallback "Mobile/VertexLit" 
	
} // Shader End