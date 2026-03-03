using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform notesParent;

    public List<NoteData> chart = new List<NoteData>();

    void Start()
    {
        foreach (var note in chart)
        {
            Spawn(note);
        }
    }

    void Spawn(NoteData data)
    {
        GameObject obj = Instantiate(notePrefab, data.position, Quaternion.identity, notesParent);
        obj.GetComponent<NoteObject>().hitTime = data.hitTime;
    }
}
