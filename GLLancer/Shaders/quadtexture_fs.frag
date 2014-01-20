@include 'includes/light.fragh'
//
// Structure definitions
//

struct VertexShaderOutput {
    vec4 Position;
    vec2 TextureCoordinate;
};


//
// Global variable definitions
//

uniform sampler2D DtSampler;
uniform int FlipU;
uniform int FlipV;

//
// Function declarations
//

vec4 PixelShaderFunction( in VertexShaderOutput xlat_var_input );

//
// Function definitions
//

vec4 PixelShaderFunction( in VertexShaderOutput xlat_var_input ) {
    vec2 texcoord;

    texcoord = xlat_var_input.TextureCoordinate;
    if ( bool( FlipU ) ){
        texcoord.x  = (1.00000 - texcoord.x );
    }
    if ( bool( FlipV ) ){
        texcoord.y  = (1.00000 - texcoord.y );
    }
    return texture2D( DtSampler, texcoord);
}


//
// Translator's entry point
//
void main() {
    vec4 xlat_retVal;
    VertexShaderOutput xlat_temp_xlat_var_input;
    xlat_temp_xlat_var_input.Position = vec4( gl_FragCoord);
    xlat_temp_xlat_var_input.TextureCoordinate = vec2( gl_TexCoord[0]);

    xlat_retVal = PixelShaderFunction( xlat_temp_xlat_var_input);

    gl_FragData[0] = vec4( xlat_retVal);
}
