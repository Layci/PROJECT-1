// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SimuranStudio/Particle_System/Speed"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_tex_voidzona("tex_voidzona", 2D) = "white" {}
		_tex_ground_03("tex_ground_03", 2D) = "white" {}
		_Speed_Noise("Speed_Noise", Vector) = (1,1,0,0)
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_Strength("Strength", Float) = 15
		_Center("Center", Vector) = (0.57,0.35,0,0)

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend One One
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
				uniform sampler2D _tex_ground_03;
				sampler2D _Sampler604;
				uniform float2 _Speed_Noise;
				uniform float2 _Center;
				uniform float _Strength;
				uniform sampler2D _tex_voidzona;
				SamplerState sampler_tex_voidzona;
				SamplerState sampler_tex_ground_03;
				uniform float4 _Color;


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

					float2 temp_output_1_0_g6 = float2( 1,1 );
					float2 texCoord80_g6 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 appendResult10_g6 = (float2(( (temp_output_1_0_g6).x * texCoord80_g6.x ) , ( texCoord80_g6.y * (temp_output_1_0_g6).y )));
					float2 temp_output_11_0_g6 = float2( 0,0 );
					float2 texCoord81_g6 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner18_g6 = ( ( (temp_output_11_0_g6).x * _Time.y ) * float2( 1,0 ) + texCoord81_g6);
					float2 panner19_g6 = ( ( _Time.y * (temp_output_11_0_g6).y ) * float2( 0,1 ) + texCoord81_g6);
					float2 appendResult24_g6 = (float2((panner18_g6).x , (panner19_g6).y));
					float2 temp_output_47_0_g6 = _Speed_Noise;
					float2 temp_output_1_0_g5 = i.texcoord.xy;
					float2 temp_output_11_0_g5 = ( temp_output_1_0_g5 - _Center );
					float2 break18_g5 = temp_output_11_0_g5;
					float2 appendResult19_g5 = (float2(break18_g5.y , -break18_g5.x));
					float dotResult12_g5 = dot( temp_output_11_0_g5 , temp_output_11_0_g5 );
					float2 temp_cast_0 = (_Strength).xx;
					float2 temp_output_31_0_g6 = ( ( temp_output_1_0_g5 + ( appendResult19_g5 * ( dotResult12_g5 * temp_cast_0 ) ) + float2( 0,0 ) ) - float2( 1,1 ) );
					float2 appendResult39_g6 = (float2(frac( ( atan2( (temp_output_31_0_g6).x , (temp_output_31_0_g6).y ) / 6.28318548202515 ) ) , length( temp_output_31_0_g6 )));
					float2 panner54_g6 = ( ( (temp_output_47_0_g6).x * _Time.y ) * float2( 1,0 ) + appendResult39_g6);
					float2 panner55_g6 = ( ( _Time.y * (temp_output_47_0_g6).y ) * float2( 0,1 ) + appendResult39_g6);
					float2 appendResult58_g6 = (float2((panner54_g6).x , (panner55_g6).y));
					float4 tex2DNode2 = tex2D( _tex_ground_03, ( ( (tex2D( _Sampler604, ( appendResult10_g6 + appendResult24_g6 ) )).rg * 1.0 ) + ( float2( 1,1 ) * appendResult58_g6 ) ) );
					float2 texCoord6 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float4 tex2DNode1 = tex2D( _tex_voidzona, texCoord6 );
					

					fixed4 col = ( tex2DNode2 * tex2DNode1 * i.color * i.color.a * tex2DNode1.a * tex2DNode2.a * _Color * _Color.a );
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
174;176;1440;799;1110.574;473.0931;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;19;-1491.224,-150.6825;Inherit;False;Property;_Strength;Strength;4;0;Create;True;0;0;False;0;False;15;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;18;-1428.842,-359.0471;Inherit;False;Property;_Center;Center;5;0;Create;True;0;0;False;0;False;0.57,0.35;0.57,0.35;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;5;-963.1343,-285.3791;Inherit;False;Property;_Speed_Noise;Speed_Noise;2;0;Create;True;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;17;-1260.805,-209.5302;Inherit;True;Radial Shear;-1;;5;c6dc9fc7fa9b08c4d95138f2ae88b526;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;4;-673.3948,-266.7257;Inherit;False;RadialUVDistortion;-1;;6;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler604;False;1;FLOAT2;1,1;False;11;FLOAT2;0,0;False;65;FLOAT;1;False;68;FLOAT2;1,1;False;47;FLOAT2;1,1;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1516.957,74.52042;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-499.9593,72.87084;Inherit;True;Property;_tex_voidzona;tex_voidzona;0;0;Create;True;0;0;False;0;False;-1;b3518cf558881654cbadf0ae28caeecb;b3518cf558881654cbadf0ae28caeecb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-223.9071,-252.1507;Inherit;True;Property;_tex_ground_03;tex_ground_03;1;0;Create;True;0;0;False;0;False;-1;2b6a5cd3a05828040822f30fb1b55aa0;2b6a5cd3a05828040822f30fb1b55aa0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;7;-361.4336,264.149;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-99.81371,339.8394;Inherit;False;Property;_Color;Color;3;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;554.6739,-73.58537;Inherit;True;8;8;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;COLOR;0,0,0,0;False;7;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1009.67,-5.397545;Float;False;True;-1;2;ASEMaterialInspector;0;7;SimuranStudio/Particle_System/Speed;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;17;2;18;0
WireConnection;17;3;19;0
WireConnection;4;47;5;0
WireConnection;4;29;17;0
WireConnection;1;1;6;0
WireConnection;2;1;4;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;3;2;7;0
WireConnection;3;3;7;4
WireConnection;3;4;1;4
WireConnection;3;5;2;4
WireConnection;3;6;8;0
WireConnection;3;7;8;4
WireConnection;0;0;3;0
ASEEND*/
//CHKSM=BE7708E57E9D955AF49669D14857423FA64E8597