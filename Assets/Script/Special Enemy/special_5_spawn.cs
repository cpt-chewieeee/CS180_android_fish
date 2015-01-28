using UnityEngine;
using System.Collections;

public class special_5_spawn : MonoBehaviour {
	
	
	public bool busy = false;
	public bool puff = false;

	private  GameObject[] hazards; //changed from public to private
	public GameObject edit_obj;
	private int Spanw_amt;//changed from public to private
	public Vector3 spawnValues;
	public float speed;
	public float time;
	public float timepuff;
	//public float timepuff2;
	public float sixth = (float)1 / (float)6;

	public int fishamt;
	private int fishcnt = 0;
	public float scale_size;
	
	//access to eatFishEnemy
	public static int counter;
	public static GameObject[] hazardsAccess;
	
	private float minx, maxx, miny, maxy;
	public bool[] hflags;
	public int[] ydir;
	public int[] oris;
	public int i = 0;
	
	IEnumerator WaitSpawn(float time, int fishct){
		busy = true;
		GameObject tmp = Instantiate(edit_obj, spawnValues, Quaternion.identity) as GameObject;
		spawnValues = new Vector3(spawnValues.x,spawnValues.y-Random.Range(-50f, 50f),spawnValues.z);
		//tmp.transform.localScale = Vector3.one;
		
		//float kmp = Random.Range (.1f, 5f);
		Vector3 scale = new Vector3(scale_size, scale_size, scale_size);//transform.localScale;
		tmp.transform.localScale = scale;
		
		hazards[fishct] = tmp;
		hazards [fishct].tag = "Enemy_1";
		hazards [fishct].AddComponent<Rigidbody> ();
		hazards [fishct].rigidbody.useGravity = false;
		hazards [fishct].rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		MeshCollider hazardsCollider = hazards[fishct].AddComponent<MeshCollider>();
		//hazards [fishct].AddComponent("eatFishEnemy");
		//hazards [fishct].AddComponent("enemy_collision");
		hazardsCollider.convex = true;
		oris[fishct] = 1;
		ydir[fishct] = 1;//change here
		if(fishcnt != fishamt){++fishcnt;}
		Spanw_amt = fishcnt;
		yield return new WaitForSeconds(time);
		busy = false;
	}

	IEnumerator WaitPuffUp(float timepuff, int fishct)
	{
		if (puff == true) {
			//Debug.Log("UPPER");
			for (int i = 0; i < fishct; i++) {
				// change tag and add component
				if (hazards [i] != null) 
				{
					hazards [i].tag = "Enemy_3";
					hazards [i].AddComponent ("ec3");
					hazards [i].transform.localScale = new Vector3 (scale_size+2, scale_size+2, scale_size+2);				
				}
			}
			yield return new WaitForSeconds(timepuff);
			puff = false;
		}
		if (puff == false)
		{
			//Debug.Log("DOWNER");
			for (int i = 0; i < fishct; i++) {
					// change tag and add component
					if (hazards [i] != null) {
						hazards [i].tag = "Enemy_1";
					Destroy (hazards[i].GetComponent("ec3"));
					hazards [i].transform.localScale = new Vector3 (scale_size, scale_size, scale_size);
					}
			}
			yield return new WaitForSeconds(timepuff);
			puff = true;
		}
	}

		
	void Start () {
		
		string s = PlayerPrefs.GetString("game_mode");
		if ( s.ToLower() != "growing")
			fishamt = 0;
		
	
		hazards = new GameObject[fishamt];
		counter = fishamt;
		hazardsAccess = hazards;
		hflags = new bool[fishamt];
		oris = new int[fishamt];
		ydir = new int[fishamt];
		for(int i = 0; i < fishamt; ++i){
			hflags[i] = false;
		}
		float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
		Vector2 bot = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
		Vector2 top = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));
		minx = bot.x;
		maxx = top.x;
		miny = bot.y;
		maxy = top.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (!busy && fishcnt < fishamt) {
			StartCoroutine (WaitSpawn (time,fishcnt));
		}

		//Debug.Log ("Call WaitPuff");
		//StartCoroutine (WaitSpawn (time,fishcnt));
		//Debug.Log ("Waitpuff Called");


		//if (!puff && fishcnt < fishamt ) {
			StartCoroutine (WaitPuffUp (timepuff, fishcnt));		
		//}


		for (i = 0; i < Spanw_amt; ++i) {
			
			if(hazards[i] != null){
				if(hazards[i].transform.position.x >= minx){
					hflags[i] = true;
				}
				if ((hazards[i].transform.position.x < minx) && hflags[i] && oris[i] == 0) {
					//turn around
					hazards[i].transform.Rotate(new Vector3(0,180,0));
					oris[i] = 1;
					//get new speed
					//speed = Random.Range(6f,10f);
				}
				if(hazards[i].transform.position.x > maxx && oris[i] == 1){
					//turn around
					hazards[i].transform.Rotate(new Vector3(0,180,0));
					oris[i] = 0;
					//get new speed
					//speed = Random.Range(6f,10f);
				}
				if (hazards[i].transform.position.y < miny){
					ydir[i] = 1;
				}
				else if(hazards[i].transform.position.y > maxy){
					ydir[i] = -1;
				}
				/*
							else{
								ydir[i] = 0;
							}*/
				
				
				hazards[i].transform.Translate( new Vector3(1*Random.Range(1f, 3f)*speed/Random.Range(50f, 70f)
				                                            ,ydir[i],0) );//cant multiple by Time.deltaTime on the x-axis 
				
				//hazards[i].transform.Translate( new Vector3(1*Random.Range(1f, 3f)*speed/Random.Range(50f, 70f)
				//,ydir[i]*speed,0) );//cant multiple by Time.deltaTime on the x
			}
			else{
				//do nothing
				//Debug.Log ("hit null");
				//if(!busy){
				//StartCoroutine (WaitSpawn(time,i));
				//}
			}
		}
		
	}
	
	
	
	
}
