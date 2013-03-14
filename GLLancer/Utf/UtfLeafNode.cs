//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
using System;

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
		public UtfLeafNode (string name, int peerOffset) : base(name,peerOffset)
		{
		}
		public void SetData(byte[] data)
		{
			this.data = data;
		}
	}
}

