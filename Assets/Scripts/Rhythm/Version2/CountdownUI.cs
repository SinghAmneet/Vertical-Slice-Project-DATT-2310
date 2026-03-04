using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownUI : MonoBehaviour
{
    [Header("References")]
    public SongController songController;
    public TMP_Text countdownText;

    [Header("Countdown")]
    public int startNumber = 3;                 // 3, 2, 1
    public float cookDisplayTime = 2f;          // how long "COOK!" stays visible
    public float songStartDelayAfterCook = 2.5f;  // start song X seconds AFTER "COOK!" appears

    void Start()
    {
        if (songController == null || countdownText == null)
        {
            Debug.LogError("CountdownUI: Missing references.");
            enabled = false;
            return;
        }

        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);

        // 3, 2, 1
        for (int i = startNumber; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // Show COOK!
        countdownText.text = "COOK!";

        // Start the song after a short delay (while COOK! is still on screen)
        yield return new WaitForSeconds(songStartDelayAfterCook);
        songController.BeginSong();

        // Keep COOK! visible for the rest of the display time
        float remaining = cookDisplayTime - songStartDelayAfterCook;
        if (remaining > 0f)
            yield return new WaitForSeconds(remaining);

        countdownText.gameObject.SetActive(false);
    }
}