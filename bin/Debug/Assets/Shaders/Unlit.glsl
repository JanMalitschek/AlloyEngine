#vertex
#version 410

layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 norm;
layout (location = 2) in vec2 uv;
layout (location = 3) in vec4 col;

out vec4 vertexColor;
out vec2 texCoords;
uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main(){
	gl_Position = proj * view * model * vec4(pos, 1.0);
	vertexColor = col;
	texCoords = uv;
}

#fragment
#version 410

in vec4 vertexColor;
in vec2 texCoords;

uniform sampler2D tex;

out vec4 col;

void main(){
	col = texture(tex, texCoords) * vertexColor;
}