Shader "VV/Sprite_RampColor_NM" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RampageTex("Rampage",2D) = "white" {}
		_ColorIndex("Color index", Int) = 0
		_NormalMap ("NormalMap", 2D) = "bump" {}
		_RampColorsCount("Ramp colors Count", Int) = 16
	}
	SubShader {
		Tags { "RenderType"="Opaque" 
				"Queue"="Transparent" 
				"IgnoreProjector"="True"
            	}
		LOD 200
		Cull Off
		
		CGPROGRAM
		#pragma surface surf BasicDiffuse alpha 

		sampler2D _MainTex;
		sampler2D _RampageTex;
		sampler2D _NormalMap;
		int _ColorIndex;
		int _RampColorsCount;
		

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
		};
		
		inline float4 LightingBasicDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			//float difLight = dot (s.Normal, lightDir)*0.5 + 0.5;
			float difLight = max(0,dot (s.Normal, lightDir));
			float4 col;
			
			float colorSize = 1.0/_RampColorsCount;
			//Coordenada invertida ya que los colores vienen arriba-abajo y se leen abajo-arriba
			float yRampcoordenate = 1 - (_ColorIndex*colorSize + (colorSize*0.5));
			
			col.rgb = tex2D(_RampageTex,float2(difLight,yRampcoordenate)).rgb;
			col.a = s.Alpha;
			return col;
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			o.Normal = UnpackNormal(tex2D (_NormalMap, IN.uv_NormalMap));
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		
		ENDCG
	} 
	FallBack "Diffuse"
}