#version 450 core

in vec2 pass_textureCoordinates;
in vec3 totalDiffuse;
in vec3 totalSpecular;
in float visibility;

out vec4 out_Color;

uniform sampler2D textureSampler;
uniform vec3 skyColour;

void main(void) {
	vec4 textureColor = texture(textureSampler, pass_textureCoordinates);
	if (textureColor.a < 0.5)
	{
		discard;
	}

	out_Color = vec4(totalDiffuse, 1.0) * textureColor + vec4(totalSpecular, 1.0);
	out_Color = mix(vec4(skyColour, 1.0), out_Color, visibility);
}