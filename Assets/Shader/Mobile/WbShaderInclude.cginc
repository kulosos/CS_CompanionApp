//-----------------------------------------------------------------
//
//	@brief		Shader Include File
//
//	@author		Benedikt Niesen (benedikt@weltenbauer-se.com)
//
//	@date		June 2013
//
//-----------------------------------------------------------------

//-----------------------------------------------------------------
// Unity CG Include

#include "UnityCG.cginc"
//#include "TerrainEngine.cginc"


//-------------------------------------------------------------------------------------------------
//
// Uniforms
//
//-------------------------------------------------------------------------------------------------

uniform float4 _LightColor0;

//-------------------------------------------------------------------------------------------------
//
// Vertex Functions
//
//-------------------------------------------------------------------------------------------------

//-------------------------------------------------------------------------------------------------
// Light Calculations

// Vertex Light

inline fixed3 VertexLight(fixed3 normal) {	
	fixed attenuation;
	attenuation = 2.0;
	fixed3 lightDirection;

	lightDirection = normalize(_WorldSpaceLightPos0.xyz);
	//lightDirection = normalize(mul(unity_LightPosition[0],UNITY_MATRIX_IT_MV).xyz);
	
	fixed3 normalDirction = normalize(fixed3(mul(fixed4(normal, 0.0), _World2Object)));//normalize(normal);
	
	fixed3 diffuseLight =  _LightColor0.xyz * max(dot(normalDirction,lightDirection),0);
	//fixed3 diffuseLight =  unity_LightColor[0].xyz * max(dot(normalDirction,lightDirection),0);

	return fixed3(diffuseLight * attenuation + (UNITY_LIGHTMODEL_AMBIENT.xyz * attenuation));
};

// Rim Light

inline fixed RimLight(fixed3 normal, float4 vertex, half rimWidth) {	

	float3 viewDir = normalize(ObjSpaceViewDir(vertex));
	float dotProduct = 1 - abs(dot(normal,viewDir));
	
	return fixed(smoothstep(1 - rimWidth, 1.0, dotProduct));
};

//-------------------------------------------------------------------------------------------------
// UV Calculations

// Compute UV Coordinates

half2 WorldUvTop(float4 vertex, half WorldSize) {

	float2 worldPos = mul (_Object2World, vertex).xz;
	return half2(worldPos.x + 750f, worldPos.y + 750f) / half2(WorldSize, WorldSize);
};

//-------------------------------------------------------------------------------------------------
// Vertex Calculations

// Vertex Animation for Fake Mobile Trees

float4 SmoothCurve(float4 x) {

	return x * x * (3.0 - 2.0 * x);
}
float4 TriangleWave(float4 x) {

	return abs( frac( x + 0.5 ) * 2.0 - 1.0);
}
float4 SmoothTriangleWave(float4 x) {

	return SmoothCurve( TriangleWave( x ) );
}

float4 AnimateVertex2(float4 pos, fixed3 normal, float4 animParams,float4 wind,float2 time){	

	float fDetailAmp = 0.1f;
	float fBranchAmp = 0.3f;
	
	float fObjPhase = dot(_Object2World[3].xyz, 1);
	float fBranchPhase = fObjPhase + animParams.x;
	
	float fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);
	
	float2 vWavesIn = time  + float2(fVtxPhase, fBranchPhase );
	
	float4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0);
	
	vWaves = SmoothTriangleWave( vWaves );
	float2 vWavesSum = vWaves.xz + vWaves.yw;

	float3 bend = animParams.y * fDetailAmp * normal.xyz;
	bend.y = animParams.w * fBranchAmp;
	pos.xyz += ((vWavesSum.xyx * bend) + (wind.xyz * vWavesSum.y * animParams.w)) * wind.w; 

	pos.xyz += animParams.z * wind.xyz;
	
	return pos;
};	

float4 _Wind;

float4 TreeWind (fixed4 color, float WindEdgeFlutter, float WindEdgeFlutterFreqScale, float4 vertex, fixed3 normal) {

	float4	wind;
	float bendingFact	= color.a;
	
	wind.xyz = mul((float3x3)_World2Object,_Wind.xyz);
	wind.w   = _Wind.w  * bendingFact;
	
	float4 windParams = float4(0,WindEdgeFlutter,bendingFact.xx);
	float  windTime   = _Time.y * float2(WindEdgeFlutterFreqScale,1);
	float4 mdlPos     = AnimateVertex2(vertex,normal,windParams,wind,windTime);
	
	return mdlPos;
};

//-------------------------------------------------------------------------------------------------
// Distance Calculations

// Distance from Vertex to Camera

float DistanceToCamera(float4 position) {

	float4 position_in_world_space = mul(_Object2World, position);
	return distance(position_in_world_space,  _WorldSpaceCameraPos);
}

//-------------------------------------------------------------------------------------------------
//
// Fragment Parts
//
//-------------------------------------------------------------------------------------------------

// Lightmap
inline fixed3 DecodeLightMap(fixed4 lighttex) {

	#if defined(SHADER_API_GLES) && defined(SHADER_API_MOBILE)
		fixed3 light = lighttex.rgb * fixed3(2.0, 2.0, 2.0);
	#else
		fixed3 light = lighttex.rgb * lighttex.a * 8f;
	#endif
	return light;
};

// Blend based on Texture
inline fixed BlendFactor(fixed blendTex, fixed blendColor, half Sharpness, half BlendAdd) {

	fixed blendFactor;
	blendFactor = saturate(pow(blendColor+(blendColor*blendTex), Sharpness));
	blendFactor = saturate(blendFactor+BlendAdd);
	return blendFactor;	
};	