using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite defaultImage;
    public Sprite pressedImage;
    public KeyCode keyPress;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyPress))
        {
            sr.sprite = pressedImage;
        }

        if (Input.GetKeyUp(keyPress))
        {
            sr.sprite = defaultImage;
        }
    }
}
