using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

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

    public GameObject listPanel;
    private string openedList;

    private bool active = false;

    private void Awake()
    {
        itemSpawner = GetComponent<ItemSpawner>();
        charController = plr.GetComponent<CharacterController2D>();
        mobSpawner = GetComponent<MobSpawner>();
        health = plr.GetComponent<Health>();
        menu = GetComponent<Image>();
        listPanel.SetActive(false);
        listPanel.transform.GetChild(0).gameObject.SetActive(false);
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

    public void SpawnItem(string name)
    {
        foreach (ItemData item in items)
        {
            if (item.name.Equals(name)) itemSpawner.Spawn(item, null, plr.transform.position);
        }
    }

    public void SpawnMob(string name)
    {
        foreach (MobData mob in mobs)
        {
            if (mob.name.Equals(name)) mobSpawner.Spawn(mob, null, plr.transform.position + Vector3.up * 5);
        }
    }

    public void ToggleMob()
    {
        if (openedList == "Mob")
        {
            listPanel.SetActive(false);
            openedList = "";
            return;
        }
        openedList = "Mob";
        SetList();
        foreach (MobData mob in mobs)
        {
            GameObject button = CreateButton();
            button.GetComponentInChildren<TextMeshProUGUI>().text = mob.name;
            button.name = mob.name;
        }
    }

    public void ToggleItem()
    {
        if (openedList == "Item")
        {
            listPanel.SetActive(false);
            openedList = "";
            return;
        }
        openedList = "Item";
        SetList();
        foreach (ItemData item in items)
        {
            GameObject button = CreateButton();
            button.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            button.name = item.name;
        }
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

    private void ButtonClicked(string name)
    {
        if (openedList.Equals("Mob"))
        {
            SpawnMob(name);
        } else if (openedList.Equals("Item"))
        {
            SpawnItem(name);
        }
    }

    private void ClearList()
    {
        for (int i = listPanel.transform.childCount - 1; i > 0; i--)
        {
            Destroy(listPanel.transform.GetChild(i).gameObject);
        }
    }

    private GameObject CreateButton()
    {
        GameObject template = listPanel.transform.GetChild(0).gameObject;
        GameObject obj = Instantiate(template, listPanel.transform);
        obj.SetActive(true);
        obj.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(obj.name));
        return obj;
    }

    private void SetList()
    {
        ClearList();
        listPanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Toggle();
        }
    }
}
