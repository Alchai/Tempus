using UnityEngine;
using System.Collections;

public class OnMouseDownStuff : MonoBehaviour
{
    #region Variables

    public float Seconds = 59;
    private bool CharacterSelected1 = false;
    private bool CharacterSelected2 = false;
    private bool CharacterSelected3 = false;
    private bool CharacterSelected4 = false;
    public static int NumCharSelected = 0;
    public static bool ChoiceLock = false;
    public bool HasLocked = false;

    #endregion
    void Start()
    {
        renderer.material.color = Color.gray;
    }
    void Update()
    {
        if (Application.loadedLevelName.Contains("Character"))
        {
            if (ChoiceLock == true && Seconds > 0)
            {
                if (HasLocked == false)
                {
                    Seconds = 5;
                    HasLocked = true;
                }
                Seconds -= Time.deltaTime;
                GameObject.Find("Timer Text").guiText.text = Seconds.ToString("f0");
            }
            else if (Seconds > 0)
            {
                Seconds -= Time.deltaTime;
                GameObject.Find("Timer Text").guiText.text = Seconds.ToString("f0");
            }


            if (Mathf.Round(Seconds) <= 0)
            {
                GameObject.Find("Timer Text").guiText.text = Seconds.ToString("f0");

            }
        }
    }

    public int currentSelection = 0;

    private void Deselect(string who)
    {
        //de-select all others
        //change colors accordingly
        //change public currentSelection int to 0 you just chose
    }

    private void Select(string who)
    {
        //de-select all others
        //change colors accordingly
        //change public currentSelection int to whichever you just chose
    }

    void OnMouseDown()
    {
        if (name.Contains("Caveman"))
        {
            if (currentSelection == 1)
                Deselect("Caveman");
            else
                Select("Caveman");
        }

        //Main Menu
        if (tag == "Play")
            Application.LoadLevel("CharacterSelect");

        else if (tag == "Credits")
            print("Credits Switch");

        if (tag == "Exit" || tag == "Exit Cube")
            Application.Quit();

        //Lock Choices
        if (tag == "Lock Choices" && NumCharSelected == 2)
        {
            ChoiceLock = true;
        }



        //THIS STUFF WILL BE MOVED TO THE SELECTION FUNCTIONS
        //THIS STUFF WILL BE MOVED TO THE SELECTION FUNCTIONS
        //THIS STUFF WILL BE MOVED TO THE SELECTION FUNCTIONS

        ////HighLight Selected button
        //Color SelectedColor = new Color(.6f, .8f, .6f, 1f);

        //if (CharacterSelected1 && tag == "Character 1")
        //    renderer.material.color = SelectedColor;

        //if (CharacterSelected2 && tag == "Character 2")
        //    renderer.material.color = SelectedColor;

        //if (CharacterSelected3 && tag == "Character 3")
        //    renderer.material.color = SelectedColor;

        //if (CharacterSelected4 && tag == "Character 4")
        //    renderer.material.color = SelectedColor;

        //if (ChoiceLock && tag == "Lock Choices")
        //    renderer.material.color = SelectedColor;
    }

    void OnMouseEnter()
    {
        Color newcol = new Color(.6f, .8f, .6f, 1f);
        renderer.material.color = newcol;
    }

    void OnMouseExit()
    {
        if (CharacterSelected1 == false && CharacterSelected2 == false && CharacterSelected3 == false && CharacterSelected4 == false)
            renderer.material.color = Color.gray;
    }
}
