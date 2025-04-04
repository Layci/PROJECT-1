// Upgrade NOTE: upgraded instancing buffer 'SimuranStudioParticle_SystemDark' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SimuranStudio/Particle_System/Dark"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_speednoise("speed noise", Vector) = (0,0,0,0)
		_MainSpeed("Main Speed", Vector) = (0,0,0,0)
		_Noise_01("Noise_01", 2D) = "white" {}
		_Gradient("Gradient", 2D) = "white" {}
		_MainTexture("MainTexture", 2D) = "white" {}
		_OpasityGradient("Opasity Gradient", Float) = 0.5
		[HDR]_Color("Color", Color) = (0.5849056,0.5849056,0.5849056,1)
		[HDR]_Color2("Color 2", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform float4 _Color;
				uniform float4 _Color2;
				uniform sampler2D _MainTexture;
				uniform float4 _MainSpeed;
				uniform sampler2D _Noise_01;
				SamplerState sampler_Noise_01;
				uniform sampler2D _Gradient;
				SamplerState sampler_Gradient;
				uniform float _OpasityGradient;
				UNITY_INSTANCING_BUFFER_START(SimuranStudioParticle_SystemDark)
					UNITY_DEFINE_INSTANCED_PROP(float4, _speednoise)
#define _speednoise_arr SimuranStudioParticle_SystemDark
					UNITY_DEFINE_INSTANCED_PROP(float4, _Gradient_ST)
#define _Gradient_ST_arr SimuranStudioParticle_SystemDark
				UNITY_INSTANCING_BUFFER_END(SimuranStudioParticle_SystemDark)


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 appendResult3 = (float2(_MainSpeed.x , _MainSpeed.y));
					float2 texCoord4 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner5 = ( 1.0 * _Time.y * appendResult3 + texCoord4);
					float2 appendResult2 = (float2(_MainSpeed.z , _MainSpeed.w));
					float2 panner6 = ( 1.0 * _Time.y * appendResult2 + texCoord4);
					float4 tex2DNode8 = tex2D( _MainTexture, panner6 );
					float4 _speednoise_Instance = UNITY_ACCESS_INSTANCED_PROP(_speednoise_arr, _speednoise);
					float2 appendResult19 = (float2(_speednoise_Instance.x , _speednoise_Instance.y));
					float2 texCoord17 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner16 = ( 1.0 * _Time.y * appendResult19 + texCoord17);
					float4 tex2DNode15 = tex2D( _Noise_01, panner16 );
					float4 _Gradient_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Gradient_ST_arr, _Gradient_ST);
					float2 uv_Gradient = i.texcoord.xy * _Gradient_ST_Instance.xy + _Gradient_ST_Instance.zw;
					float4 tex2DNode21 = tex2D( _Gradient, uv_Gradient );
					float temp_output_26_0 = ( ( ( ( ( tex2D( _MainTexture, panner5 ).r + tex2DNode8.r ) * tex2DNode8.r * tex2DNode15.g ) + ( tex2DNode21.b * _OpasityGradient ) ) * tex2DNode15.g ) * tex2DNode21.b );
					float2 appendResult20 = (float2(_speednoise_Instance.z , _speednoise_Instance.w));
					float clampResult28 = clamp( pow( temp_output_26_0 , appendResult20.x ) , 0.0 , 1.0 );
					float4 lerpResult31 = lerp( _Color , _Color2 , clampResult28);
					float4 appendResult35 = (float4((( lerpResult31 * i.color )).rgb , ( _Color.a * _Color2.a * temp_output_26_0 * i.color.a )));
					

					fixed4 col = appendResult35;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18500
174;176;1440;799;3217.151;411.8834;1.725342;True;False
Node;AmplifyShaderEditor.Vector4Node;1;-2398.52,-222.5621;Inherit;False;Property;_MainSpeed;Main Speed;1;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1848.314,-209.6729;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;18;-2236.023,418.3345;Inherit;False;InstancedProperty;_speednoise;speed noise;0;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;3;-2045.127,-325.3734;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;2;-2063.127,-102.3734;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;6;-1557.844,103.3574;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;5;-1395.72,-380.7808;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-2040.293,271.7558;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;19;-1966.556,418.9766;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;12;-1435.841,-209.9357;Inherit;True;Property;_MainTexture;MainTexture;4;0;Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;8;-1100.763,-125.0582;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;16;-1570.213,308.5465;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;7;-1111.015,-396.7116;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;21;-1112.556,360.9766;Inherit;True;Property;_Gradient;Gradient;3;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-729.5559,253.9766;Inherit;False;Property;_OpasityGradient;Opasity Gradient;5;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-644.0992,-286.4332;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-1125.749,157.2601;Inherit;True;Property;_Noise_01;Noise_01;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-446.5559,104.9766;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-469.6695,-172.1438;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-158.5559,42.97659;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;7.444092,138.9766;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;353.2535,277.4052;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;20;-581.9056,636.1165;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;27;692.144,472.3587;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT2;1,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;826.6716,126.0984;Inherit;False;Property;_Color2;Color 2;7;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;28;1087.517,503.3325;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;972.1878,-111.8366;Inherit;False;Property;_Color;Color;6;1;[HDR];Create;True;0;0;False;0;False;0.5849056,0.5849056,0.5849056,1;0.5849056,0.5849056,0.5849056,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;1501.109,142.5775;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;33;1096.627,681.8879;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;1730.68,164.4416;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;1630.472,410.4107;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;36;1895.549,231.0472;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;2158.436,280.7871;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2358.13,267.0532;Float;False;True;-1;2;ASEMaterialInspector;0;7;SimuranStudio/Particle_System/Dark;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;3;0;1;1
WireConnection;3;1;1;2
WireConnection;2;0;1;3
WireConnection;2;1;1;4
WireConnection;6;0;4;0
WireConnection;6;2;2;0
WireConnection;5;0;4;0
WireConnection;5;2;3;0
WireConnection;19;0;18;1
WireConnection;19;1;18;2
WireConnection;8;0;12;0
WireConnection;8;1;6;0
WireConnection;16;0;17;0
WireConnection;16;2;19;0
WireConnection;7;0;12;0
WireConnection;7;1;5;0
WireConnection;13;0;7;1
WireConnection;13;1;8;1
WireConnection;15;1;16;0
WireConnection;23;0;21;3
WireConnection;23;1;24;0
WireConnection;14;0;13;0
WireConnection;14;1;8;1
WireConnection;14;2;15;2
WireConnection;22;0;14;0
WireConnection;22;1;23;0
WireConnection;25;0;22;0
WireConnection;25;1;15;2
WireConnection;26;0;25;0
WireConnection;26;1;21;3
WireConnection;20;0;18;3
WireConnection;20;1;18;4
WireConnection;27;0;26;0
WireConnection;27;1;20;0
WireConnection;28;0;27;0
WireConnection;31;0;29;0
WireConnection;31;1;30;0
WireConnection;31;2;28;0
WireConnection;34;0;31;0
WireConnection;34;1;33;0
WireConnection;32;0;29;4
WireConnection;32;1;30;4
WireConnection;32;2;26;0
WireConnection;32;3;33;4
WireConnection;36;0;34;0
WireConnection;35;0;36;0
WireConnection;35;3;32;0
WireConnection;0;0;35;0
ASEEND*/
//CHKSM=6CE4C224D160D986E25E69D48EF904617C8772C2