using UnityEngine.UI;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    public GameObject plr;
    public DayCycle dayCycle;

    public FoodData[] items;
    public MobData[] mobs;

    private ItemSpawner itemSpawner;
    private MobSpawner mobSpawner;

    private CharacterController2D charController;
    private Health health;

    private Image menu;

    private bool active = false;

    private void Awake()
    {
        itemSpawner = GetComponent<ItemSpawner>();
        charController = plr.GetComponent<CharacterController2D>();
        mobSpawner = GetComponent<MobSpawner>();
        health = plr.GetComponent<Health>();
        menu = GetComponent<Image>();
        Toggle(false);
    }

    public void SetSpeed(float speed)
    {
        charController.SetSpeed(speed);
    }

    public void SetTime(float time)
    {
        dayCycle.SetTime((int) time);
    }

    public void ToggleInvulnerable(bool invulnerable)
    {
        health.SetInvulnerable(invulnerable);
    }

    public void SpawnItem()
    {
        itemSpawner.SpawnRandom(items, null, plr.transform.position);
    }

    public void SpawnMob()
    {
        mobSpawner.Spawn(mobs[Random.Range(0, mobs.Length)], null, plr.transform.position + Vector3.up * 5);
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Toggle();
        }
    }
}
