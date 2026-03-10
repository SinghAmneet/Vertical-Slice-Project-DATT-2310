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
    public int startNumber = 3;
    public float cookDisplayTime = 1.5f;
    public float songStartDelayAfterCook = 1.7f;

    void Start()
    {
        if (songController == null || countdownText == null)
        {
            Debug.LogError("CountdownUI: Missing references.");
            enabled = false;
            return;
        }

        // Start the background transition as soon as countdown begins
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
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "COOK!";

        yield return new WaitForSeconds(songStartDelayAfterCook);
        songController.BeginSong();

        float remaining = cookDisplayTime - songStartDelayAfterCook;
        if (remaining > 0f)
            yield return new WaitForSeconds(remaining);

        countdownText.gameObject.SetActive(false);
    }
}