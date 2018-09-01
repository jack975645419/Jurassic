using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SitPlaceColliderListener : MonoBehaviour {

    public BoxCollider m_BoxCollider = null;
    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.isTrigger = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && enabled)
        {
            var pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.DriveAnimal(transform.root.gameObject);
            }
        }
    }
}
