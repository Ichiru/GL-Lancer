using System;
using System.IO;
using System.Collections.Generic;
namespace GLLancer
{
	public class VMeshData
	{
		public uint MeshType;
		public uint SurfaceType;
		public ushort NumMeshes;
		public ushort NumVertices;
		public D3DFVF FlexibleVertexFormat;
		public ushort NumRefVertices;

		public List<TMeshHeader> Meshes = new List<TMeshHeader>();
		public List<TTriangle> Triangles = new List<TTriangle>();
		public List<TVertex> Vertices = new List<TVertex>();

		public VMeshData (UtfLeafNode leaf)
		{
			using(var reader = new BinaryReader(new MemoryStream(leaf)))
			{
				MeshType = reader.ReadUInt32 ();
				SurfaceType = reader.ReadUInt32 ();
				NumMeshes = reader.ReadUInt16 ();
				NumRefVertices = reader.ReadUInt16 ();
				FlexibleVertexFormat = (D3DFVF)reader.ReadUInt16 ();
				NumVertices = reader.ReadUInt16 ();
				switch(FlexibleVertexFormat)
				{
				case D3DFVF.XYZ | D3DFVF.Normal:
				case D3DFVF.XYZ | D3DFVF.Tex1:
				case D3DFVF.XYZ | D3DFVF.Normal | D3DFVF.Tex1:
					break;
				default:
					throw new Exception("Unhandled FVF Format: " + FlexibleVertexFormat.ToString ());
				}
				int triangleStartOffset = reader.ReadUInt32();
				int vertexBaseOffset = reader.ReadUInt32 ();
				for(int i = 0; i < NumMeshes;i++)
				{
					TMeshHeader item = new TMeshHeader();
					item.MaterialCrc = reader.ReadUInt32 ();
					item.StartVertex = reader.ReadUInt16 ();
					item.EndVertex = reader.ReadUInt16 ();
					item.NumRefVertices = reader.ReadUInt16 ();
					item.Padding = reader.ReadUInt16 ();
					item.TriangleStart = triangleStartOffset;
					triangleStartOffset += item.NumRefVertices;
					item.BaseVertex = vertexBaseOffset;
					vertexBaseOffset += item.EndVertex - item.StartVertex + 1;
					Meshes.Add (item);
				}
				int num_triangles = NumRefVertices / 3;
				for(int i = 0; i < num_triangles;i++)
				{
					TTriangle item = new TTriangle();
					item.Vertex1 = reader.ReadUInt16();
					item.Vertex2 = reader.ReadUInt16 ();
					item.Vertex3 = reader.ReadUInt16 ();
					Triangles.Add (item);
				}
				try {
					for(int i = 0; i < NumVertices;i++)
					{
						TVertex item = new TVertex();
						item.FVF = FlexibleVertexFormat;
						item.X = reader.ReadSingle ();
						item.Y = reader.ReadSingle ();
						item.Z = reader.ReadSingle ();
						if((FlexibleVertexFormat & D3DFVF.Normal) == D3DFVF.Normal)
						{
							item.NormalX = reader.ReadSingle ();
							item.NormalY = reader.ReadSingle ();
							item.NormalZ = reader.ReadSingle ();
						}
						if ((FlexibleVertexFormat & D3DFVF.Diffuse) == D3DFVF.Diffuse)
						{
							item.Diffuse = reader.ReadUInt32 ();
						}
						if((FlexibleVertexFormat & D3DFVF.Tex1) == D3DFVF.Tex1)
						{
							item.S = reader.ReadSingle ();
							item.T = reader.ReadSingle ();
						}
						if((FlexibleVertexFormat & D3DFVF.Tex2) == D3DFVF.Tex2)
						{
							item.S = reader.ReadSingle();
							item.T = reader.ReadSingle ();
							item.U = reader.ReadSingle ();
							item.V = reader.ReadSingle ();
						}
						Vertices.Add (item);
					}
				} catch (Exception ex) {
					throw new Exception("Header has more vertices than data");
				}
			}
		}
	}
}