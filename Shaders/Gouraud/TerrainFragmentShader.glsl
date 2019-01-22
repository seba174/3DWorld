#version 450 core

in vec2 pass_textureCoordinates;
in vec3 totalDiffuse;
in vec3 totalSpecular;
in float visibility;

out vec4 out_Color;

uniform sampler2D backgroundTexture;
uniform sampler2D rTexture;
uniform sampler2D gTexture;
uniform sampler2D bTexture;
uniform sampler2D blendMap;

uniform vec3 skyColour;

void main(void) {
	vec4 blendMapColour = texture(blendMap, pass_textureCoordinates);

	float backTextureAmount = 1 - (blendMapColour.r + blendMapColour.g + blendMapColour.b);
	vec2 tiledCoords = pass_textureCoordinates * 40;
	vec4 backgroundTextureColour = texture(backgroundTexture, tiledCoords) * backTextureAmount;
	vec4 rTextureCoulour = texture(rTexture, tiledCoords) * blendMapColour.r;
	vec4 gTextureCoulour = texture(gTexture, tiledCoords) * blendMapColour.g;
	vec4 bTextureCoulour = texture(bTexture, tiledCoords) * blendMapColour.b;

	vec4 totalColour = backgroundTextureColour + rTextureCoulour + gTextureCoulour + bTextureCoulour;
	
	out_Color = vec4(totalDiffuse, 1.0) * totalColour + vec4(totalSpecular, 1.0);
	out_Color = mix(vec4(skyColour, 1.0), out_Color, visibility);
}

