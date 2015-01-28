using UnityEngine;
using System.Collections;

public class splash : MonoBehaviour {

	private int is_level_complete;
	private float time_to_wait = 3.0f;
	private string game_mode;
	private string s;
	private int max_unlocked;
	private int next_level;
	private bool game_over = false;
	public Texture bg_png;
	public Texture ld_png;
	private bool load_screen = false;
	private int max_level;
	// Use this for initialization
	void Start () {
		max_level = PlayerPrefs.GetInt ("max_levels", 10);
		is_level_complete = PlayerPrefs.GetInt ("level_complete");

		game_mode = PlayerPrefs.GetString ("game_mode");
		s = "max_unlocked_" + game_mode;
		max_unlocked = PlayerPrefs.GetInt (s);
		next_level = PlayerPrefs.GetInt ( "level_to_load") + 1;
		Debug.Log ("next_level:  " + next_level);	
		if ( is_level_complete == 1)
		{
			load_screen = true;
			Debug.Log ("splash::Level complete");
			max_unlocked++;
			if (max_unlocked > max_level)
			{
				max_unlocked = max_level;
			}
			if (next_level > max_level)
				next_level = 0;

			PlayerPrefs.SetInt(s, max_unlocked);
			PlayerPrefs.SetInt ("level_to_load", next_level);
			game_over = false;
			PlayerPrefs.Save ();
		}
		else
		{
			game_over = true;
			load_screen = false;
			//doesn't change max unlocked level
			Debug.Log ("Splash::Level failed");
			PlayerPrefs.SetInt ("num_lives", 0);
			PlayerPrefs.SetInt ("level_to_load", 0); //for safeguarding
			next_level = 0;
			PlayerPrefs.Save();
		}			
	}//end start
	void OnGUI(){
		if(game_over){
			GUI.DrawTexture(new Rect(1, 1, Screen.width, Screen.height), bg_png, ScaleMode.ScaleToFit, true, 0F);
			//game_over = false;
		}
		if(load_screen){
			GUI.DrawTexture(new Rect(1, 1, Screen.width, Screen.height), ld_png, ScaleMode.ScaleToFit, true, 0F);
		}
	
	}
	// Update is called once per frame
	void Update () {
		time_to_wait -= Time.deltaTime;
		if ( time_to_wait <= 0 )
			if ( is_level_complete == 1 && next_level <= max_level && next_level > 0 )
				Application.LoadLevel("test");
			else 
				Application.LoadLevel ("Menu Scene");
	}//end update
}//end class
