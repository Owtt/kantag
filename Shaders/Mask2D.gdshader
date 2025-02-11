shader_type canvas_item;

uniform vec2 parentPos;
uniform vec2 parentSize;
uniform vec4 boundsTBRL;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
  COLOR = texture(TEXTURE, UV);
  vec4 bounds;
  bounds.x = parentPos.y + boundsTBRL.x;
  bounds.y = parentPos.y + parentSize.y - boundsTBRL.y;
  bounds.z = parentPos.x + boundsTBRL.z;
  bounds.w = parentPos.x + parentSize.x - boundsTBRL.w;
  
  if (VERTEX.x < 10.0 || VERTEX.y < 10.0)
    COLOR.a = 0.0;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
