Shader "VV/Sprite_VColor_NM" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		LOD 200
		
		CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
		#pragma vertex vert
		#pragma surface surf BasicDiffuse alpha exclude_path:prepass noforwardadd

		sampler2D _MainTex;
		sampler2D _NormalMap;

		struct Input {
			fixed2 uv_MainTex;
			fixed4 vertexColor;
		};
		
		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertexColor = v.color;
		}
		
		inline fixed4 LightingBasicDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 col;
			
			//Lerp
			fixed difLight = max(0,dot (s.Normal, lightDir));
			col.rgb = lerp(s.Albedo*0.75,s.Albedo*1.15,difLight);
			
			
			col.a = s.Alpha;
			return col;
		}
		

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = IN.vertexColor.rgb;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
