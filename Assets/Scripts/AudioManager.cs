using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    //private AudioClip[] sounds;
    //private AudioClip[] music;

    System.Collections.Generic.Dictionary<string, AudioClip> sounds;
    System.Collections.Generic.Dictionary<string, AudioClip> music;

    AudioSource p1Source, p2Source, mainSource;

	// Use this for initialization
	void Start () 
    {
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

        //playBGM();
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public void play(string soundName, float volume, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(sounds[soundName], position, volume);
    }

    //TODO: put two audiosources on camera, one for each player
    //      Begin playing all sounds on start, set their volume based upon current hp/tugowar level
    //      
    private void playBGM()
    {

    }

    private void updateBGM(float player1Weight, float player2Weight)
    {
        p1Source.volume = player1Weight;
        p2Source.volume = player2Weight;
    }
}