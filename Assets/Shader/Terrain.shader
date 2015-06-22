/*
 * brief	
 * author	Benedikt Niesen (benedikt@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * date		March 2015
 */

//-----------------------------------------------------------------------------

Shader "Terrain/Terrain" {
    
	//-----------------------------------------------------------------------------

	Properties {

        _HeightMap ("Texture", 2D) = "white" {}
        _SplatMap  ("_SplatMap", 2D) = "white" {}
		_GrasMap  ("_GrasMap", 2D) = "white" {}
        _Amount ("Extrusion Amount", Float) = 1
        _Height ("_Height Amount", Float) = 1

        _WorldSizeX ("_WorldSizeX Amount", Float) = 0
        _WorldSizeY ("_WorldSizeY Amount", Float) = 0
    }

	//-----------------------------------------------------------------------------

    SubShader {

        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        #pragma target 3.0
        #pragma glsl
      	
		//-----------------------------------------------------------------------------
        
		struct Input {
            float2 uv_HeightMap;
            half2 texcoordx : TEXCOORD2;
			half2 texcoordWorld : TEXCOORD3;
            fixed4 color : COLOR;
        };
      
		//-----------------------------------------------------------------------------

        float _Amount, _Height, _WorldSizeX, _WorldSizeY;
        int LOD;
		float4 _MAX, _MIN;
      
        sampler2D _HeightMap,_SplatMap, _GrasMap;
     
		//-----------------------------------------------------------------------------
        
		inline fixed3 UnpackNormalTerrain (fixed4 packednormal){
            fixed3 normal;
            normal.xy = packednormal.ba * 2 - 1;
            normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
            return normal;
        }

		//-----------------------------------------------------------------------------

        void vert (inout appdata_full v, out Input o) {
			
            UNITY_INITIALIZE_OUTPUT(Input,o);
			 
            float res = 1.0/1024.0;
            float4 worldPos = mul(_Object2World, v.vertex);
			half3 inverseLerp = (worldPos.xyz-_MIN.xyz)/(_MAX.xyz-_MIN.xyz) ;
            worldPos.xyz = clamp(worldPos.xyz, _MIN.xyz ,_MAX.xyz);
			
			float4 tex = tex2Dlod(_HeightMap, float4(inverseLerp.x, inverseLerp.z,0, LOD * v.color.r));//LOD * v.color.r));
            
			float h = (tex.r * 255.0 + tex.g) / 255 * _Height;
            
			o.texcoordx = half2(inverseLerp.x, inverseLerp.z);
			o.texcoordWorld = half2(worldPos.x, worldPos.z);
			worldPos.y = h-0.25;
			worldPos = mul(_World2Object, float4(worldPos));
			v.vertex.xyz = worldPos.xyz;
			
			v.color.g =  h;
			
        }

      	//-----------------------------------------------------------------------------

        void surf (Input IN, inout SurfaceOutput o) {

            fixed4 splat = tex2D(_SplatMap, IN.texcoordx.xy );
            fixed4 height = saturate(tex2D(_HeightMap, IN.texcoordx.xy));
			fixed4 gras = saturate(tex2D(_GrasMap, IN.texcoordWorld.xy*0.1));
            o.Albedo = 0.5;
            o.Normal = UnpackNormalTerrain(height);
        }
      
      	//-----------------------------------------------------------------------------
      
        ENDCG
    }

    Fallback "Diffuse"
}
