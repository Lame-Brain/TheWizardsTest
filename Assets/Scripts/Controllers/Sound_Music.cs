using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound_Music : MonoBehaviour
{
    public AudioSource[] ambience, music;
    public float volumeLimit_SFX, volumeLimit_Music, volumeLimit_Dialogue, volumeLimit_Ambience;

    private AudioSource current_ambience, current_music, battleMusic;
    private List<AudioSource> allSFX, allMusic, allDialogue, allAmbientSFX;
    private bool battleMusicLoaded = false;

    private void Start()
    {
        allSFX = new List<AudioSource>();
        allMusic = new List<AudioSource>();
        allDialogue = new List<AudioSource>();
        allAmbientSFX = new List<AudioSource>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("SFX")) allSFX.Add(go.GetComponent<AudioSource>());
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Music")) allMusic.Add(go.GetComponent<AudioSource>());
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Dialogue")) allDialogue.Add(go.GetComponent<AudioSource>());
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("AmbientSFX")) allAmbientSFX.Add(go.GetComponent<AudioSource>());
    }

    public void FadeOutBG() { StartCoroutine(_FadeOutBG()); }
    IEnumerator _FadeOutBG()
    {
        while(current_ambience.volume > 0 && current_music.volume > 0)
        {
            current_ambience.volume -= .005f;
            current_music.volume -= .005f;
        }
        current_ambience.Stop();
        current_music.Stop();
        yield return 0;
    }

    public void FadeInBG(int i)
    {
        if (i >= ambience.Length) current_ambience = ambience[i];
        if (i < ambience.Length) current_ambience = ambience[i];
        if (i >= music.Length) current_music = music[i];
        if (i < music.Length) current_music = music[i];
        current_ambience.Play();
        current_music.Play();
        current_ambience.volume = 0;
        current_music.volume = 0;
        StartCoroutine(_FadeInBG());
    }
    IEnumerator _FadeInBG()
    {
        while (current_ambience.volume < 1 && current_music.volume < 1)
        {
            current_ambience.volume += .05f;
            current_music.volume += .05f;
            yield return null;
        }
        current_ambience.volume = 1;
        current_ambience.loop = true;
        current_music.volume = 1;
        current_music.loop = true;
        yield return 0;
    }
    IEnumerator _FadeInAudioSource(AudioSource _as)
    {
        battleMusic = _as;
        battleMusic.Play();
        while(battleMusic.volume < 1)
        {
            battleMusic.volume += .05f;
            yield return null;
        }
        battleMusic.volume = 1;
        yield return 0;
    }
    IEnumerator _FadeOutAudioSource()
    {
        while (battleMusic.volume > 0)
        {
            battleMusic.volume -= .05f;
            yield return null;
        }
        battleMusic.volume = 0;
        battleMusic.Stop();
        yield return 0;
    }

    public void GetLevelBGM(int i)
    {
        FadeInBG(i);
    }

    public void StartBattleMusic(AudioSource _as)
    {
        if (!battleMusicLoaded)
        {
            current_ambience.Pause();
            current_music.Pause();
            StartCoroutine(_FadeInAudioSource(_as));
        }
    }
    public void StopBattleMusic()
    {
        StartCoroutine(_FadeOutAudioSource());
        current_ambience.UnPause();
        current_music.UnPause();
        battleMusicLoaded = false;
    }

    private void UpdateSoundLists()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("SFX")) if(!allSFX.Contains(go.GetComponent<AudioSource>())) allSFX.Add(go.GetComponent<AudioSource>());
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Music")) if (!allMusic.Contains(go.GetComponent<AudioSource>())) allMusic.Add(go.GetComponent<AudioSource>());
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Dialogue")) if (!allDialogue.Contains(go.GetComponent<AudioSource>())) allDialogue.Add(go.GetComponent<AudioSource>());
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("AmbientSFX")) if (!allAmbientSFX.Contains(go.GetComponent<AudioSource>())) allAmbientSFX.Add(go.GetComponent<AudioSource>());
    }

    private void Update()
    {
        //TO DO: set these values in playerprefs
        volumeLimit_SFX = .6f;
        volumeLimit_Music = .3f;
        volumeLimit_Ambience = 1f;
        volumeLimit_Dialogue = 1f;

        //Limit Sound volume
        foreach (AudioSource _as in allSFX) if (_as.volume > volumeLimit_SFX) _as.volume = volumeLimit_SFX;
        foreach (AudioSource _as in allMusic) if (_as.volume > volumeLimit_Music) _as.volume = volumeLimit_Music;
        foreach (AudioSource _as in allAmbientSFX) if (_as.volume > volumeLimit_Ambience) _as.volume = volumeLimit_Ambience;
        foreach (AudioSource _as in allDialogue) if (_as.volume > volumeLimit_Dialogue) _as.volume = volumeLimit_Dialogue;


        if (Input.GetKeyUp(KeyCode.P))
        {
            GameObject[] ALL = GameObject.FindGameObjectsWithTag("AmbientSFX");
            foreach (GameObject go in ALL)
            {
                Debug.Log("SFX: " + go.name + " is at volume level: " + +go.GetComponent<AudioSource>().volume);
                if (go.GetComponent<AudioSource>().isPlaying) Debug.Log(go.name + " is playing");
            }

        }
    }
}
