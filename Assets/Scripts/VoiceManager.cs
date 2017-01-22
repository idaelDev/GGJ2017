using UnityEngine;
using System.Collections;

public class VoiceManager : MonoBehaviour {

    public AudioSource[] sources;
    private int currentPlaying = 0;

    public float fadeTime = 0.3f;

    public void VoiceTransition(int newVoice)
    {
        if (newVoice != currentPlaying)
        {
            sources[newVoice].volume = 1;
            sources[currentPlaying].volume = 0;
            currentPlaying = newVoice;
        }
        else if(sources[currentPlaying].volume < 1)
        {
            sources[currentPlaying].volume = 1;
        }
        //    StartCoroutine(transitionCoroutine(state + 1));
    }

    public void Stop()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = 0;
        }
        //StartCoroutine(stopCoroutine());
    }

    IEnumerator transitionCoroutine(int newVoice)
    {
        float timer = 0;
        while(timer < fadeTime)
        {
            float r = Mathf.Lerp(0, 1, timer / fadeTime);
            sources[newVoice].volume = r;
            r = Mathf.Lerp(1, 0, timer / fadeTime);
            sources[currentPlaying].volume = r;
            timer += Time.deltaTime;
            yield return 0;
        }
        sources[newVoice].volume = 1;
        sources[currentPlaying].volume = 0;
        currentPlaying = newVoice;
    }

    IEnumerator stopCoroutine()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            float r = Mathf.Lerp(1, 0, timer / fadeTime);
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].volume != 0)
                    sources[i].volume = r;
            }
            timer += Time.deltaTime;
            yield return 0;
        }
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = 0;
        }
    }
}
