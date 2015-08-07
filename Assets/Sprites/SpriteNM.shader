Shader "VV/SpriteNM" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RampageTex("Rampage color tex",2D) = "white" {}
		_NormalMap ("NormalMap", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BasicDiffuse alpha 

		sampler2D _MainTex;
		sampler2D _RampageTex;
		sampler2D _NormalMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
		};
		
		inline float4 LightingBasicDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			//float difLight = dot (s.Normal, lightDir)*0.5 + 0.5;
			float difLight = max(0,dot (s.Normal, lightDir));
			float4 col;
			
			col.rgb = tex2D(_RampageTex,float2(difLight,1)).rgb;
			col.a = s.Alpha;
			return col;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D (_NormalMap, IN.uv_NormalMap));
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}