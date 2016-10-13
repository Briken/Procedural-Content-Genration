using UnityEngine;
using System.Collections;

public class PCGTerrain : MonoBehaviour
{
	public float width = 100.0f;
	public float depth = 100.0f;
	public float maxHeight = 10.0f;

    public int count = 0;

	public int cellsX = 100;
	public int cellsZ = 100;

    public GameObject tree;

	protected Vector3[] vertices;
	protected Color[] colours;
	protected Vector2[] uvs;
	protected Vector3[] normals;
	protected int[] triangles;

	protected Mesh mesh;

	protected void Awake()
	{
		CreateMesh();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void CreateMesh()
	{
		if(mesh == null)
		{
			mesh = new Mesh();
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			meshFilter.mesh = mesh;
		}
		
		int verticesRowCount = cellsX + 1;
		int verticesCount = verticesRowCount * (cellsZ + 1);
		int trianglesCount = 6 * cellsX * cellsZ;


		vertices = new Vector3[verticesCount];
		uvs = new Vector2[verticesCount];
		colours = new Color[verticesCount];
		normals = new Vector3[verticesCount];
		triangles = new int[trianglesCount];

        Color colour = Color.white;
        int vertexIndex = 0;
		for (int z = 0; z <= cellsZ; ++z)
		{
			float percentageZ = (float)z / (float)cellsZ;
			float startZ = percentageZ * depth;

			for (int x = 0; x <= cellsX; x++)
			{
				float percentageX = (float)x / (float)cellsX;
				float startX = percentageX * width;

                //float height = Random.Range(0, maxHeight);
                float height = Mathf.PerlinNoise(x*0.1f, z*0.1f) * maxHeight;
                if (height < 2)
                {
                    colour = Color.blue;
                }
                if (height >=2 && height < 5)
                {
                    colour = Color.yellow;
                }
                if (height >= 5 && height < 8)
                {
                    colour = Color.green;
                    float mod = count % 12;
                    if (mod == 0)
                    {
                        GameObject tempTree = Instantiate(tree);
                        tempTree.transform.position = new Vector3(x, height, z);
                    }
                    count++;
                }
                if (height >= 8 && height < 9)
                {
                    colour = Color.gray;
                }

				vertices[vertexIndex] = new Vector3(startX, height, startZ);
				colours[vertexIndex] = colour;
				uvs[vertexIndex] = new Vector2();		// No texturing so just set to zero
				normals[vertexIndex] = Vector3.up;      // These should be set based on heights of terrain but we can use Recalulated normal on mesh we just them to up and that function will do the rest
				++vertexIndex;
			}
		}
		
		vertexIndex = 0;
		int trianglesIndex = 0;
		for (int z = 0; z < cellsZ; ++z)
		{
			for (int x = 0; x < cellsX; ++x)
			{
				vertexIndex = x + (verticesRowCount * z);

				triangles[trianglesIndex++] = vertexIndex;
				triangles[trianglesIndex++] = vertexIndex + verticesRowCount;
				triangles[trianglesIndex++] = (vertexIndex + 1) + verticesRowCount;
				triangles[trianglesIndex++] = (vertexIndex + 1) + verticesRowCount;
				triangles[trianglesIndex++] = vertexIndex + 1;
				triangles[trianglesIndex++] = vertexIndex;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.colors = colours;
		mesh.normals = normals;
		mesh.RecalculateNormals();
		mesh.UploadMeshData(true);
	}

	public void OnValidate()
	{
		//CreateMesh();
	}
}
