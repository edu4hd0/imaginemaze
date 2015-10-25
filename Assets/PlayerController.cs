using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    NavMeshAgent agent;
    public GameObject target;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if(target)
            agent.destination = target.transform.position;
	}
}
