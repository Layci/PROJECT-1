// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SimuranStudio/Particle_System/3TEX"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Speed12Tex("Speed (1/2) Tex", Vector) = (0,0,0,0)
		_Texture1("Texture 1", 2D) = "white" {}
		_Texture3("Texture 3", 2D) = "white" {}
		_Texture2("Texture 2", 2D) = "white" {}
		[HDR]_Color1("Color 1", Color) = (1,0.5586855,0,1)
		[HDR]_Color2("Color 2", Color) = (0.5377358,0.3559349,0.2206746,1)
		_Emissive_3_tex("Emissive_3_tex", Float) = 3.62

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
				uniform sampler2D _Texture1;
				SamplerState sampler_Texture1;
				uniform float2 _Speed12Tex;
				uniform float4 _Color2;
				uniform sampler2D _Texture2;
				uniform float4 _Color1;
				uniform sampler2D _Texture3;
				SamplerState sampler_Texture3;
				uniform float _Emissive_3_tex;


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

					float2 texCoord1 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner3 = ( 1.0 * _Time.y * _Speed12Tex + texCoord1);
					float4 temp_output_12_0 = ( tex2D( _Texture2, panner3 ) * _Color1 );
					float4 lerpResult29 = lerp( ( step( tex2D( _Texture1, panner3 ).r , 0.25 ) * _Color2 ) , temp_output_12_0 , temp_output_12_0);
					float2 texCoord36 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float4 tex2DNode30 = tex2D( _Texture3, texCoord36 );
					

					fixed4 col = ( lerpResult29 * i.color * i.color.a * ( tex2DNode30 * tex2DNode30.a * _Emissive_3_tex ) );
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
174;176;1440;799;2283.024;1070.934;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1887.807,-933.425;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;2;-1892.807,-784.425;Inherit;False;Property;_Speed12Tex;Speed (1/2) Tex;0;0;Create;True;0;0;False;0;False;0,0;0,0.78;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;3;-1616.807,-861.425;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;5;-1388.689,-895.425;Inherit;True;Property;_Texture1;Texture 1;1;0;Create;True;0;0;False;0;False;-1;e50ea5e7e31e2354586dc239a61eaca1;e50ea5e7e31e2354586dc239a61eaca1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-1378.709,-625.5795;Inherit;True;Property;_Texture2;Texture 2;3;0;Create;True;0;0;False;0;False;-1;e50ea5e7e31e2354586dc239a61eaca1;e50ea5e7e31e2354586dc239a61eaca1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-923.8364,-642.4849;Inherit;False;Property;_Color2;Color 2;5;1;[HDR];Create;True;0;0;False;0;False;0.5377358,0.3559349,0.2206746,1;0.5377358,0.3559349,0.2206746,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-949.3304,-175.6421;Inherit;False;Property;_Color1;Color 1;4;1;[HDR];Create;True;0;0;False;0;False;1,0.5586855,0,1;0.8962264,0.5286956,0.06341223,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;10;-1004.229,-881.9884;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1169.199,204.4498;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-569.7305,-824.0836;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-749.0103,-381.5465;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-408.5856,387.6925;Inherit;False;Property;_Emissive_3_tex;Emissive_3_tex;6;0;Create;True;0;0;False;0;False;3.62;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-627.6788,155.1878;Inherit;True;Property;_Texture3;Texture 3;2;0;Create;True;0;0;False;0;False;-1;e50ea5e7e31e2354586dc239a61eaca1;e50ea5e7e31e2354586dc239a61eaca1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-84.77197,132.1939;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;29;-402.5006,-556.2755;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;21;-453.0045,-211.3724;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;19.64335,-240.912;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;205.5054,-430.2642;Float;False;True;-1;2;ASEMaterialInspector;0;7;SimuranStudio/Particle_System/3TEX;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;3;0;1;0
WireConnection;3;2;2;0
WireConnection;5;1;3;0
WireConnection;24;1;3;0
WireConnection;10;0;5;1
WireConnection;13;0;10;0
WireConnection;13;1;7;0
WireConnection;12;0;24;0
WireConnection;12;1;11;0
WireConnection;30;1;36;0
WireConnection;31;0;30;0
WireConnection;31;1;30;4
WireConnection;31;2;32;0
WireConnection;29;0;13;0
WireConnection;29;1;12;0
WireConnection;29;2;12;0
WireConnection;23;0;29;0
WireConnection;23;1;21;0
WireConnection;23;2;21;4
WireConnection;23;3;31;0
WireConnection;0;0;23;0
ASEEND*/
//CHKSM=881D08B0EFB7956A07CF7065D36C9451566307F0