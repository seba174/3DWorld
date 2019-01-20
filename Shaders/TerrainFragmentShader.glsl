﻿#version 450 core

in vec2 pass_textureCoordinates;
in vec3 surfaceNormal;
in vec3 toLightVector;
in vec3 toCameraVector;
in float visibility;

out vec4 out_Color;

uniform sampler2D backgroundTexture;
uniform sampler2D rTexture;
uniform sampler2D gTexture;
uniform sampler2D bTexture;
uniform sampler2D blendMap;

uniform vec3 lightColour;
uniform float shineDamper;
uniform float reflectivity;
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

	vec3 unitNormal = normalize(surfaceNormal);
	vec3 unitLightVector = normalize(toLightVector);
	vec3 unitToCameraVector = normalize(toCameraVector);

	float nDot1 = dot(unitNormal, unitLightVector);
	float brightness = max(nDot1, 0.15);
	vec3 diffuse = brightness * lightColour;

	vec3 lightDirection = -unitLightVector;
	vec3 reflectedLightDirection = reflect(lightDirection, unitNormal);

	float specularFactor = dot(reflectedLightDirection, unitToCameraVector);
	specularFactor = max(specularFactor, 0.0);
	float dampedFactor = pow(specularFactor, shineDamper);
	vec3 finalSpecular = dampedFactor * reflectivity * lightColour;

	out_Color = vec4(diffuse, 1.0) * texture(backgroundTexture, tiledCoords) + vec4(finalSpecular, 1.0);
	out_Color = mix(vec4(skyColour, 1.0), out_Color, visibility);
}