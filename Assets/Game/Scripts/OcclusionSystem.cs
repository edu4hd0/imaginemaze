using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OcclusionSystem : MonoBehaviour {

    public int minHoleSizeInPixels = 5;
    public float overlapSphereSize = 15f;
    Camera cam;
    List<Renderer> renderersEnabled =  new List<Renderer>();
    LayerMask layerHided, layerHidedFilter, layerDefault, layerDefaultFilter, layerHidedDefaultFilter;
    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        renderersEnabled.AddRange(FindObjectsOfType<Renderer>());
        layerHided = LayerMask.NameToLayer("Hided");
        layerHidedFilter = 1 << layerHided;
        layerDefault = LayerMask.NameToLayer("Default");
        layerDefaultFilter = 1 << layerDefault;
        layerHidedDefaultFilter = layerHidedFilter | layerDefaultFilter;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (minHoleSizeInPixels <= 0)
            minHoleSizeInPixels = 1;

        for (int i = 0; i < renderersEnabled.Count; i++)
        {
            if (renderersEnabled[i].enabled)
            {
                renderersEnabled[i].enabled = false;
                renderersEnabled[i].gameObject.layer = layerHided;
            }
        }
        renderersEnabled.Clear();
        Vector3 camForward = transform.forward;
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, 4.9f, pos.z);
        transform.forward = new Vector3(transform.forward.x, 0 , transform.forward.z).normalized;
        for (int j = 0; j < Screen.width / minHoleSizeInPixels; j++)
        {
            Ray ray = cam.ScreenPointToRay(new Vector2(j * minHoleSizeInPixels, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, layerHidedDefaultFilter))
            {
                Collider[] c = Physics.OverlapSphere(hit.point, overlapSphereSize, layerHidedFilter);
                for(int i = 0; i< c.Length; i++)
                {
                    Renderer r = c[i].GetComponent<Renderer>();
                    if (r && !r.enabled)
                    {
                        r.gameObject.layer = layerDefault;
                        renderersEnabled.Add(r);
                    }
                }
            }
        }
        transform.forward = camForward;
        transform.position = pos;
 
        for (int i = 0; i < renderersEnabled.Count; i++)
        {
            renderersEnabled[i].enabled = true;
        }
    }
}
