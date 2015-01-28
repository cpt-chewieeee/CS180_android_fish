using UnityEngine;
using System.Collections;

//create a new scene
//add this in by going to hierarchy, select main camera
//uncheck everything, then Add New Component >> Script
//select this file
//change the names of load levels to the names of the scenes we're using


	public class menu : MonoBehaviour {

	private bool is_main_menu = true;
	private bool is_level_select = false;
	private bool is_growing_mode = false;
	private bool is_scoring_mode = false;
	private bool is_control_select = false;
	
	private int max_unlocked_growing, max_unlocked_survival, max_unlocked_scoring;
	private const int max_levels = 10; //limiting outselves to only 10 levels for each mode for demo purposes
	private int button_width; // 
	private int button_height = 25; //this may change later
	public Texture bg_png; 
	// Use this for initialization
	void Start () {
		
		//use the following line when testing anew. it removes all player preferences
		//PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt ("max_levels", max_levels);
		int i = PlayerPrefs.GetInt("input_mode", 1); //defaults to touchscreen if not set
		PlayerPrefs.SetInt("input_mode", i); 
		
		//the default value of unset playerprefs is 0, which usually means the game was never staretd
		if ( PlayerPrefs.GetInt ("max_unlocked_growing" ) == 0)
			PlayerPrefs.SetInt ("max_unlocked_growing", 1);
			
		if ( PlayerPrefs.GetInt ( "max_unlocked_survival") == 0)
			PlayerPrefs.SetInt ("max_unlocked_survival", 1);
		
		if ( PlayerPrefs.GetInt ("max_unlocked_scoring") == 0 )
			PlayerPrefs.SetInt ("max_unlocked_scoring", 1);
			
		
		//don't have to care about most recent value in game_mode, it will change as needed later in code
		PlayerPrefs.SetString("game_mode", "survival"); //defaults to survival mode
		
		//this is the int variable I use to a void repeated calls to playerPrefs.
		max_unlocked_growing = PlayerPrefs.GetInt("max_unlocked_growing");
		max_unlocked_scoring = PlayerPrefs.GetInt ("max_unlocked_scoring");
		//max_unlocked_growing = 7;
		//max_unlocked_scoring = 2;
		max_unlocked_survival = PlayerPrefs.GetInt ("max_unlocked_survival"); 
		
		//check to make sure user somehow doesn't unlock levels that don't exist.
		//currently limit the game to 20 levels. May have more later
		if ( max_unlocked_growing > max_levels )
		{
			max_unlocked_growing = max_levels;
			PlayerPrefs.SetInt ("max_unlocked_growing", max_levels); //defaults to the value in max_unlocked_growing
		}
		if ( max_unlocked_scoring > max_levels)
		{
			max_unlocked_scoring = max_levels;
			PlayerPrefs.SetInt ("max_unlocked_scoring", max_unlocked_scoring);
		}
		if ( max_unlocked_survival > max_levels)
		{
			max_unlocked_survival = max_levels;
			PlayerPrefs.SetInt("max_unlocked_survival", max_unlocked_survival);
		}
		
		//this is for a changing button size. Buttons shrink as more levels unlocked
		//Note, currently removed because we're dealing with only one column of buttons.
		//if ( max_unlocked_level < 5)
		//	button_width = Screen.width / max_unlocked_level;
		//else
		button_width = Screen.width / 5;

	}
	
	// Update is called once per frame
	void Update () {
		//left empty
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(1, 1, Screen.width, Screen.height), bg_png, ScaleMode.ScaleToFit, true, 0F);
		main_menu ();
		level_select ();
		control_select ();
		if (is_level_select || is_control_select) {
			if (GUI.Button (new Rect (10, Screen.height - 35, 150, button_height), "Back")) {
				is_level_select = false;
				is_main_menu = true;
				is_control_select = false;
				PlayerPrefs.SetInt ("num_lives", 0);
			}
		}//end if

	}//end onGui

	void main_menu()
	{
		if ( is_main_menu)
		{
			if ( GUI.Button( new Rect(10, Screen.height /2 -100, button_width, button_height), "Growing") )
			{
				is_main_menu = false;
				is_level_select = true;
				is_growing_mode = true;
				is_control_select = false;
			}
			if ( GUI.Button( new Rect(10, Screen.height/2 - 75, button_width, button_height), "Survival") )
			{
				is_main_menu = false;
				is_level_select = true;
				is_growing_mode = false;
				is_scoring_mode = false;
				is_control_select = false;
			}
			if ( GUI.Button( new Rect(10, Screen.height/2 - 50, button_width, button_height), "Scoring") )
			{
				is_main_menu = false;
				is_level_select = true;
				is_scoring_mode = true;
				is_control_select = false;
			}
			if ( GUI.Button ( new Rect(10, Screen.height/2 - 25, button_width, button_height), "Change Controls") )
			{
				is_main_menu = false; //really, only main_menu = false is required, but i put the other two
				is_level_select = false; //in as a safeguard
				is_scoring_mode = false;
				is_control_select = true;
			}
			if ( GUI.Button( new Rect(10, Screen.height/2, button_width, button_height), "Quit") )
			{
				//no need for safeguard here. Just save preferences, and quit.
				
				PlayerPrefs.SetString("game_mode", "survival"); //defaults to survival mode;
				PlayerPrefs.SetInt ( "level_to_load", 0);
				PlayerPrefs.Save ();
				Application.Quit ();
			}
		}//end if
	}

	void level_select()
	{
		if ( is_level_select)
		{
			//note that the back button is provided by the main_menu()
			if ( is_growing_mode )
			{
				for ( int x = 1; x <= max_unlocked_growing; ++x)
				{
					if ( GUI.Button (new Rect( 10, (button_height + 1) * x, button_width, button_height), "Level " + x ) )
					{
						PlayerPrefs.SetInt ("level_to_load", x);
						PlayerPrefs.SetString ("game_mode", "growing");
						PlayerPrefs.SetInt ("num_lives", 3);
						Application.LoadLevel ("test"); //test is our actual game scene
					}
				}
			}
			else if ( is_scoring_mode)
			{
				for ( int x = 1; x <= max_unlocked_scoring; ++x)
					if ( GUI.Button (new Rect( 10, (button_height + 1) * x, button_width, button_height), "Level " + x ) )
					{	
						PlayerPrefs.SetInt ("level_to_load", x);
						PlayerPrefs.SetString ("game_mode", "scoring");
						PlayerPrefs.SetInt ("num_lives", 3);
						
						Application.LoadLevel ("test"); //CHANGE ME
					}
			}
			else
			{
				for ( int x = 1; x <= max_unlocked_survival; ++x)
					if ( GUI.Button (new Rect( 10, (button_height + 1) * x, button_width, button_height), "Level " + x ) )
					{	
						PlayerPrefs.SetInt ("level_to_load", x);
						PlayerPrefs.SetString ("game_mode", "survival");
						PlayerPrefs.SetInt ("num_live", 1);
						Application.LoadLevel ("test"); //CHANGE ME
					}
			}
			
		}//end if
		
	}//end level select
	
	void control_select()
	{
		//see comments in player_movement.cs for numbers
		if ( is_control_select )
		{
			if ( GUI.Button (new Rect(10, Screen.height/2 - 25, button_width, button_height), "Keyboard") )
			{
				PlayerPrefs.SetInt("input_mode", 0);
				GUI.Box (new Rect(10, Screen.height/2 - 100, button_width, button_height), "Changed to Keyboard");

			}
			if ( GUI.Button (new Rect(10, Screen.height/2 - 50, button_width, button_height), "Accelerometer") )
			{
				PlayerPrefs.SetInt ("input_mode", 1);
				GUI.Box (new Rect(10, Screen.height/2 - 100, button_width, button_height), "Changed to Accelerometer");
			}
			if ( GUI.Button (new Rect(10, Screen.height/2 - 75, button_width, button_height), "Touchscreen") )
			{
				PlayerPrefs.SetInt ("input_mode", 2);
				GUI.Box (new Rect(10, Screen.height/2 - 100, button_width, button_height), "Changed to Touchscreen");
			}
		}//end if
	}//end control_select

}//end class