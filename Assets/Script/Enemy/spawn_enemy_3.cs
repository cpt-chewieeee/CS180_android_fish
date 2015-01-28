using UnityEngine;
using System.Collections;

public class spawn_enemy_3 : MonoBehaviour {

	
	public bool busy = false;
	private  GameObject[] hazards; //changed from public to private
	public GameObject edit_obj;
	private int Spanw_amt;//changed from public to private
	public Vector3 spawnValues;
	public float speed;
	public Transform pt;
	public float time;
	public int fishamt;
	private int fishcnt = 0;
	public float scale_size;
	
	
	public float rad;
	private bool trig = false;
	
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
		spawnValues = new Vector3(spawnValues.x,spawnValues.y-Random.Range(-100f, 100f),spawnValues.z);
		//tmp.transform.localScale = Vector3.one;
	
		//float kmp = Random.Range (.1f, 5f);
		Vector3 scale = new Vector3(scale_size, scale_size, scale_size);//transform.localScale;
		tmp.transform.localScale = scale;
		
		hazards[fishct] = tmp;
		hazards [fishct].tag = "Enemy_3";
		hazards [fishct].AddComponent<Rigidbody> ();
		hazards [fishct].rigidbody.useGravity = false;
		hazards [fishct].rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		MeshCollider hazardsCollider = hazards[fishct].AddComponent<MeshCollider>();
		//hazards [fishct].AddComponent("eatFishEnemy");
		//hazards [fishct].AddComponent("enemy_collision");
		hazards [fishct].AddComponent("ec3");
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

		if (game_mode.ToLower () == "growing") {
						int x = PlayerPrefs.GetInt ("level_to_load"); //i is only set here and in a loop below
						if (x < 2)
								fishamt = 1;
						else if (x < 6)
								fishamt = 2;
						else if (x < 10)
								fishamt = 3;
						else
								fishamt = 3;
				} else {
			fishamt = 10;
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
		game_mode = PlayerPrefs.GetString("game_mode");

	}
	
	

	// Update is called once per frame
	void Update () {
		if (!busy && fishcnt < fishamt) {
						StartCoroutine (WaitSpawn (time,fishcnt));
				}

				for (i = 0; i < Spanw_amt; ++i) {
		
						if(hazards[i] != null){

							Vector3 viewPos = Camera.main.WorldToViewportPoint(hazards[i].transform.position);
							viewPos.x = Mathf.Clamp01(viewPos.x);
							viewPos.y = Mathf.Clamp01(viewPos.y);
							
							hazards[i].transform.position = Camera.main.ViewportToWorldPoint(viewPos);
							//Debug.Log("[" + i + "][x:" + viewPos.x + "][y:" + viewPos.y +"]");
							if(viewPos.x == 1 && oris[i] == 1){
								hazards[i].transform.Rotate(new Vector3(0,180,0));
								oris[i] = 0;
							}
							else if(viewPos.x == 0 && oris[i] == 0){
								hazards[i].transform.Rotate(new Vector3(0,-180,0));
								oris[i] = 1;
							
							}
							if(viewPos.y == 1){
								ydir[i] = -1;
							}
							else if(viewPos.y == 0){
								ydir[i] = 1;
							}
							//Debug.Log(hazards[i].transform.position.x + " " + hazards[i].transform.position.y);
							hazards[i].transform.Translate(new Vector3(speed*Time.deltaTime, ydir[i]*speed*Time.deltaTime, 0));
							Vector3 epos = hazards[i].transform.position;
							Collider[] hitColliders = Physics.OverlapSphere(epos,rad);
							int colcnt = 0;
							while(colcnt < hitColliders.Length){
								if(hitColliders[colcnt].gameObject.tag == "Player"){
									
									float step = speed * Time.deltaTime*2;
									hazards[i].transform.position = Vector3.MoveTowards(hazards[i].transform.position, pt.position, step);
									trig = true;
									Debug.Log("in range");
								}
								colcnt++;
							}
				if(game_mode.ToLower() == "scoring")
					rad =0;

							if(!trig){
								hazards[i].transform.Translate( new Vector3(0,0,0) );//cant multiple by Time.deltaTime on the x-axis 
							}
							else{
								trig = false;
							}
							//hazards[i].transform.Translate( new Vector3(1*Random.Range(1f, 3f)*speed/Random.Range(50f, 70f)
							//											,ydir[i],0) );//cant multiple by Time.deltaTime on the x-axis 
						
							//hazards[i].transform.Translate( new Vector3(1*Random.Range(1f, 3f)*speed/Random.Range(50f, 70f)
																		//,ydir[i]*speed,0) );//cant multiple by Time.deltaTime on the x
						}
						else{
						
						}
				}

	}
	

	
	
	
}
