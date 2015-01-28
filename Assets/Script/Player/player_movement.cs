using UnityEngine;
using System.Collections;

public class player_movement : MonoBehaviour {
	
	public float speed;
	public GameObject hero;
	public string ori = "right";
	public Vector3 heroSize;
	public float heroSize_x;
	public float heroSize_y;
	private float minx, maxx, miny, maxy;
	
	public int control_choice;
	public GUIText livesText;
	public GUIText scoreText;
	public GUIText scoreMaxText;
	private bool crazy = false;
	public static int lives = 0;
	public Vector3 playerSize;
	public int eatCount = 0;
	public int lvl_req;//number of fish the player needs to eat (multiple of 3)
	public static int score = 0;
	public static int scoreMax;
	private float speed_remember;
	private int level_num;
	private string game_mode;
	
	public float invert = 1;
	
	public Texture growthRemainingTexture; 
	public Texture growthRemainingBehindTexture; 
	public float growthPercent;
	
	private float left;
	private float top;
	private float backgroundWidthGrowth;
	private float growthWidth;
	private float height;
	private Vector3 original_size;
	private float max_length = 15;
	private int button_height = 25;
	private int button_width;
	private bool paused = false;
	private bool pressed = false;
	
	private float time_remaining;
	
	
	void OnGUI(){
	
		
		//this following stuff is for the growth bar	
		if(paused){
			pressed = true;
			
			
				GUILayout.Label("Game is paused!");
				//paused = togglePause();
				if ( GUI.Button (new Rect(10, Screen.height/2 - 50, button_width, button_height), "Resume") )
				{
					paused = togglePause();
					pressed = false;
				}
				if ( GUI.Button (new Rect(10, Screen.height/2 - 20, button_width, button_height), "Quit") )
				{
					//Application.Quit();
					//paused = togglePause();
					//pressed = false;
					Application.LoadLevel("Menu Scene");
					paused = togglePause();
					pressed = false;
				}
			
		}
		else{
			if(GUI.Button(new Rect(Screen.width-100, Screen.height-50, button_width/4, button_height), "Pause") ){
				paused = togglePause();
			}
			if ( game_mode.ToLower() == "growing")
			{
				height = 12;
				left = 10;
				top = 0; 
				GUI.DrawTexture(new Rect(left, top, max_length*(float)lvl_req, height), growthRemainingBehindTexture, ScaleMode.StretchToFill, true, 1.0f);
				GUI.DrawTexture(new Rect(left, top, max_length*(float)eatCount, height), growthRemainingTexture, ScaleMode.StretchToFill, true, 1.0f);
			}
			
			if ( game_mode.ToLower () != "growing" )
				GUI.Box( new Rect( 10, 10, 100, 25 ) , ( (int) time_remaining).ToString () );
		
		}
	}	
	
