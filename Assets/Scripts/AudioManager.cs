using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    //private AudioClip[] sounds;
    //private AudioClip[] music;

   static System.Collections.Generic.Dictionary<string, AudioClip> sounds;
   static System.Collections.Generic.Dictionary<string, AudioClip> music;
   static AudioSource p1Source, p2Source, p3Source, p4Source;
   static Player playerRef;
   static Client clientRef;

    [SerializeField]
   private bool mainMenus = false;

	// Use this for initialization
	void Start () 
    {
        AudioSource[] audioSources =  this.GetComponents<AudioSource>();
        p1Source = audioSources[0];
        p2Source = audioSources[1];
        p3Source = audioSources[2];
        p4Source = audioSources[3];

        sounds = new System.Collections.Generic.Dictionary<string, AudioClip>();
        music = new System.Collections.Generic.Dictionary<string, AudioClip>();

        //Load up all sounds/Music
        AudioClip[] tempSoundArray = Resources.LoadAll<AudioClip>("Sounds");
        for(int curSoundIndex = 0; curSoundIndex < tempSoundArray.Length; ++curSoundIndex)
        {
            sounds.Add(tempSoundArray[curSoundIndex].name, tempSoundArray[curSoundIndex]);
        }
        AudioClip[] tempMusArray = Resources.LoadAll<AudioClip>("Music");
        for(int curMusIndex = 0; curMusIndex < tempMusArray.Length; ++curMusIndex)
        {
            music.Add(tempMusArray[curMusIndex].name, tempMusArray[curMusIndex]);
        }

        playBGM();

        //Get a ref to the player
        if (!mainMenus)
        {
            playerRef = GameObject.Find("me").GetComponent<Player>();
            if(playerRef)
                clientRef = playerRef.client;

            MuteBGM();
            //Debug.Log("MUTE");

            if (clientRef)
            {
                if (clientRef.myChar == 0 || clientRef.hisChar == 0)
                {
                    p1Source.mute = false;
                    p1Source.volume = 0.5f;
                }
                if (clientRef.myChar == 1 || clientRef.hisChar == 1)
                {
                    p2Source.mute = false;
                    p2Source.volume = 0.5f;
                }
                if (clientRef.myChar == 2 || clientRef.hisChar == 2)
                {
                    p3Source.mute = false;
                    p3Source.volume = 0.5f;
                }
                if (clientRef.myChar == 3 || clientRef.hisChar == 3)
                {
                    p4Source.mute = false;
                    p4Source.volume = 0.5f;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!mainMenus)
        {
            if (playerRef)
                updateBGM(playerRef.GetWinBackground());
            else
                playerRef = GameObject.Find("me").GetComponent<Player>();
        }
	}

   static public void play(string soundName, float volume, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(sounds[soundName], position, volume);
    }

   static public void playAll()
   {
       if (p1Source)
       {
           p1Source.Play();
       }
       if (p2Source)
       {
           p2Source.Play();
           p2Source.time = p1Source.time;
       }
       if (p3Source)
       {
           p3Source.Play();
           p3Source.time = p2Source.time;
       }
       if (p4Source)
       {
           p4Source.Play();
           p4Source.time = p3Source.time;
       }
   }

    //TODO: put two audiosources on camera, one for each player
    //      Begin playing all sounds on start, set their volume based upon current hp/tugowar level
    //      
    private void playBGM()
    {
        if (p1Source)
        {
            p1Source.Play();
        }
        if (p2Source)
        {
            p2Source.Play();
            p2Source.time = p1Source.time;
        }
        if (p3Source)
        {
            p3Source.Play();
            p3Source.time = p2Source.time;
        }
        if (p4Source)
        {
            p4Source.Play();
            p4Source.time = p3Source.time;
        }
    }

    private void MuteBGM()
    {
        p4Source.mute = p3Source.mute = p2Source.mute = p1Source.mute = true;
    }

    #region Update
    private void updateBGM(float player1Weight)
    {
        if (clientRef.myChar == 0 && clientRef.hisChar == 1)
        {
            p1Source.volume = player1Weight;
            p2Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 0 && clientRef.hisChar == 2)
        {
            p1Source.volume = player1Weight;
            p3Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 0 && clientRef.hisChar == 3)
        {
            p1Source.volume = player1Weight;
            p4Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 1 && clientRef.hisChar == 0)
        {
            p2Source.volume = player1Weight;
            p1Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 1 && clientRef.hisChar == 2)
        {
            p2Source.volume = player1Weight;
            p3Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 1 && clientRef.hisChar == 3)
        {
            p2Source.volume = player1Weight;
            p4Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 2 && clientRef.hisChar == 0)
        {
            p3Source.volume = player1Weight;
            p1Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 2 && clientRef.hisChar == 1)
        {
            p3Source.volume = player1Weight;
            p2Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 2 && clientRef.hisChar == 3)
        {
            p3Source.volume = player1Weight;
            p4Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 3 && clientRef.hisChar == 0)
        {
            p4Source.volume = player1Weight;
            p1Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 3 && clientRef.hisChar == 0)
        {
            p4Source.volume = player1Weight;
            p2Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
        else if (clientRef.myChar == 3 && clientRef.hisChar == 0)
        {
            p4Source.volume = player1Weight;
            p3Source.volume = playerRef.GetMaxLevelState() - player1Weight;
        }
    }
    #endregion
}