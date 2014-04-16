using System;
using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	class Program
	{
		public string Name;
		public int ID;
		public Dictionary<string, GLUniform> Uniforms = new Dictionary<string, GLUniform> ();
		public List<SupportedAttribute> Attributes;
		public void ApplyTextures ()
		{
			int i = 0;
			foreach (var str in Uniforms.Keys) {
				var u = Uniforms [str];
				if (u.IsTexture) {
					switch (i) {
						case 0:
						GL.ActiveTexture (TextureUnit.Texture0);
						((Texture)u.Value).Bind ();
						break;
						case 1:
						GL.ActiveTexture (TextureUnit.Texture1);
						((Texture)u.Value).Bind ();
						break;
						case 2:
						GL.ActiveTexture (TextureUnit.Texture2);
						((Texture)u.Value).Bind ();
						break;
						default:
						throw new ArgumentOutOfRangeException ();
					}
					i++;
					((Texture)u.Value).Bind ();
				}

			}
		}
		public void SetUniform (string name, object value)
		{
			if (!Uniforms.ContainsKey (name))
				return;
			var u = Uniforms [name];
			u.Value = value;
			switch (u.Description.Type) {
			case GLSLTypes.Vector3:
				GL.Uniform3 (u.Location, (Vector3)value);
				break;
			case GLSLTypes.Vector4:
				GL.Uniform4 (u.Location, (Vector4)value);
				break;
			case GLSLTypes.Matrix4:
				var m = (Matrix4)value;
				GL.UniformMatrix4 (u.Location, false, ref m);
				break;
			case GLSLTypes.Float:
				GL.Uniform1 (u.Location, (float)value);
				break;
			case GLSLTypes.Int:
				GL.Uniform1 (u.Location, (int)value);
				break;
			case GLSLTypes.Array:
				SetArrayUniform (name, 0, value);
				break;
			case GLSLTypes.Sampler2D:
				break; //set in applytextures
			case GLSLTypes.SamplerCube:
				break; //set in applytextures
			default:
				throw new ArgumentOutOfRangeException ();
			}
		}
		public void SetArrayUniform (string name, int index, object value, bool overrideType = false, GLSLTypes overidden = GLSLTypes.Int)
		{
			if (!Uniforms.ContainsKey (name))
				return;
			var u = Uniforms [name];
			switch (overrideType ? overidden : u.Description.Type) {
				case GLSLTypes.Vector3:
				GL.Uniform3 (u.Location + index, (Vector3)value);
				break;
				case GLSLTypes.Vector4:
				GL.Uniform4 (u.Location + index, (Vector4)value);
				break;
			case GLSLTypes.Matrix4:
				var m = (Matrix4)value;
				GL.UniformMatrix4 (u.Location + index, false, ref m);
				break;
				case GLSLTypes.Float:
				GL.Uniform1 (u.Location + index, (float)value);
				break;
				case GLSLTypes.Int:
				GL.Uniform1 (u.Location + index, (int)value);
				break;
			case GLSLTypes.Array:

				SetArrayUniform (name, 0, value, true, u.Description.ArrayType);
				//SetUniform (name, value);
				break;
				case GLSLTypes.Sampler2D:
				break; //set in applytextures
				case GLSLTypes.SamplerCube:
				break; //set in applytextures
				default:
				throw new ArgumentOutOfRangeException ();
			}
		}
		public void SetTextureUniforms ()
		{
			int i = 0;
			foreach (var str in Uniforms.Keys) {
				var u = Uniforms [str];
				if (u.IsTexture) {
					GL.Uniform1 (u.Location, i);
					i++;
				}
			}
		}
	}
}

