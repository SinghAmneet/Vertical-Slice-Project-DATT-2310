using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public double hitTime;
    public float approachDuration = 0.6f;

    public Transform hitCircle;
    public Transform approachCircle;

    private SongController songController;
    private bool judged = false;

    void Start()
    {
        songController = FindObjectOfType<SongController>();
    }

    void Update()
    {
        if (judged) return;

        double songTime = songController.GetSongTime();
        double timeUntilHit = hitTime - songTime;

        float progress = 1f - (float)(timeUntilHit / approachDuration);
        progress = Mathf.Clamp01(progress);

        float easedProgress = progress * progress;
        float scale = Mathf.Lerp(1.6f, 0.5f, easedProgress);
        approachCircle.localScale = Vector3.one * scale;

        if (timeUntilHit < -0.15f)
        {
            Miss();
        }
    }

    public void Judge(double offset)
    {
        judged = true;
        Destroy(gameObject);
    }

    void Miss()
    {
        judged = true;
        Destroy(gameObject);
    }
}
