#version 450 core
#define NR_POINT_LIGHTS 12

in vec3 position;
in vec2 textureCoordinates;
in vec3 normal;

out vec2 pass_textureCoordinates;
out vec3 totalDiffuse;
out vec3 totalSpecular;
out float visibility;

uniform vec3 lightPosition[NR_POINT_LIGHTS];
uniform vec3 lightColour[NR_POINT_LIGHTS];
uniform vec3 attenuation[NR_POINT_LIGHTS];
uniform vec3 coneDirection[NR_POINT_LIGHTS];
uniform vec2 angle[NR_POINT_LIGHTS];
uniform float shineDamper;
uniform float reflectivity;
uniform float useFakeLighting;
uniform float numberOfRows;
uniform vec2 offset;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

const float density = 0.0035;
const float gradient = 4.0;

void main(void) {
	vec4 worldPosition = transformationMatrix * vec4(position, 1.0);
	vec4 positionRelativeToCamera = viewMatrix * worldPosition;
	gl_Position = projectionMatrix * positionRelativeToCamera;
	pass_textureCoordinates = (textureCoordinates / numberOfRows) + offset;

	vec3 actualNormal = normal;
	if (useFakeLighting > 0.5)
	{
		actualNormal = vec3(0.0, 1.0, 0.0);
	}
	
	vec3 toLightVector[NR_POINT_LIGHTS];
	vec3 surfaceNormal = (transformationMatrix * vec4(actualNormal, 0.0)).xyz;
	for (int i = 0; i < NR_POINT_LIGHTS; i++)
	{
		toLightVector[i] = lightPosition[i] - worldPosition.xyz;
	}
	vec3 toCameraVector = (inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - worldPosition.xyz;

	float distance = length(positionRelativeToCamera.xyz);
	visibility = exp(-pow((distance * density), gradient));
	visibility = clamp(visibility, 0.0, 1.0);
	
	vec3 unitNormal = normalize(surfaceNormal);
	vec3 unitToCameraVector = normalize(toCameraVector);

	totalDiffuse = vec3(0.0);
	totalSpecular = vec3(0.0);

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

	totalDiffuse = max(totalDiffuse, 0.2);	
}