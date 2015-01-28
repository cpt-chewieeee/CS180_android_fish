using UnityEngine;
using System.Collections;

public class enemy_collision : MonoBehaviour {
	public float gameObjectArea;
	public float colObjectArea;
	// Use this for initialization
	void Start () {	}
	
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Enemy_1" && col.gameObject.tag != "Enemy_2" && col.gameObject.tag != "Enemy_3"){
			//Destroy(col.gameObject);
			//col.gameObject.transform.Translate( new Vector3(0, -180, 0) );
		}
		
	}
}
