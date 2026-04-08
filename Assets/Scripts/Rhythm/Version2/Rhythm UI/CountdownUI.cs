using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownUI : MonoBehaviour
{
    [Header("References")]
    public SongController songController;
    public TMP_Text countdownText;
    public BackgroundVisualController backgroundVisualController;

    [Header("Countdown")]
    public bool autoStartCountdown = false;
    public int startNumber;
    public float cookDisplayTime;
    public float songStartDelayAfterCook;

    [Header("Countdown Sound")]
    public AudioSource audioSource;
    public AudioClip popSound;

    private bool countdownStarted = false;

    void Start()
    {
        if (songController == null || countdownText == null)
        {
            //Debug.LogError("CountdownUI: Missing references.");
            enabled = false;
            return;
        }

        countdownText.gameObject.SetActive(false);

        if (autoStartCountdown)
            BeginCountdown();
    }

    public void BeginCountdown()
    {
        if (countdownStarted) return;
        countdownStarted = true;

        //Debug.Log("CountdownUI: BeginCountdown called.");

        if (backgroundVisualController != null)
            backgroundVisualController.StartCountdownFade();

        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = startNumber; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            //Debug.Log("CountdownUI: " + i);

            PlayCountdownSound(i);

            yield return new WaitForSecondsRealtime(1f);
        }

        countdownText.text = "COOK!";
        //Debug.Log("CountdownUI: COOK!");

        PlayCountdownSound(0); // COOK sound

        yield return new WaitForSecondsRealtime(songStartDelayAfterCook);

        //Debug.Log("CountdownUI: Calling SongController.BeginSong()");
        songController.BeginSong();

        float remaining = cookDisplayTime - songStartDelayAfterCook;
        if (remaining > 0f)
            yield return new WaitForSecondsRealtime(remaining);

        countdownText.gameObject.SetActive(false);
    }

    void PlayCountdownSound(int count)
    {
        if (audioSource == null || popSound == null)
            return;

        switch (count)
        {
            case 3:
                audioSource.pitch = 1.0f;
                break;
            case 2:
                audioSource.pitch = 1.1f;
                break;
            case 1:
                audioSource.pitch = 1.2f;
                break;
            case 0: // COOK!
                audioSource.pitch = 1.4f;
                break;
        }
        audioSource.PlayOneShot(popSound);
    }
}