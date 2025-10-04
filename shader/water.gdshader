shader_type spatial;

render_mode unshaded;

uniform float slide_speed = 0.3;
uniform float wobble_speed = 1.0;
uniform float wobble_intensity = 0.4;
uniform float texture_scale = 0.2;
uniform sampler2D water : repeat_enable;

// https://godotshaders.com/shader/shield-shader-with-intersection-highlight/
uniform sampler2D depth_texture : source_color, hint_depth_texture;

// 🌟 New uniform for brightness
uniform float brightness : hint_range(0.0, 3.0) = 1.0;

varying vec2 sample_choord;

void vertex() {
	VERTEX.y += sin(TIME * wobble_speed + (VERTEX.x + VERTEX.z) * 0.3) * 0.2;
	sample_choord = VERTEX.xz;
}

void fragment() {
	vec3 color1 = vec3(1.0);
	vec3 color2 = texture(
		water,
		vec2(
			(sample_choord.x + sin(TIME * wobble_speed) * wobble_intensity + TIME * slide_speed) * texture_scale,
			(sample_choord.y + cos(TIME * wobble_speed * 0.5) * wobble_intensity) * texture_scale
		)
	).rgb;

	float depth = texture(depth_texture, SCREEN_UV).x;
	vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);
	vec4 view = INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	view.xyz /= view.w;
	float linear_depth = -view.z;

	float object_depth = FRAGCOORD.z;
	vec3 object_ndc = vec3(SCREEN_UV * 2.0 - 1.0, object_depth);
	vec4 object_view = INV_PROJECTION_MATRIX * vec4(object_ndc, 1.0);
	object_view.xyz /= object_view.w;
	float linear_object_depth = -object_view.z;

	vec3 final_color = mix(color1, color2, smoothstep(0.0, 4.0, linear_depth - linear_object_depth));

	// 🌟 Apply brightness adjustment
	ALBEDO = final_color * brightness;
}
