using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Collider))]
public class DestructibleObject : MonoBehaviour {
	bool destroyed = false;

	void Start(){
		for (int i = 0; i< transform.childCount; i++) {
			if(!transform.GetChild(i).GetComponent<Rigidbody>())
				transform.GetChild(i).gameObject.AddComponent<Rigidbody>().isKinematic = true;
			else
				transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;

			if(!transform.GetChild(i).GetComponent<MeshCollider>()){
				transform.GetChild(i).gameObject.AddComponent<MeshCollider>().enabled = false;
			}else{
				transform.GetChild(i).GetComponent<MeshCollider>().enabled = false;
			}
			transform.GetChild(i).gameObject.GetComponent<MeshCollider>().convex = true;
		}
	}

	void Update(){
		if (!destroyed)
			return;
		for (int i = 0; i< transform.childCount; i++) {
			if(transform.GetChild(i).GetComponent<Rigidbody>().velocity.magnitude<=0){
				transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;
				transform.GetChild(i).GetComponent<MeshCollider>().enabled = false;
			}
		}
	}


	void OnCollisionEnter(){
		destroyed = true;
		GetComponent<Collider> ().enabled = false;
		for (int i = 0; i< transform.childCount; i++) {
			transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
			transform.GetChild(i).GetComponent<MeshCollider>().enabled = true;
		}
	}
}
