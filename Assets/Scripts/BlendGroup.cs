using UnityEngine;
using System.Collections;

public class BlendGroup : MonoBehaviour
{

    private Player player1;
    private Player player2;
    private GameObject fader;
    public Texture2D[] textures;


    public void InitializeBlend()
    {

        fader = GameObject.Find("Fader");

        if (name.Contains("SM_Left_Platform"))
            fader.GetComponent<AlphaFade>().FadeOut();

        // Initialize
        player1 = GameObject.Find("me").GetComponent<Player>();
        player2 = GameObject.Find("them").GetComponent<Player>();
        int index = 0;


        // Initialize the main texture for each child to the texture relative to the character
        if (player1.tag == "Character 1")
            index = 0;
        else if (player1.tag == "Character 2")
            index = 1;
        else if (player1.tag == "Character 3")
            index = 2;
        else
            index = 3;
        renderer.material.SetTexture("_MainTex", textures[index]);

        // Initialize the blend texture for each child to the texture relative to the enemy
        if (player2.tag == "Character 1")
            index = 0;
        else if (player2.tag == "Character 2")
            index = 1;
        else if (player2.tag == "Character 3")
            index = 2;
        else
            index = 3;
        renderer.material.SetTexture("_BlendTex", textures[index]);

        ////float LerpValue = 1.0f - player1.GetWinBackground();
        ////renderer.material.SetFloat("_LerpValue", LerpValue);
    }

    // Update is called once per frame
    void Update()
    {
        //float LerpValue = 1.0f - player1.GetWinBackground();
        //renderer.material.SetFloat(_LerpValue, LerpValue);
    }
}
