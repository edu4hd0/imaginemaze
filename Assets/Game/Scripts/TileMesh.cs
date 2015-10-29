using UnityEngine;
using System.Collections;
using UnityEditor;

public class TileMesh : MonoBehaviour {
	Mesh mesh;
    public string meshName;
	public Rect tile = new Rect(0,0,1,1);

	Vector2 a,b,c,d;

	void Start()
	{
		CheckParameters();
        UpdateTile(tile);
    }
	
	void UpdateTile(Rect r){
		mesh = Instantiate(GetComponent<MeshFilter>().sharedMesh) as Mesh;
		mesh.name = meshName + "_" + tile.width + "x" + tile.height;
		a.x = r.x;
		a.y = r.y;
		b.x = r.x + r.width;
		b.y = r.y + r.height;
		c.x = r.x + r.width;
		c.y = r.y;
		d.x = r.x;
		d.y = r.y + r.height;
		Vector2[] uvs = {
			a ,
			b ,
			c ,
			d
		};
		mesh.uv = uvs;
		GetComponent<MeshFilter>().mesh = mesh;
	}

	bool CheckParameters()
	{
		if (GetComponent<MeshFilter>().sharedMesh == null || (
			GetComponent<MeshFilter>().sharedMesh.uv.Length != 4 &&
			GetComponent<MeshFilter>().sharedMesh.uv.Length != 0))
		{
			Debug.LogWarning("TileMesh need a Quad mesh on MeshFilter to work properly!");
			return false;
		}
		return true;
	}

}
