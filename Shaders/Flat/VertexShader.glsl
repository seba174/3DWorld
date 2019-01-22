#version 450 core
#define NR_POINT_LIGHTS 12

in vec3 position;
in vec2 textureCoordinates;
in vec3 normal;

out vec2 pass_textureCoordinates;
flat out vec3 surfaceNormal;
flat out vec3 toLightVector[NR_POINT_LIGHTS];
flat out vec3 toCameraVector;
flat out float visibility;

uniform sampler2D textureSampler;
uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform vec3 lightPosition[NR_POINT_LIGHTS];

uniform float useFakeLighting;

uniform float numberOfRows;
uniform vec2 offset;

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

	surfaceNormal = (transformationMatrix * vec4(actualNormal, 0.0)).xyz;
	for (int i = 0; i < NR_POINT_LIGHTS; i++)
	{
		toLightVector[i] = lightPosition[i] - worldPosition.xyz;
	}
	toCameraVector = (inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - worldPosition.xyz;

	float distance = length(positionRelativeToCamera.xyz);
	visibility = exp(-pow((distance * density), gradient));
	visibility = clamp(visibility, 0.0, 1.0);
}