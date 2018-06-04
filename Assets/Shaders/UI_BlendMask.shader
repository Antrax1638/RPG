﻿Shader "UI/BlendMask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainMask ("Mask", 2D) = "white" {}
		_Rate ("Rate", float) = 0.0
		[Toggle] _PixelSnap ("PixelSnap", float) = 0.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _MainMask;
			float _Rate;
			float _PixelSnap;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				if (_PixelSnap) o.vertex = UnityPixelSnap(v.vertex);

				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				return col;
			}
			ENDCG
		}
	}
}