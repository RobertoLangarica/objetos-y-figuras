Shader "VV/SpriteNM" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NormalMap ("NormalMap", 2D) = "bump" {}
		_MainColor("Main Color",Color) = (1,1,1,1)
		_ShadowColor("Shadow Color",Color) = (1,1,1,1)
		_ShadowRange("Shadow less than",Range(0,1)) = .5
		_GlowColor("Glow Color",Color) = (1,1,1,1)
		_GlowRange("Glow greater than",Range(0,1)) = .6
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BasicDiffuse alpha

		sampler2D _MainTex;
		sampler2D _NormalMap;
		half4 _ShadowColor;
		half4 _MainColor;
		half4 _GlowColor;
		float _ShadowRange;
		float _GlowRange;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
		};
		
		inline float4 LightingBasicDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = dot (s.Normal, lightDir)*0.5 + 0.5;
			float4 col;
			
			/*if(difLight < _ShadowRange)
			{
				col.rgb = _ShadowColor.rgb;
			}
			else if(difLight > _GlowRange)
			{
				col.rgb = _GlowColor.rgb;
			}
			else
			{
				col.rgb = s.Albedo;
			}
			*/
			col.a = s.Alpha;
			
			col.rgb = s.Albedo * difLight *(_MainColor * atten);
			
			return col;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = _MainColor.rgb;
			o.Normal = UnpackNormal(tex2D (_NormalMap, IN.uv_NormalMap));
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}