//
// Structure definitions
//

struct VertexShaderOutput {
    vec4 Position;
    vec2 TextureCoordinate;
};

struct VertexShaderInput {
    vec4 Position;
    vec2 TextureCoordinate;
};


//
// Function declarations
//

VertexShaderOutput VertexShaderFunction( in VertexShaderInput xlat_var_input );

//
// Function definitions
//

VertexShaderOutput VertexShaderFunction( in VertexShaderInput xlat_var_input ) {
    VertexShaderOutput xlat_var_output;

    xlat_var_output.Position = xlat_var_input.Position;
    xlat_var_output.TextureCoordinate = xlat_var_input.TextureCoordinate;
    return xlat_var_output;
}


//
// Translator's entry point
//
void main() {
    VertexShaderOutput xlat_retVal;
    VertexShaderInput xlat_temp_xlat_var_input;
    xlat_temp_xlat_var_input.Position = vec4( gl_Vertex);
    xlat_temp_xlat_var_input.TextureCoordinate = vec2( gl_MultiTexCoord0);

    xlat_retVal = VertexShaderFunction( xlat_temp_xlat_var_input);

    gl_Position = vec4( xlat_retVal.Position);
    gl_TexCoord[0] = vec4( xlat_retVal.TextureCoordinate, 0.0, 0.0);
}
