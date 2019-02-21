using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksManager : MonoBehaviour
{
    public ParticleSystem Firework1;
    public ParticleSystem Firework2;
    public ParticleSystem[] BasicFireworks;

    private void OnEnable()
    {
        StartCoroutine("StartFirework1");
        StartCoroutine("StartFirework2");
        StartCoroutine("Delay");
    }

    private IEnumerator Delay()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                BasicFireworks[i].Play();
                if (PlayerPrefs.GetFloat("Sound", 0) == 0)
                    BasicFireworks[0].GetComponents<AudioSource>()[0].Play();
                Invoke("Wait1", 1.8f);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void Wait1()
    {
        if (PlayerPrefs.GetFloat("Sound", 0) == 0)
            BasicFireworks[0].GetComponents<AudioSource>()[1].Play();
    }

    private void Wait2()
    {
        if (PlayerPrefs.GetFloat("Sound", 0) == 0)
            Firework1.GetComponents<AudioSource>()[1].Play();
    }

    private void Wait3()
    {
        if (PlayerPrefs.GetFloat("Sound", 0) == 0)
            Firework2.GetComponents<AudioSource>()[1].Play();
    }

    private IEnumerator StartFirework1()
    {
        while (true)
        {
            if (Firework1.isPlaying)
            {
                Firework1.Stop();
                Firework1.Clear();
            }

            Firework1.Play();
            if (PlayerPrefs.GetFloat("Sound", 0) == 0)
                Firework1.GetComponents<AudioSource>()[0].Play();
            Invoke("Wait2", 3.5f);

            yield return new WaitForSeconds(15f);
        }
    }

    private IEnumerator StartFirework2()
    {
        while (true)
        {
            if (Firework2.isPlaying)
            {
                Firework2.Stop();
                Firework2.Clear();
            }

            Firework2.Play();
            if (PlayerPrefs.GetFloat("Sound", 0) == 0)
                Firework2.GetComponents<AudioSource>()[0].Play();
            Invoke("Wait3", 2.4f);
            Invoke("Wait3", 2.6f);
            Invoke("Wait3", 2.8f);

            yield return new WaitForSeconds(8f);
        }
    }
}