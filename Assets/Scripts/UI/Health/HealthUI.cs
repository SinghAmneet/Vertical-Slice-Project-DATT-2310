using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject heartPrefab;

    private int heartCount;
    private List<HeartUI> hearts = new();

    public void Setup(int health)
    {
        this.heartCount = health;

        for (int i = 0; i < health; i ++)
        {
            // create new heart and parent to health object
            GameObject heartObj = Instantiate(heartPrefab);
            heartObj.transform.SetParent(gameObject.transform);

            HeartUI heart = heartObj.GetComponent<HeartUI>();
            hearts.Add(heart);
        }
    }

    public void UpdateHearts(float health)
    {
        // get the current heart to update
        int heartIndex = Mathf.Clamp(Mathf.FloorToInt(health), 0, heartCount - 1);
        
        // get the alpha for current heart
        float heartProgress = health - heartIndex;

        hearts[heartIndex].UpdateProgress(heartProgress); // update current heart

        // update other hearts
        for (int i = 0; i < heartCount; i++)
        {
            if (i > heartIndex)
            {
                hearts[i].FullDeplete();
            }
            else if (i < heartIndex)
            {
                hearts[i].FullHeal();
            }
        }
    }
}
