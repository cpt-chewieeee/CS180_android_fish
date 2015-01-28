using UnityEngine;
using System.Collections;

public class ec2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	void OnCollisionEnter(Collision col){
		//eat Enemy_1
		if(col.gameObject.tag != "Enemy_3" && col.gameObject.tag != "Enemy_2" && col.gameObject.tag == "Enemy_1" ){
			Debug.Log("2::eat Enemy_1");
			Destroy(col.gameObject);
		}
		//eat Enemy_2
		if(col.gameObject.tag == "Enemy_2" && col.gameObject.tag != "Enemy_1" && col.gameObject.tag != "Enemy_3"){
			//Destroy(col.gameObject);
			//col.gameObject.transform.Translate( new Vector3(0, -180, 0) );
		}
		if(col.gameObject.tag == "Special_1" || col.gameObject.tag == "Special_2" || col.gameObject.tag == "Special_3"){
			Destroy(col.gameObject);
		}
	}
}
