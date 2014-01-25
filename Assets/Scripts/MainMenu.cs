using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    //Updates every frame
    void Update()
    {

    }

   public GUISkin myskin;
   public Texture2D background, LOGO;

   private string clicked =  "";
   public string messageToDisplayOnClick = "Credits \n Press Esc to return";

    private void OnGUI()
    {
        if (background != null)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        if (clicked == "" || clicked == "options")
        {
            if (LOGO != null)
                GUI.DrawTexture(new Rect((Screen.width) / 2 - 100, 75, 200, 200), LOGO);
        }

        if (clicked == "")
        {
           
            GUI.skin = myskin;
            if (GUI.Button(new Rect((Screen.width / 2) - 150, Screen.height / 2, 300, 50), "Find Match"))
            {

            }

            GUI.skin = myskin;
            if (GUI.Button(new Rect((Screen.width / 2) - 150, (Screen.height / 2) + 75, 300, 50), "Credits"))
            {
                //application.loadlevel("Credits");
            }

            GUI.skin = myskin;
            if (GUI.Button(new Rect((Screen.width / 2) - 150, (Screen.height / 2) + 150, 300, 50), "Quit"))
            {

            }
        }
        
    }


}