	// Use this for initialization
	void Start () {
		button_width = Screen.width / 5;
		level_num = PlayerPrefs.GetInt ("level_to_load");
		game_mode = PlayerPrefs.GetString("game_mode");
		control_choice = PlayerPrefs.GetInt("input_mode"); //note that the default is set in menu.cs, and it defaults to touchscreen
		original_size = transform.localScale;

		speed_remember = speed;
		heroSize = collider.bounds.size;
		float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
		Vector2 bot = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
		Vector2 top = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));
		
		minx = bot.x;
        miny = bot.y;
        maxx = top.x;
        maxy = top.y;

		lives = PlayerPrefs.GetInt ("num_lives");
		if ( lives == 0 ) //if non-zero number, it will keep that amount of lives
			if (game_mode.ToLower () == "survival")
				lives = 1;
			else 
				lives = 3;

		if (game_mode.ToLower () == "survival")
			lives = 1;
				
		time_remaining = 30.0f + (30.0f * level_num);
		lvl_req = 3 * level_num;
		scoreMax = 1000*level_num;
	
		if ( game_mode.ToLower() == "scoring")
		{
			gameObject.transform.localScale = new Vector3(transform.localScale.x + 1, transform.localScale.y + 1, transform.localScale.z + 1);
			gameObject.transform.localScale = new Vector3(transform.localScale.x + 1, transform.localScale.y + 1, transform.localScale.z + 1);
			if ( level_num <= PlayerPrefs.GetInt ("max_levels")/2 )
				time_remaining = 90.0f;
			else
				time_remaining = 180.0f;
		}

		updateLives ();
		updateScore ();

	}
	void updateLives()
	{
		PlayerPrefs.SetInt ("num_lives", lives);
		if ( game_mode.ToLower () != "scoring")
			livesText.text = "Lives: " + lives;
		if ( game_mode.ToLower () == "scoring")
			livesText.text = "";

	}

	void updateScore(){
		if (game_mode.ToLower () == "scoring") {
			scoreText.text = "Score: " + score;
			scoreMaxText.text = "Next Level: " + (scoreMax - score);			
		} 
		else {
			scoreText.text = "";
			scoreMaxText.text = "";
		}
	}

	void reloadLevel (){	
		Application.LoadLevel(Application.loadedLevel);
		//DontDestroyOnLoad (gameObject);

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.P) && pressed == false){
			paused = togglePause();
		}
		if(control_choice == 1){
			Vector3 dir1 = Vector3.zero;
			dir1.x = Input.acceleration.x;
			dir1.y = Input.acceleration.y;
			dir1.z = 0;
					
			Vector3 dir3 = -dir1;
			float angle = Mathf.Atan2(dir3.y, dir3.x)*Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle+180f, new Vector3(0,180,0));
			dir1 *= Time.deltaTime;
			//transform.Translate(dir1*speed, Space.World);
			
			Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
			viewPos.x = Mathf.Clamp01(viewPos.x);
			viewPos.y = Mathf.Clamp01(viewPos.y);
			transform.position = Camera.main.ViewportToWorldPoint(viewPos);
			
			dir1.x *= invert;
			dir1.y *= invert;
			
			//transform.Translate(Input.acceleration.x*speed, Input.acceleration.y*speed, 0);
			transform.Translate(dir1*speed, Space.World);
		
		}
		else if(control_choice == 2){
			if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
			
				Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
				viewPos.x = Mathf.Clamp01(viewPos.x);
				viewPos.y = Mathf.Clamp01(viewPos.y);
				transform.position = Camera.main.ViewportToWorldPoint(viewPos);
				Vector2 touchDeltaPosition1 = Input.GetTouch(0).deltaPosition;
				if(touchDeltaPosition1.x > 0){
						if(invert == 1){
							if(ori == "right"){
								rotate_fish(180);
							}
							transform.Translate(touchDeltaPosition1.x*speed*Time.deltaTime, touchDeltaPosition1.y*speed*Time.deltaTime,0);

						}
						else if(invert == -1){
							if(ori == "right"){
								rotate_fish(-180);
							}
							transform.Translate((-1)*touchDeltaPosition1.x*speed*Time.deltaTime, (-1)*touchDeltaPosition1.y*speed*Time.deltaTime,0);

						}
				}
				if(touchDeltaPosition1.x < 0){
					if(invert == 1){
						if(ori == "left"){
							rotate_fish(-180);
						}
						transform.Translate((-1)*touchDeltaPosition1.x*speed*Time.deltaTime, touchDeltaPosition1.y*speed*Time.deltaTime,0);

					}
					else if(invert == -1){
						if(ori == "left"){
							rotate_fish(180);
						}
						transform.Translate(touchDeltaPosition1.x*speed*Time.deltaTime, (-1)*touchDeltaPosition1.y*speed*Time.deltaTime,0);
					}
				}//end if(touch delta
			}	
			
		}
		else{
			/*
			if(Input.GetKey(KeyCode.Space)){
				Destroy(hero_revive);
			}*/
			Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
			viewPos.x = Mathf.Clamp01(viewPos.x);
			viewPos.y = Mathf.Clamp01(viewPos.y);
			transform.position = Camera.main.ViewportToWorldPoint(viewPos);
			if(!crazy){
				if(Input.GetKey(KeyCode.UpArrow)&&transform.position.y < maxy-1.5){
					transform.Translate(0, invert*speed*Time.deltaTime, 0);
				}
				if(Input.GetKey(KeyCode.DownArrow)&&transform.position.y > miny+1.5){
				   transform.Translate(0, invert*(speed*-1)*Time.deltaTime, 0);
				}   
				if(Input.GetKey(KeyCode.LeftArrow)&&transform.position.x > minx+2){
					//transform.Rotate(Vector3.up, -180);
					if(ori == "left"){
						rotate_fish(180);
					}

					transform.Translate(invert*speed*Time.deltaTime, 0, 0);		
				}
				if(Input.GetKey(KeyCode.RightArrow)&&transform.position.x < maxx-2){
					//transform.Rotate(Vector3.up, 180);
					if(ori == "right"){
						rotate_fish(-180);
					}
					transform.Translate(invert*speed*Time.deltaTime, 0, 0);
				}
			}
			else if(crazy){
				if(Input.GetKey(KeyCode.UpArrow)&&transform.position.y > miny+1.5){
					transform.Translate(0, invert*speed*Time.deltaTime, 0);
				}
				if(Input.GetKey(KeyCode.DownArrow)&&transform.position.y < maxy-1.5){
				
				   transform.Translate(0, invert*(speed*-1)*Time.deltaTime, 0);
				}   
				if(Input.GetKey(KeyCode.LeftArrow)&&transform.position.x < maxx-2){
				
					//transform.Rotate(Vector3.up, -180);
					if(ori == "left"){
						rotate_fish(180);
					}

					transform.Translate(invert*speed*Time.deltaTime, 0, 0);		
				}
				if(Input.GetKey(KeyCode.RightArrow) && transform.position.x > minx+2){					
					//transform.Rotate(Vector3.up, 180);
					if(ori == "right"){
						rotate_fish(-180);
					}
					transform.Translate(invert*speed*Time.deltaTime, 0, 0);
				}
			}
		}

		if (game_mode.ToLower() != "growing") {
					time_remaining -= Time.deltaTime;
				}
		if ( (eatCount >= lvl_req && game_mode.ToLower() == "growing") 
		    || ( game_mode.ToLower () == "survival" && time_remaining <= 0 ) 
		    || (score >= scoreMax && time_remaining > 0 && game_mode.ToLower()=="scoring")
		   )
		{
			PlayerPrefs.SetInt("level_complete", 1);
			PlayerPrefs.Save ();
			Application.LoadLevel ("splash");
			score = 0;
		}
		
		if ( lives <= 0)
		{
			PlayerPrefs.SetInt ("level_complete", 0);
			Application.LoadLevel ("splash");
			score = 0;
		}
		if (score < scoreMax && time_remaining <= 0 && game_mode.ToLower () == "scoring" ) 
		{
			PlayerPrefs.SetInt ("level_complete", 0);
			Application.LoadLevel ("splash");
			score = 0;
		}
		
		if ( game_mode.ToLower () != "growing" && game_mode.ToLower () != "survival" )
			eatCount = 50;
		
	}//end update
	void rotate_fish(int x){
		transform.Rotate(Vector3.up, x, Space.World);
		if(ori == "left"){
			ori = "right";
		}
		else{
			ori = "left";
		}
	}
	
	IEnumerator Waittt(float time){
		invert = -1;
		yield return new WaitForSeconds(time);
		invert = 1;
		
	}
	IEnumerator Change_speed(float spee){
		speed = spee/4;
		yield return new WaitForSeconds(5);
		speed = speed_remember;
	
	
	}
	void Grow(){
		if (game_mode.ToLower () == "growing") 
			if (eatCount == lvl_req / 3) {
				//Debug.Log ("Grow-3");
				gameObject.transform.localScale = new Vector3 (transform.localScale.x + 1, transform.localScale.y + 1, transform.localScale.z + 1);
			} else if (eatCount == ( (lvl_req*2)/3 ) ) {
				//Debug.Log ("Grow-6");
				gameObject.transform.localScale = new Vector3 (transform.localScale.x + 1, transform.localScale.y + 1, transform.localScale.z + 1);
				
			} else if (eatCount >= lvl_req) {
				//Debug.Log ("Grow-9");
				gameObject.transform.localScale = new Vector3 (transform.localScale.x + 1, transform.localScale.y + 1, transform.localScale.z + 1);
			}
		
	}
	bool togglePause(){
		if(Time.timeScale == 0F){
			Time.timeScale = 1F;
			return(false);
		}
		else {
			Time.timeScale = 0F;
			return(true);
		}
	}
	void OnCollisionEnter(Collision col){
		//Enemy_1
		if(col.gameObject.tag == "Enemy_1"){
			if(game_mode.ToLower()!= "survival"){
			score += 50;
			updateScore();
			++eatCount;
			Grow();
			Destroy(col.gameObject);
			}

			else
			--lives;

		}
		
		//Enemy_2
		else if(col.gameObject.tag == "Enemy_2"){
			score += 100;
			updateScore ();
			++eatCount;
			Grow();
			//Debug.Log("Player::eat Enemy_2");
			Destroy(col.gameObject);
		}
		else if(col.gameObject.tag == "Enemy_2"){
			//Debug.Log("Player:: lives subtracted");
			--lives;
			updateLives ();
			if ( lives > 0)
				reloadLevel ();
		}
		
		
		//Enemy_3
		else if(col.gameObject.tag == "Enemy_3" && eatCount >= (lvl_req*2)/3 ){
			score += 200;
			updateScore();
			++eatCount;
			Grow();
			//Debug.Log("Player::eat Enemy_3");
			Destroy(col.gameObject);
			if(ori == "left"){
					rotate_fish(180);
				}
		}
		else if(col.gameObject.tag == "Enemy_3" && eatCount < (lvl_req*2)/3){
			//Debug.Log("Player:: lives subtracted");
			--lives;
			updateLives ();
			if ( lives > 0 )
				reloadLevel ();
		}
		
		//Special_1
		else if(col.gameObject.tag == "Special_1"){
			++lives;
			updateLives ();
			//Debug.Log("Player:: eat special_1::added lives");
			Destroy(col.gameObject);
		}
		
		//Special_2
		else if(col.gameObject.tag == "Special_2" && invert == 1){
			StartCoroutine(Waittt(10));
			Destroy(col.gameObject);	
		}
		
		//Special_3
		if(col.gameObject.tag == "Special_3"){
			StartCoroutine(Change_speed(speed));
			Destroy(col.gameObject);
		
		}
		
		if(col.gameObject.tag == "Special_4"){
			if(eatCount > 0){
				eatCount = 0;
			}
			gameObject.transform.localScale = original_size;
			Destroy(col.gameObject);
		
		}
		
		Debug.Log("{eatCount= " + eatCount +"}{lives: " +lives +"}" +"{" +lvl_req+"}");
	
	}
}