using UnityEngine;
using System.Collections;

public class spawn_enemy_2 : MonoBehaviour {

	
	public bool busy = false;
	private  GameObject[] hazards; //changed from public to private
	public GameObject edit_obj;
	private int Spanw_amt;//changed from public to private
	public Vector3 spawnValues;
	public float speed;
	public float time;
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

	private string game_mode;

	IEnumerator WaitSpawn(float time, int fishct){
		busy = true;
		GameObject tmp = Instantiate(edit_obj, spawnValues, Quaternion.identity) as GameObject;
		spawnValues = new Vector3(spawnValues.x,spawnValues.y-Random.Range(-50f, 50f),spawnValues.z);
		//tmp.transform.localScale = Vector3.one;
	
		//float kmp = Random.Range (.1f, 5f);
		Vector3 scale = new Vector3(scale_size, scale_size, scale_size);//transform.localScale;
		tmp.transform.localScale = scale;
		
		hazards[fishct] = tmp;
		hazards [fishct].tag = "Enemy_2";
		hazards [fishct].AddComponent<Rigidbody> ();
		hazards [fishct].rigidbody.useGravity = false;
		hazards [fishct].rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		MeshCollider hazardsCollider = hazards[fishct].AddComponent<MeshCollider>();
		hazards [fishct].AddComponent("enemy_collision");
		//hazards [fishct].AddComponent("eatFishEnemy");
		hazards [fishct].AddComponent("ec2");
		hazardsCollider.convex = true;
		oris[fishct] = 1;
		ydir[fishct] = 1;//change here
		
		if(fishcnt != fishamt){++fishcnt;}
		Spanw_amt = fishcnt;
		yield return new WaitForSeconds(time);
		busy = false;
	}
	
	void Start () {
		game_mode = PlayerPrefs.GetString("game_mode");
		int x = PlayerPrefs.GetInt ("level_to_load"); //i is only set here and in a loop below
		if (game_mode.ToLower() == "growing") {
						switch (x) {
						case 1:
						case 2:
						case 3:
						case 4:
						case 5:
								fishamt = 3;
								break;
						case 6:
						case 7:
						case 8:
						case 9:
						case 10:
								fishamt = 6;
								break;
						default:
								fishamt = 5;
								break;
						}
				}//end switch
		else {
			fishamt = 15;
			}

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

				for (i = 0; i < Spanw_amt; ++i) {
		
						if(hazards[i] != null){
							/*
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
							
							

							hazards[i].transform.Translate( new Vector3(1*Random.Range(1f, 3f)*speed/Random.Range(50f, 70f),ydir[i],0) );*/
							//hazards[i].transform.Translate( new Vector3(1*Random.Range(1f, 3f)*speed/Random.Range(50f, 70f)
																		//,ydir[i]*speed,0) );//cant multiple by Time.deltaTime on the x
							Vector3 viewPos = Camera.main.WorldToViewportPoint(hazards[i].transform.position);
							viewPos.x = Mathf.Clamp01(viewPos.x);
							viewPos.y = Mathf.Clamp01(viewPos.y);
							//print(Mathf.Clamp(10, 1, 3));
							hazards[i].transform.position = Camera.main.ViewportToWorldPoint(viewPos);
							//Debug.Log("[" + i + "][x:" + viewPos.x + "][y:" + viewPos.y +"]");
							if(viewPos.x == 1 && oris[i] == 1){
								hazards[i].transform.Rotate(new Vector3(0,180,0));
								//xdir[i] = -1;
								oris[i] = 0;
								//Debug.Log("[" + i + "][x:" + viewPos.x + "][y:" + viewPos.y +"]");
							}
							else if(viewPos.x == 0 && oris[i] == 0){
								hazards[i].transform.Rotate(new Vector3(0,-180,0));
								//xdir[i] = 1;
								oris[i] = 1;
								//Debug.Log("[" + i + "][x:" + viewPos.x + "][y:" + viewPos.y +"]");
							}
							if(viewPos.y == 1){
								ydir[i] = -1;
								//Debug.Log("[" + i + "][x:" + viewPos.x + "][y:" + viewPos.y +"]");
							}
							else if(viewPos.y == 0){
								ydir[i] = 1;
								//Debug.Log("[" + i + "][x:" + viewPos.x + "][y:" + viewPos.y +"]");
							}
							//Debug.Log(hazards[i].transform.position.x + " " + hazards[i].transform.position.y);
							hazards[i].transform.Translate(new Vector3(speed*Time.deltaTime, ydir[i]*speed*Time.deltaTime, 0));
						
						}
						else{
						//do nothing
							//Debug.Log ("hit null");
							//if(!busy){
								//StartCoroutine (WaitSpawn(time,i));
						
								/*** used in survival mode ***/
								/*
								++j;
								if(j == fishamt){
									Application.Quit();
								}*/
							//}
						}
				}

	}
	
	
	
	
}
