using UnityEngine;
using System.Collections;

public class CameraFOV : MonoBehaviour {

    Camera cam;
    float fovReference = 60;
    public float fovAccel = 1;
    void Start()
    {
        cam = GetComponent<Camera>();
    }
	
    void Update()
    {
        float currentFov = 60 + (transform.parent.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 2);
        if (currentFov> fovReference)
        {
            if (currentFov < fovAccel * Time.deltaTime + fovReference)
                fovReference = currentFov;
            else
                fovReference += fovAccel * Time.deltaTime;
        } else if (currentFov < fovReference)
        {
            if (currentFov > fovAccel * Time.deltaTime - fovReference)
                fovReference = currentFov;
            else
                fovReference -= fovAccel * Time.deltaTime;
        }
    }

	void LateUpdate () {
        cam.fieldOfView = fovReference;
	}
}
