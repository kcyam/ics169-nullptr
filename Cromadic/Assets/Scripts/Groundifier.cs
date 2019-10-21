using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Groundifier : MonoBehaviour {
	
	public bool groundify;
	public bool reset;
	public Texture2D noise;
	
	// Use this for initialization
	void Start () {
		groundify = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(groundify && gameObject.tag != "GroundBlock")
		{
			groundify = false;
			if(noise == null)
			{
				Debug.Log("NOISE IS NULL!");
				return;
			}
			
			GameObject[] groundBlocks = GameObject.FindGameObjectsWithTag("GroundBlock");
			
			for(int i = 0; i < groundBlocks.Length; i++)
			{
				Groundify(groundBlocks[i]);
			}
		}
		
		else if(groundify && gameObject.tag == "GroundBlock")
		{
			groundify = false;
			if(noise == null)
			{
				Debug.Log("NOISE IS NULL!");
				return;
			}
			
			Groundify(gameObject);
		}
		
		else if(reset && gameObject.tag != "GroundBlock")
		{
			reset = false;
			GameObject[] groundBlocks = GameObject.FindGameObjectsWithTag("GroundBlock");
			
			foreach(GameObject g in groundBlocks)
			{
				g.GetComponent<CubeDetailer>().UpdateMesh();
				g.GetComponent<CubeDetailer>().canUpdate = true;
			}
		}
		
		else if(reset && gameObject.tag == "GroundBlock")
		{
			reset = false;
			
			gameObject.GetComponent<CubeDetailer>().UpdateMesh();
			gameObject.GetComponent<CubeDetailer>().canUpdate = true;
		}
	}
	
	void Groundify( GameObject g )
	{
		g.GetComponent<CubeDetailer>().canUpdate = false;
		
		float jaggedness = 0.1f;
		float seed = 248.5f;
		
		Vector3[] oldVertices = g.GetComponent<MeshFilter>().mesh.vertices;
		List<Vector3> vertices = new List<Vector3>();
		Mesh newMesh = new Mesh();
		for(int i = 0; i < oldVertices.Length; i++)
		{
			Vector3 vert = oldVertices[i];
			Vector3.Scale(vert, g.transform.localScale);
			Vector3 newVert;
			
			Vector3 worldPos = g.transform.TransformPoint(vert);
			float tX = Mathf.Abs(worldPos.x * seed) % 1;
			float tY = Mathf.Abs(worldPos.y * worldPos.y / worldPos.z * seed) % 1;
			float tZ = Mathf.Abs(worldPos.z * worldPos.x * worldPos.y * seed) % 1;
			
			Vector3 noiseAdd;
			noiseAdd.x = (noise.GetPixel((int)Mathf.Round(tX*512), (int)Mathf.Round(tY*512)).r * 2 - 1) * jaggedness * 2;
			noiseAdd.y = (noise.GetPixel((int)Mathf.Round(tY*512), (int)Mathf.Round(tZ*512)).r * 2 - 1) * jaggedness;
			noiseAdd.z = (noise.GetPixel((int)Mathf.Round(tZ*512), (int)Mathf.Round(tY*512)).r * 2 - 1) * jaggedness * 2;
			
			newVert = new Vector3( worldPos.x + noiseAdd.x, worldPos.y + noiseAdd.y, worldPos.z + noiseAdd.z );
			//newVert = new Vector3(worldPos.x, worldPos.y, worldPos.z);
			Vector3.Scale(newVert, new Vector3(1/transform.localScale.x, 1/transform.localScale.y, 1/transform.localScale.z));
			newVert = g.transform.InverseTransformPoint(newVert);
			
			vertices.Add(newVert);
		}
		
		newMesh.name = "Custom Ground Mesh";
		newMesh.vertices = vertices.ToArray();
		newMesh.triangles = g.GetComponent<MeshFilter>().mesh.triangles;
		RecalculateNormalsSmooth(newMesh, 90);
		//newMesh.RecalculateNormals();
		//newMesh.RecalculateTangents();
		
		g.GetComponent<MeshFilter>().mesh = newMesh;
	}
	
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	//NORMAL RECALCULATOR BY CHARIS MARANGOS
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	 public void RecalculateNormalsSmooth(Mesh mesh, float angle) {
        var cosineThreshold = Mathf.Cos(angle * Mathf.Deg2Rad);
 
        var vertices = mesh.vertices;
        var normals = new Vector3[vertices.Length];
 
        // Holds the normal of each triangle in each sub mesh.
        var triNormals = new Vector3[mesh.subMeshCount][];
 
        var dictionary = new Dictionary<VertexKey, List<VertexEntry>>(vertices.Length);
 
        for (var subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; ++subMeshIndex) {
            
            var triangles = mesh.GetTriangles(subMeshIndex);
 
            triNormals[subMeshIndex] = new Vector3[triangles.Length / 3];
 
            for (var i = 0; i < triangles.Length; i += 3) {
                int i1 = triangles[i];
                int i2 = triangles[i + 1];
                int i3 = triangles[i + 2];
 
                // Calculate the normal of the triangle
                Vector3 p1 = vertices[i2] - vertices[i1];
                Vector3 p2 = vertices[i3] - vertices[i1];
                Vector3 normal = Vector3.Cross(p1, p2).normalized;
                int triIndex = i / 3;
                triNormals[subMeshIndex][triIndex] = normal;
 
                List<VertexEntry> entry;
                VertexKey key;
 
                if (!dictionary.TryGetValue(key = new VertexKey(vertices[i1]), out entry)) {
                    entry = new List<VertexEntry>(4);
                    dictionary.Add(key, entry);
                }
                entry.Add(new VertexEntry(subMeshIndex, triIndex, i1));
 
                if (!dictionary.TryGetValue(key = new VertexKey(vertices[i2]), out entry)) {
                    entry = new List<VertexEntry>();
                    dictionary.Add(key, entry);
                }
                entry.Add(new VertexEntry(subMeshIndex, triIndex, i2));
 
                if (!dictionary.TryGetValue(key = new VertexKey(vertices[i3]), out entry)) {
                    entry = new List<VertexEntry>();
                    dictionary.Add(key, entry);
                }
                entry.Add(new VertexEntry(subMeshIndex, triIndex, i3));
            }
        }
 
        // Each entry in the dictionary represents a unique vertex position.
 
        foreach (var vertList in dictionary.Values) {
            for (var i = 0; i < vertList.Count; ++i) {
 
                var sum = new Vector3();
                var lhsEntry = vertList[i];
 
                for (var j = 0; j < vertList.Count; ++j) {
                    var rhsEntry = vertList[j];
 
                    if (lhsEntry.VertexIndex == rhsEntry.VertexIndex) {
                        sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
                    } else {
                        // The dot product is the cosine of the angle between the two triangles.
                        // A larger cosine means a smaller angle.
                        var dot = Vector3.Dot(
                            triNormals[lhsEntry.MeshIndex][lhsEntry.TriangleIndex],
                            triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex]);
                        if (dot >= cosineThreshold) {
                            sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
                        }
                    }
                }
 
                normals[lhsEntry.VertexIndex] = sum.normalized;
            }
        }
 
        mesh.normals = normals;
    }
 
    private struct VertexKey
    {
        private readonly long _x;
        private readonly long _y;
        private readonly long _z;
 
        // Change this if you require a different precision.
        private const int Tolerance = 100000;
 
        // Magic FNV values. Do not change these.
        private const long FNV32Init = 0x811c9dc5;
        private const long FNV32Prime = 0x01000193;
 
        public VertexKey(Vector3 position) {
            _x = (long)(Mathf.Round(position.x * Tolerance));
            _y = (long)(Mathf.Round(position.y * Tolerance));
            _z = (long)(Mathf.Round(position.z * Tolerance));
        }
 
        public override bool Equals(object obj) {
            var key = (VertexKey)obj;
            return _x == key._x && _y == key._y && _z == key._z;
        }
 
        public override int GetHashCode() {
            long rv = FNV32Init;
            rv ^= _x;
            rv *= FNV32Prime;
            rv ^= _y;
            rv *= FNV32Prime;
            rv ^= _z;
            rv *= FNV32Prime;
 
            return rv.GetHashCode();
        }
    }
 
    private struct VertexEntry {
        public int MeshIndex;
        public int TriangleIndex;
        public int VertexIndex;
 
        public VertexEntry(int meshIndex, int triIndex, int vertIndex) {
            MeshIndex = meshIndex;
            TriangleIndex = triIndex;
            VertexIndex = vertIndex;
        }
    }
}
