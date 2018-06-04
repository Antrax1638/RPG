Shader "UI/Pixel Snap"
{
	Properties
	{
		_MainTex("Font Texture", 2D) = "white" {}
		_MaskTex("Mask Texture", 2D) = "white" {}
		_Color("Color", Color) = (1.0,1.0,1.0,1.0)
		[Toggle] _PixelSnap("Pixel Snap", float) = 0.0
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Lighting Off Cull Off ZWrite Off Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma surface surf Lambert alpha vertex:vert

		sampler2D _MainTex;
		sampler2D _MaskTex;
		float _PixelSnap;
		fixed4 _Color;

		struct Input 
		{
			float2 uv_MainTex : TEXCOORD0;
			float2 uv2_MaskTex : TEXCOORD1;
			float4 color : COLOR;
		};

		void vert(inout appdata_full v) 
		{
			if(_PixelSnap)
				v.vertex = UnityPixelSnap(v.vertex);
		}

		void surf(Input IN, inout SurfaceOutput surface)
		{
			half4 text = tex2D(_MainTex, IN.uv_MainTex.xy);
			half4 mask = tex2D(_MaskTex, IN.uv2_MaskTex.xy);
			surface.Emission = mask.rgb * IN.color.rgb;
			surface.Emission *= _Color;
			surface.Alpha = text.a * mask.a * IN.color.a;
		}
		ENDCG
	
	}

}