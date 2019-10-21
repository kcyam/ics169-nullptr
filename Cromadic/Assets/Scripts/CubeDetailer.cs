using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubeDetailer : MonoBehaviour {
	private int gridWidth, gridHeight, gridDepth;
	public bool isWater = false;
	private bool wasWater = false;
	
	public bool reset = false;
	public bool canUpdate = true;
	
	private bool applied = false;
	
	void Start () {
		if(Application.isPlaying)
		{
			//IF THE MESH REVERTS TO A PLAIN CUBE AFTER EXPORTING
			//UNCOMMENT THE FOLLOWING LINE
			//UpdateMesh();
			Destroy(this);
		}
		
		//Debug.Log(canUpdate);
		
		reset = false;
		
		gridWidth = (int)Mathf.Round(transform.lossyScale.x);
		gridHeight = (int)Mathf.Round(transform.lossyScale.y);
		gridDepth = (int)Mathf.Round(transform.lossyScale.z);
		
		//UpdateMesh();
	}
	
	void Update()
	{
		if(Application.isPlaying)
		{
			Destroy(this);
		}
		
		int newGridWidth = Mathf.Max(1, Mathf.Abs((int)Mathf.Round(transform.lossyScale.x)));
		int newGridHeight = Mathf.Max(1, Mathf.Abs((int)Mathf.Round(transform.lossyScale.y)));
		int newGridDepth = Mathf.Max(1, Mathf.Abs((int)Mathf.Round(transform.lossyScale.z)));
		
		if(gridWidth != newGridWidth || gridHeight != newGridHeight || gridDepth != newGridDepth)
		{
			gridWidth = newGridWidth;
			gridHeight = newGridHeight;
			gridDepth = newGridDepth;
			UpdateMesh();
		}
		else if(!wasWater && isWater)
		{
			wasWater = true;
			UpdateMesh();
		}
		else if(wasWater && !isWater)
		{
			wasWater = false;
			UpdateMesh();
		}
		
		else if(reset)
		{
			reset = false;
			UpdateMesh();
		}
	}
	
	public void UpdateMesh()
	{
		if(!canUpdate)
		{
			return;
		}
		Mesh newMesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		int triAmount = (gridWidth * gridHeight * 12) + (gridWidth * gridDepth * 12) + (gridHeight * gridDepth * 12);
		int[] triangles = new int[triAmount];
		
		float xStep = 1f / gridWidth;
		float yStep = 1f / gridHeight;
		float zStep = 1f / gridDepth;
		
		//~~~~~~~~~~~
		//DIVIDE CUBE
		//~~~~~~~~~~~
		
		int triangleIndex = 0;
		int vertIndex = 0;
		//XY PLANE
		for(int x = 0; x < gridWidth; ++x)
		{
			for(int y = 0; y < gridHeight; ++y)
			{
				int norm = 1;
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f + (y*yStep), 0.5f));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f + (y*yStep), 0.5f));
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f + ((y+1)*yStep), 0.5f));
				
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f + (y*yStep), 0.5f));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f + ((y+1)*yStep), 0.5f));
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f + ((y+1)*yStep), 0.5f));
				
				for(int i = 0; i < 6; ++i)
				{
					normals.Add(new Vector3(0,0,norm));
					triangles[triangleIndex++] = vertIndex++;
				}
				
				norm = -1;
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f + ((y+1)*yStep), -0.5f));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f + (y*yStep), -0.5f));
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f + (y*yStep), -0.5f));
				
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f + ((y+1)*yStep), -0.5f));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f + ((y+1)*yStep), -0.5f));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f + (y*yStep), -0.5f));
				
				for(int i = 0; i < 6; ++i)
				{
					normals.Add(new Vector3(0,0,norm));
					triangles[triangleIndex++] = vertIndex++;
				}
			}
		}
		
		//XZ PLANE
		for(int x = 0; x < gridWidth; ++x)
		{
			for(int z = 0; z < gridDepth; ++z)
			{
				int norm = -1;
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f, -0.5f + (z*zStep)));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f, -0.5f + (z*zStep)));
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f, -0.5f + ((z+1)*zStep)));
				
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f, -0.5f + (z*zStep)));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), -0.5f, -0.5f + ((z+1)*zStep)));
				vertices.Add(new Vector3(-0.5f + (x*xStep), -0.5f, -0.5f + ((z+1)*zStep)));
				
				for(int i = 0; i < 6; ++i)
				{
					normals.Add(new Vector3(0,norm,0));
					triangles[triangleIndex++] = vertIndex++;
				}
				
				norm = 1;
				vertices.Add(new Vector3(-0.5f + (x*xStep), 0.5f, -0.5f + ((z+1)*zStep)));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), 0.5f, -0.5f + (z*zStep)));
				vertices.Add(new Vector3(-0.5f + (x*xStep), 0.5f, -0.5f + (z*zStep)));
				
				vertices.Add(new Vector3(-0.5f + (x*xStep), 0.5f, -0.5f + ((z+1)*zStep)));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), 0.5f, -0.5f + ((z+1)*zStep)));
				vertices.Add(new Vector3(-0.5f + ((x+1)*xStep), 0.5f, -0.5f + (z*zStep)));				
				
				for(int i = 0; i < 6; ++i)
				{
					normals.Add(new Vector3(0,norm,0));
					triangles[triangleIndex++] = vertIndex++;
				}
			}
		}
		
		//YZ PLANE
		for(int y = 0; y < gridHeight; ++y)
		{
			for(int z = 0; z < gridDepth; ++z)
			{
				int norm = 1;
				vertices.Add(new Vector3(0.5f, -0.5f + (y*yStep), -0.5f + (z*zStep)));
				vertices.Add(new Vector3(0.5f, -0.5f + ((y+1)*yStep), -0.5f + (z*zStep)));
				vertices.Add(new Vector3(0.5f, -0.5f + (y*yStep), -0.5f + ((z+1)*zStep)));
				
				vertices.Add(new Vector3(0.5f, -0.5f + ((y+1)*yStep), -0.5f + (z*zStep)));
				vertices.Add(new Vector3(0.5f, -0.5f + ((y+1)*yStep), -0.5f + ((z+1)*zStep)));
				vertices.Add(new Vector3(0.5f, -0.5f + (y*yStep), -0.5f + ((z+1)*zStep)));
				
				for(int i = 0; i < 6; ++i)
				{
					normals.Add(new Vector3(norm,0,0));
					triangles[triangleIndex++] = vertIndex++;
				}
				
				norm = -1;
				vertices.Add(new Vector3(-0.5f, -0.5f + (y*yStep), -0.5f + ((z+1)*zStep)));
				vertices.Add(new Vector3(-0.5f, -0.5f + ((y+1)*yStep), -0.5f + (z*zStep)));
				vertices.Add(new Vector3(-0.5f, -0.5f + (y*yStep), -0.5f + (z*zStep)));
				
				vertices.Add(new Vector3(-0.5f, -0.5f + (y*yStep), -0.5f + ((z+1)*zStep)));
				vertices.Add(new Vector3(-0.5f, -0.5f + ((y+1)*yStep), -0.5f + ((z+1)*zStep)));
				vertices.Add(new Vector3(-0.5f, -0.5f + ((y+1)*yStep), -0.5f + (z*zStep)));
				
				for(int i = 0; i < 6; ++i)
				{
					normals.Add(new Vector3(norm,0,0));
					triangles[triangleIndex++] = vertIndex++;
				}
			}
		}
		//~~~~~~~~~~~~~~~
		//END DIVIDE CUBE
		//~~~~~~~~~~~~~~~
		newMesh.name = "Cube";
		newMesh.vertices = vertices.ToArray();
		newMesh.normals = normals.ToArray();
		newMesh.triangles = triangles;
		if(isWater)
		{
			Vector2[] uv = new Vector2[vertices.Count];
			int coord = 0;
			foreach( Vector3 v in vertices )
			{
				uv[coord++] = new Vector2(v.x + 0.5f, v.z + 0.5f);
			}
			newMesh.uv = uv;
		}
		GetComponent<MeshFilter>().mesh = newMesh;
	}
}