#version 450 core
#define NR_POINT_LIGHTS 12

flat in vec2 pass_textureCoordinates;
flat in vec3 surfaceNormal;
flat in vec3 toLightVector[NR_POINT_LIGHTS];
flat in vec3 toCameraVector;
flat in float visibility;

out vec4 out_Color;

uniform sampler2D backgroundTexture;
uniform sampler2D rTexture;
uniform sampler2D gTexture;
uniform sampler2D bTexture;
uniform sampler2D blendMap;

uniform vec3 lightColour[NR_POINT_LIGHTS];
uniform vec3 attenuation[NR_POINT_LIGHTS];
uniform vec3 coneDirection[NR_POINT_LIGHTS];
uniform vec2 angle[NR_POINT_LIGHTS];
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
	vec3 unitToCameraVector = normalize(toCameraVector);

	vec3 totalDiffuse = vec3(0.0);
	vec3 totalSpecular = vec3(0.0);

	for (int i = 0; i < NR_POINT_LIGHTS; i++)
	{
		float distance = length(toLightVector[i]);
		float attenuationFactor = attenuation[i].x + attenuation[i].y * distance + attenuation[i].z * distance * distance;

		vec3 unitLightVector = normalize(toLightVector[i]);

		float intensity = 1.0;
		float coneAngle = angle[i].x;
		float outerConeAngle = angle[i].y;

		// spotlight
		if (coneAngle >= 0)
		{
			float lightToSurfaceAngle = dot(unitLightVector, normalize(coneDirection[i]));
			if (lightToSurfaceAngle < outerConeAngle)
			{
				continue;
			}

			if (lightToSurfaceAngle < coneAngle)
			{
				float eps = coneAngle - outerConeAngle;
				intensity = clamp((lightToSurfaceAngle - outerConeAngle) / eps, 0.0, 1.0);
			}
		}

		float nDot1 = dot(unitNormal, unitLightVector);
		float brightness = max(nDot1, 0.0);
		vec3 lightDirection = -unitLightVector;
		vec3 reflectedLightDirection = reflect(lightDirection, unitNormal);
		float specularFactor = dot(reflectedLightDirection, unitToCameraVector);
		specularFactor = max(specularFactor, 0.0);
		float dampedFactor = pow(specularFactor, shineDamper);

		totalDiffuse = totalDiffuse + intensity * ((brightness * lightColour[i]) / attenuationFactor);
		totalSpecular = totalSpecular + intensity * ((dampedFactor * reflectivity * lightColour[i]) / attenuationFactor);
	}

	totalDiffuse = max(totalDiffuse, 0.15);

	out_Color = vec4(totalDiffuse, 1.0) * totalColour + vec4(totalSpecular, 1.0);
	out_Color = mix(vec4(skyColour, 1.0), out_Color, visibility);
}

