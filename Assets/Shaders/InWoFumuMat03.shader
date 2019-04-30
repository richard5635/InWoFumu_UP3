// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "InWoFumu/Mat03"
{
	Properties
	{
		_TouchPattern("TouchPattern", 2D) = "white" {}
		_OverallBrightness("Overall Brightness", Range( 0 , 1)) = 0.07058824
		_BeatPattern("BeatPattern", 2D) = "white" {}
		_BeatStrength("BeatStrength", Range( 0 , 1)) = 0
		_PulseColor("PulseColor", Color) = (0.25,0.7540736,1,0)
		_PulseValue("PulseValue", Range( 0 , 1)) = 0
		_AVBrightness("AVBrightness", Float) = 0
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

			uniform float4 _PulseColor;
			uniform sampler2D _TouchPattern;
			uniform float4 _TouchPattern_ST;
			uniform float _PulseValue;
			uniform float _OverallBrightness;
			uniform sampler2D _BeatPattern;
			uniform float4 _BeatPattern_ST;
			uniform float _BeatStrength;
			uniform float _AVBrightness;
			
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
				float2 uv_TouchPattern = i.ase_texcoord.xy * _TouchPattern_ST.xy + _TouchPattern_ST.zw;
				float grayscale44 = Luminance(tex2D( _TouchPattern, uv_TouchPattern ).rgb);
				float temp_output_54_0 = (1.2 + (_PulseValue - 0.0) * (-0.3 - 1.2) / (1.0 - 0.0));
				float ifLocalVar41 = 0;
				if( grayscale44 > temp_output_54_0 )
				ifLocalVar41 = grayscale44;
				else if( grayscale44 < temp_output_54_0 )
				ifLocalVar41 = 0.0;
				float temp_output_46_0 = ( temp_output_54_0 + 0.2 );
				float ifLocalVar42 = 0;
				if( ifLocalVar41 > temp_output_46_0 )
				ifLocalVar42 = 0.0;
				else if( ifLocalVar41 < temp_output_46_0 )
				ifLocalVar42 = (0.0 + (grayscale44 - temp_output_54_0) * (1.0 - 0.0) / (temp_output_46_0 - temp_output_54_0));
				float clampResult71 = clamp( ifLocalVar42 , 0.0 , 1.0 );
				float4 TouchVisual60 = ( _PulseColor * clampResult71 );
				float4 temp_output_27_0 = ( TouchVisual60 + float4( 0,0,0,0 ) );
				float2 uv_BeatPattern = i.ase_texcoord.xy * _BeatPattern_ST.xy + _BeatPattern_ST.zw;
				float mulTime7 = _Time.y * ( UNITY_PI / 0.4 );
				float4 BPMTexture18 = ( tex2D( _BeatPattern, uv_BeatPattern ) * pow( (0.0 + (cos( mulTime7 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) , (float)12 ) * _BeatStrength );
				float4 temp_output_22_0 = ( temp_output_27_0 + ( _OverallBrightness + BPMTexture18 + _AVBrightness ) );
				float4 temp_cast_2 = (0.0).xxxx;
				float4 temp_cast_3 = (1.0).xxxx;
				float4 clampResult74 = clamp( temp_output_22_0 , temp_cast_2 , temp_cast_3 );
				
				
				finalColor = ( 1.0 - clampResult74 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16100
426;225;1090;939;2089.65;597.0375;1.477021;True;True
Node;AmplifyShaderEditor.CommentaryNode;3;-790.3602,874.0533;Float;False;1047.09;603.7585;Followsbeat;12;21;20;19;13;11;10;9;8;7;6;5;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;61;-3221.307,-1740.33;Float;False;1836.635;963.7903;Touch Visual;21;47;42;41;49;46;48;53;52;51;50;43;44;54;55;56;59;58;45;57;60;71;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-716.3242,1102.613;Float;False;Constant;_BeatPeriod;BeatPeriod;7;0;Create;True;0;0;False;0;0.4;1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;5;-740.3602,1017.518;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-3053.872,-966.535;Float;False;Constant;_Float8;Float 8;9;0;Create;True;0;0;False;0;-0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-3056.269,-1124.716;Float;False;Constant;_Float4;Float 4;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-3037.095,-891.0397;Float;False;Constant;_Float9;Float 9;9;0;Create;True;0;0;False;0;1.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-3053.871,-1046.824;Float;False;Constant;_Float7;Float 7;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-3171.307,-1249.344;Float;False;Property;_PulseValue;PulseValue;9;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-2628.503,-1078.333;Float;False;Constant;_Float5;Float 5;8;0;Create;True;0;0;False;0;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;54;-2836.975,-1101.945;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-518.1103,1054.385;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;43;-3127.79,-1594.752;Float;True;Property;_TouchPattern;TouchPattern;1;0;Create;True;0;0;False;0;ff0178fd9ead54e8ea4c3c9abae4755f;911914c81964944db988c224b70174f2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;44;-2841.883,-1593.837;Float;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-445.8885,924.0533;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2656.902,-1173.924;Float;False;Constant;_NewMax;New Max;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2651.296,-1246.321;Float;False;Constant;_NewMin;New Min;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-2232.501,-1277.333;Float;False;Constant;_Float6;Float 6;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-2435.503,-1151.333;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;11;-261.7267,928.3686;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;49;-2479.51,-1391.165;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-232.9521,1214.681;Float;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-233.5768,1285.849;Float;False;Constant;_Float3;Float 3;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;41;-2472.249,-1585.368;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-231.8958,1363.311;Float;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;13;-34.76965,1163.011;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;12;334.9022,1331.556;Float;False;Constant;_Abruptness;Abruptness;8;0;Create;True;0;0;False;0;12;0;0;1;INT;0
Node;AmplifyShaderEditor.ConditionalIfNode;42;-2144.104,-1511.269;Float;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;481.5894,1463.251;Float;False;Property;_BeatStrength;BeatStrength;7;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;465.8686,988.0649;Float;True;Property;_BeatPattern;BeatPattern;5;0;Create;True;0;0;False;0;5784bd170c4d34e9ea13515420b56c52;9c2fecf84ca614c4f86224eee4770612;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;71;-1872.597,-1241.189;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;53;-2156.315,-1690.33;Float;False;Property;_PulseColor;PulseColor;8;0;Create;True;0;0;False;0;0.25,0.7540736,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;15;510.1747,1196.186;Float;True;2;0;FLOAT;0;False;1;INT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;803.2802,1124.204;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-1830.337,-1536.488;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-1618.173,-1553.004;Float;False;TouchVisual;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;945.9093,1126.117;Float;True;BPMTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;62;-1083.942,-593.3423;Float;False;60;TouchVisual;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1141.21,-137.2936;Float;True;Property;_OverallBrightness;Overall Brightness;4;0;Create;True;0;0;False;0;0.07058824;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;-1050.659,90.90155;Float;False;18;BPMTexture;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1040.968,171.7533;Float;False;Property;_AVBrightness;AVBrightness;10;0;Create;True;0;0;False;0;0;0.002214839;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-680.1296,-80.29785;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-888.9092,-583.3513;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-275.8707,-427.9171;Float;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-419.4534,-241.7679;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-275.8711,-330.4338;Float;False;Constant;_Float10;Float 10;11;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;74;-60.22474,-215.2263;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-3123.031,-1376.225;Float;False;Constant;_PulseValueRaw;PulseValueRaw;8;0;Create;True;0;0;False;0;-0.2;0;-0.2;1.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;19;-567.6403,1299.866;Float;False;2;0;INT;0;False;1;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1717.58,-414.2486;Float;False;Property;_TouchIntensity;TouchIntensity;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;72;174.6252,27.00566;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-662.1118,-334.8486;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;21;-728.6693,1382.575;Float;False;Property;_BPM;BPM;6;0;Create;True;0;0;False;0;75;0;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;1;-393,176.5;Float;True;Property;_Mask;Mask;2;0;Create;True;0;0;False;0;b00a9dac9fd41443ca8a7515a4f61d96;b00a9dac9fd41443ca8a7515a4f61d96;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;2;-69,46.5;Float;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;20;-779.9203,1300.085;Float;False;Constant;_OneMinutePerTwo;OneMinutePerTwo;8;0;Create;True;0;0;False;0;30;0;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;23;-1740.681,-327.3434;Float;True;Property;_TouchPatternOld;TouchPatternOld;0;0;Create;True;0;0;False;0;ff0178fd9ead54e8ea4c3c9abae4755f;e410aaf0937f36442a5098a3529a434c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1412.536,-395.6192;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;379.3058,49.92552;Float;False;True;2;Float;ASEMaterialInspector;0;1;InWoFumu/Mat03;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;54;0;57;0
WireConnection;54;1;55;0
WireConnection;54;2;56;0
WireConnection;54;3;59;0
WireConnection;54;4;58;0
WireConnection;6;0;5;0
WireConnection;6;1;4;0
WireConnection;44;0;43;0
WireConnection;7;0;6;0
WireConnection;46;0;54;0
WireConnection;46;1;47;0
WireConnection;11;0;7;0
WireConnection;49;0;44;0
WireConnection;49;1;54;0
WireConnection;49;2;46;0
WireConnection;49;3;50;0
WireConnection;49;4;51;0
WireConnection;41;0;44;0
WireConnection;41;1;54;0
WireConnection;41;2;44;0
WireConnection;41;4;48;0
WireConnection;13;0;11;0
WireConnection;13;1;10;0
WireConnection;13;2;9;0
WireConnection;13;3;8;0
WireConnection;13;4;9;0
WireConnection;42;0;41;0
WireConnection;42;1;46;0
WireConnection;42;2;48;0
WireConnection;42;4;49;0
WireConnection;71;0;42;0
WireConnection;71;1;55;0
WireConnection;71;2;56;0
WireConnection;15;0;13;0
WireConnection;15;1;12;0
WireConnection;17;0;16;0
WireConnection;17;1;15;0
WireConnection;17;2;14;0
WireConnection;52;0;53;0
WireConnection;52;1;71;0
WireConnection;60;0;52;0
WireConnection;18;0;17;0
WireConnection;66;0;26;0
WireConnection;66;1;28;0
WireConnection;66;2;73;0
WireConnection;27;0;62;0
WireConnection;22;0;27;0
WireConnection;22;1;66;0
WireConnection;74;0;22;0
WireConnection;74;1;77;0
WireConnection;74;2;78;0
WireConnection;19;0;20;0
WireConnection;19;1;21;0
WireConnection;72;0;74;0
WireConnection;69;0;27;0
WireConnection;2;0;22;0
WireConnection;2;1;1;0
WireConnection;25;0;24;0
WireConnection;25;1;23;0
WireConnection;0;0;72;0
ASEEND*/
//CHKSM=19B9E76A64FAD660F07DF6009B1531B5822F059D