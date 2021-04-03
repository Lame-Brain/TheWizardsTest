using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound_Music : MonoBehaviour
{
    public AudioSource[] ambience, music;
    public AudioSource current_ambience, current_music;

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

    public void GetLevelBGM(int i)
    {
        if(i == 1) FadeInBG(1);
    }
}
