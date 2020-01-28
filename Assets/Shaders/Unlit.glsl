#vertex
#version 410

layout (location = 0) in vec3 pos;

void main(){
	gl_Position = vec4(pos, 1.0);
}

#fragment
#version 410

out vec4 col;

void main(){
	col = vec4(0.5f, 1.0f, 0.0f, 1.0f);
}