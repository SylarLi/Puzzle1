Shader "Puzzle1/Quad"
 {
	Properties 
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
		
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f 
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = _Color;
				return o;
			}

			float4 frag(v2f i) : Color
			{
				return i.color;
			}

			ENDCG
		}
	}
}
