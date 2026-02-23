using UnityEngine.UI;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    public GameObject plr;
    public DayCycle dayCycle;
    private ItemSpawner itemSpawner;
    private MobSpawner mobSpawner;

    private CharacterController2D charController;

    private Image menu;

    private bool active = false;

    private void Awake()
    {
        itemSpawner = GetComponent<ItemSpawner>();
        charController = plr.GetComponent<CharacterController2D>();
        mobSpawner = GetComponent<MobSpawner>();
        menu = GetComponent<Image>();
        Toggle(false);
    }

    public void SetSpeed(int speed)
    {
        charController.SetSpeed(speed);
    }

    public void SetTime(int time)
    {
        dayCycle.SetTime(time);
    }

    private void Toggle()
    {
        active = !active;
        Toggle(active);
        
    }

    private void Toggle(bool show)
    {
        active = show;
        foreach (Transform trans in transform)
        {
            trans.gameObject.SetActive(active);
        }

        menu.color = active ? new Color(0, 0, 0, 0.5f) : new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Toggle();
        }
    }
}
