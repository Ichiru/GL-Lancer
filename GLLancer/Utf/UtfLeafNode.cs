//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
using System;
using OpenTK;
using OpenTK.Graphics;
namespace GLLancer
{
	public class UtfLeafNode : UtfNode
	{
		byte[] data;
		public int? Int32 {
			get {
				if(data.Length == 4)
					return BitConverter.ToInt32 (data,0);
				else
					return null;
			}
		}
		public byte[] ByteArray {
			get {
				return data;
			}
		}
		public int[] Int32Array {
			get {
				if(data.Length % 4 != 0)
					return null;
				else {
					int len = data.Length / 4;
					int[] array = new int[len];
					for(int i = 0; i < len;i++) {
						array[i] = BitConverter.ToInt32 (data,i * 4);
					}
					return array;
				}
			}
		}
		public float? Single {
			get {
				if(data.Length == 4)
					return BitConverter.ToSingle (data,0);
				else
					return null;
			}
		}
		public float[] SingleArray {
			get {
				if(data.Length % 4 != 0)
					return null;
				else {
					int len = data.Length / 4;
					float[] array = new float[len];
					for(int i = 0; i < len;i++)
					{
						array[i] = BitConverter.ToSingle (data,i * 4);
					}
					return array;
				}
			}
		}
		public Vector3? Vector3 {
			get {
				if(data.Length == 12)
				{
					float[] array = SingleArray;
					return new Vector3(array[0],array[1],array[2]);
				}
				else
					return null;
			}
		}
		public Color4? Color {
			get {
				if(data.Length == 12)
				{
					float[] array = SingleArray;
					return new Color4(array[0],array[1],array[2],1f);
				}
				else
					return null;
			}
		}
		public UtfLeafNode (string name, int peerOffset) : base(name,peerOffset)
		{
		}
		public void SetData(byte[] data)
		{
			this.data = data;
		}
	}
}

