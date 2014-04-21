using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Platform.MacOS;

namespace FLCommon
{
	public class EffectInstance : IDisposable
	{
		//static
		static Dictionary<string, Effect> loaded = new Dictionary<string, Effect>();
		//instance
		Effect effectInternal;
		internal Program ActiveProgram;
		bool isDisposed = false;
		public GraphicsDevice GraphicsDevice { get; private set; }
		public string CurrentTechnique {
			get {
				return ActiveProgram.Name;
			}
			set {
				ActiveProgram = effectInternal.Programs [value];
			}
		}
		Dictionary<string, Effect.Uniform> uniforms;
		static StringHack strhack = new StringHack ();
		public StringHack Techniques
		{
			get {
				return strhack;
			}
		}
		//very dirty hack for XNA-style syntax
		public class StringHack 
		{
			public string this[string index] {
				get {
					return index;
				}
			}
		}
		public EffectInstance(GraphicsDevice device, string filename)
		{
			GraphicsDevice = device;
			if (loaded.ContainsKey (filename)) {
				effectInternal = loaded [filename];
			} else {
				effectInternal = new Effect (filename);
				loaded.Add (filename, effectInternal);
			}
			uniforms = new Dictionary<string, Effect.Uniform> (effectInternal.Uniforms);
		}

		public bool IsDisposed {
			get {
				return isDisposed;
			}
		}

		public void Apply()
		{
			if(ActiveProgram == null) {
				Console.WriteLine ("technique not set, defaulting to {0}", effectInternal.Programs.Keys.First());
				CurrentTechnique = effectInternal.Programs.Keys.First();
			}
			UpdateUniforms ();
			ActiveProgram.ApplyTextures ();
			GraphicsDevice.CurrentEffect = this;
		}
		void UpdateUniforms ()
		{
			GL.UseProgram (ActiveProgram.ID);
			foreach (var k in uniforms.Keys) {
				if (uniforms [k].Value != null && ActiveProgram.Uniforms.ContainsKey (k)) {
					if (uniforms [k].Description.Type == GLSLTypes.Array) {
						var array = (object[])uniforms [k].Value;
						for (int i = 0; i < array.Length; i++) {
							if (array [i] != null) {
								ActiveProgram.SetArrayUniform (k, i, array [i]);
							}
						}
					} else {
						ActiveProgram.SetUniform (k, uniforms [k].Value);
					}
				}
			}
		}

		void InternalSetParameter(string name, GLSLTypes type, object value)
		{
			if (!uniforms.ContainsKey (name))
				throw new KeyNotFoundException (name);
			if (uniforms [name].Description.Type != type)
				throw new InvalidDataException ();
			uniforms [name].Value = value;
		}

		void InternalSetArrayParameter (string name,GLSLTypes type,int index, object value)
		{
			if (!uniforms.ContainsKey (name))
				throw new KeyNotFoundException (name);
			if (uniforms [name].Description.Type != GLSLTypes.Array)
				throw new InvalidOperationException ();
			if (index >= uniforms [name].Description.ArrayLength)
				throw new IndexOutOfRangeException ();
			if (uniforms [name].Description.ArrayType != type)
				throw new InvalidDataException ();
			((object[])uniforms [name].Value) [index] = value;
		}

		public void SetParameter(string name, Vector4 vec)
		{
			InternalSetParameter (name, GLSLTypes.Vector4, vec);
		}

		public void SetParameter (string name, Matrix4 mat)
		{
			InternalSetParameter (name, GLSLTypes.Matrix4, mat);
		}

		public void SetParameter (string name, int i)
		{
			InternalSetParameter (name, GLSLTypes.Int, i);
		}

		public void SetParameter (string name, Vector3 vec)
		{
			InternalSetParameter (name, GLSLTypes.Vector3, vec);
		}

		public void SetParameter (string name, float f)
		{
			InternalSetParameter (name, GLSLTypes.Float, f);
		}

		public void SetParameter (string name, Texture tex)
		{
			if (tex is Texture2D)
				SetParameter (name, (Texture2D)tex);
			else
				SetParameter (name, (TextureCube)tex);
		}

		public void SetParameter (string name, Texture2D tex)
		{
			InternalSetParameter (name, GLSLTypes.Sampler2D, tex);
		}

		public void SetParameter (string name, TextureCube tex)
		{
			InternalSetParameter (name, GLSLTypes.SamplerCube, tex);
		}

		public void SetArrayParameter (string name, int index, Vector4 vec)
		{
			InternalSetArrayParameter (name, GLSLTypes.Vector4, index, vec);
		}

		public void SetArrayParameter (string name, int index, Matrix4 mat)
		{
			InternalSetArrayParameter (name, GLSLTypes.Matrix4, index, mat);
		}

		public void SetArrayParameter (string name, int index, int i)
		{
			InternalSetArrayParameter (name, GLSLTypes.Int, index, i);
		}

		public void SetArrayParameter (string name, int index, Vector3 vec)
		{
			InternalSetArrayParameter (name, GLSLTypes.Vector3, index, vec);
		}

		public void SetArrayParameter (string name, int index, float f)
		{
			InternalSetArrayParameter (name, GLSLTypes.Float, index, f);
		}

		public void Dispose()
		{
			isDisposed = true;
			Console.WriteLine("Attempting to dispose shader. Incorrect behaviour?");
		}
	}
}