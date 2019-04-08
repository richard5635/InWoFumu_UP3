// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "InWoFumu/Mat02"
{
	Properties
	{
		_Pattern("Pattern", 2D) = "white" {}
		_BorderIntensity("BorderIntensity", Range( 0 , 1)) = 0.04453347
		_TouchIntensity("TouchIntensity", Range( 0 , 1)) = 0
		_OverallBrightness("Overall Brightness", Range( 0 , 0.6)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_BeatStrength("BeatStrength", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			uniform float _TouchIntensity;
			uniform sampler2D _Pattern;
			uniform float4 _Pattern_ST;
			uniform float _OverallBrightness;
			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;
			uniform float _BorderIntensity;
			uniform sampler2D _TextureSample2;
			uniform float4 _TextureSample2_ST;
			uniform float _BeatStrength;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float2 uv_Pattern = i.ase_texcoord.xy * _Pattern_ST.xy + _Pattern_ST.zw;
				float2 uv_TextureSample0 = i.ase_texcoord.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float2 uv_TextureSample2 = i.ase_texcoord.xy * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
				float mulTime18 = _Time.y * ( UNITY_PI / 0.4 );
				float4 BPMTexture35 = ( tex2D( _TextureSample2, uv_TextureSample2 ) * pow( (0.0 + (cos( mulTime18 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) , (float)12 ) * _BeatStrength );
				
				
				finalColor = ( ( float4( 0,0,0,0 ) + ( _TouchIntensity * tex2D( _Pattern, uv_Pattern ) ) ) + _OverallBrightness + ( tex2D( _TextureSample0, uv_TextureSample0 ) * _BorderIntensity ) + BPMTexture35 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16100
426;204;1494;960;972.3871;-590.9956;1.458059;True;True
Node;AmplifyShaderEditor.CommentaryNode;27;-1286.392,834.3499;Float;False;1047.09;603.7585;Followsbeat;12;22;18;15;26;20;17;23;24;21;29;31;28;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1212.356,1062.91;Float;False;Constant;_BeatPeriod;BeatPeriod;7;0;Create;True;0;0;False;0;0.4;1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;24;-1236.392,977.8145;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;26;-1014.142,1014.682;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;18;-941.9203,884.3499;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-729.6086,1246.146;Float;False;Constant;_Float3;Float 3;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-727.9275,1323.608;Float;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-728.9839,1174.978;Float;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;17;-757.7584,888.6652;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;33;-161.1295,1291.853;Float;False;Constant;_Abruptness;Abruptness;8;0;Create;True;0;0;False;0;12;0;0;1;INT;0
Node;AmplifyShaderEditor.TFHCRemapNode;21;-530.8014,1123.308;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-14.44234,1423.548;Float;False;Property;_BeatStrength;BeatStrength;8;0;Create;True;0;0;False;0;0;0.6;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;32;14.14297,1156.483;Float;True;2;0;FLOAT;0;False;1;INT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-30.16319,948.3616;Float;True;Property;_TextureSample2;Texture Sample 2;6;0;Create;True;0;0;False;0;e410aaf0937f36442a5098a3529a434c;e410aaf0937f36442a5098a3529a434c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1194.3,-242.4085;Float;True;Property;_Pattern;Pattern;0;0;Create;True;0;0;False;0;e410aaf0937f36442a5098a3529a434c;e410aaf0937f36442a5098a3529a434c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;307.2484,1084.501;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1173.35,-356.6107;Float;False;Property;_TouchIntensity;TouchIntensity;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-900.2704,141.1464;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;8814f0eece65fa84c95b755d5a019b1a;f34b66c8606b8ed479a721619e505b48;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;449.8776,1086.414;Float;True;BPMTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-868.4731,-310.6843;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-859.4687,393.374;Float;False;Property;_BorderIntensity;BorderIntensity;1;0;Create;True;0;0;False;0;0.04453347;0.093;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-521.9296,148.5651;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-797.7151,-84.37888;Float;False;Property;_OverallBrightness;Overall Brightness;3;0;Create;True;0;0;False;0;0;0.028;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-620.0926,-402.5201;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-364.7565,421.0178;Float;True;35;BPMTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;12;-835.0785,495.7249;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;65d073430dc2a534c9cccb6705daf8cb;65d073430dc2a534c9cccb6705daf8cb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;29;-1063.672,1260.163;Float;False;2;0;INT;0;False;1;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.IntNode;31;-1275.952,1260.382;Float;False;Constant;_OneMinutePerTwo;OneMinutePerTwo;8;0;Create;True;0;0;False;0;30;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;28;-1224.701,1342.872;Float;False;Property;_BPM;BPM;7;0;Create;True;0;0;False;0;75;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-411.2838,-249.6466;Float;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;1;InWoFumu/Mat02;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;26;0;24;0
WireConnection;26;1;15;0
WireConnection;18;0;26;0
WireConnection;17;0;18;0
WireConnection;21;0;17;0
WireConnection;21;1;20;0
WireConnection;21;2;22;0
WireConnection;21;3;23;0
WireConnection;21;4;22;0
WireConnection;32;0;21;0
WireConnection;32;1;33;0
WireConnection;34;0;13;0
WireConnection;34;1;32;0
WireConnection;34;2;38;0
WireConnection;35;0;34;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;8;0;7;0
WireConnection;8;1;9;0
WireConnection;4;1;3;0
WireConnection;29;0;31;0
WireConnection;29;1;28;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;6;2;8;0
WireConnection;6;3;36;0
WireConnection;0;0;6;0
ASEEND*/
//CHKSM=C4E1EEC789AB5599A1ED6E557525D667C5034145