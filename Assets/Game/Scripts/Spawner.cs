using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
    public SpawnObject[] stuff;
	// Use this for initialization
    List<Point> availableLocations = new List<Point>();
    List<GameObject>[] spawnedObjects = new List<GameObject>[0];
	void Start () {
        spawnedObjects = new List<GameObject>[stuff.Length];
        for(int i = 0; i< spawnedObjects.Length; i++)
        {
            spawnedObjects[i] = new List<GameObject>();
        }
        int[,] maze = GetComponent<MazeBuilder>().maze;
        int size = GetComponent<MazeBuilder>().size;
        for(int i = 0; i< size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if(maze[i,j] != 0)
                {
                    availableLocations.Add(new Point(i, j));
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < spawnedObjects.Length; i++)
        {
            if(spawnedObjects[i].Count < stuff[i].maxElements)
            {
                GameObject go = Instantiate(stuff[i].element) as GameObject;
                spawnedObjects[i].Add(go);
                Point p = availableLocations[Random.Range(0, availableLocations.Count)];
                float xPos = (p.x * 5) + Random.Range(stuff[i].minPosOffset.x, stuff[i].maxPosOffset.x);
                float yPos = Random.Range(stuff[i].minPosOffset.y, stuff[i].maxPosOffset.y);
                float zPos = (p.y * 5) + Random.Range(stuff[i].minPosOffset.z, stuff[i].maxPosOffset.z);
                go.transform.position = new Vector3(xPos,yPos,zPos);

                go.transform.Rotate(
                        Random.Range(stuff[i].minRotation.x, stuff[i].maxRotation.x),
                        Random.Range(stuff[i].minRotation.y, stuff[i].maxRotation.y),
                        Random.Range(stuff[i].minRotation.z, stuff[i].maxRotation.z)
                      );
                if (stuff[i].sameAxisScale)
                    go.transform.localScale = Vector3.one * Random.Range(stuff[i].minScale.x, stuff[i].maxScale.x);
                else
                    go.transform.localScale = new Vector3(
                            Random.Range(stuff[i].minScale.x, stuff[i].maxScale.x),
                            Random.Range(stuff[i].minScale.y, stuff[i].maxScale.y),
                            Random.Range(stuff[i].minScale.z, stuff[i].maxScale.z)
                          );
                go.transform.parent = transform;
                go.name = stuff[i].name;
            }
        }
	}
}

[System.Serializable]
public class SpawnObject
{
    public string name;
    public GameObject element;
    public int maxElements;
    public bool atCorner;
    public bool forwardedToCenter;
    public bool sameAxisScale;
    public Vector3 minPosOffset = Vector3.zero;
    public Vector3 maxPosOffset = Vector3.zero;
    public Vector3 maxRotation = Vector3.zero;
    public Vector3 minRotation = Vector3.zero;
    public Vector3 maxScale = Vector3.one;
    public Vector3 minScale = Vector3.one;
}
