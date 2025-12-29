using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private GameObject[] heartContainers;
    private Image[] heartFills;

    public Transform heartsParent;
    public GameObject heartContainerPrefab;

    private void Start()
    {
        PlayerHealth player = PlayerHealth.Instance;

        heartContainers = new GameObject[(int)player.maxHealth];
        heartFills = new Image[(int)player.maxHealth];

        player.onHealthChangedCallback += UpdateHeartsHUD;

        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    public void UpdateHeartsHUD()
    {
        PlayerHealth player = PlayerHealth.Instance;
        SetHeartContainers(player.maxHealth);
        SetFilledHearts(player.currentHealth);
    }

    void SetHeartContainers(float maxHealth)
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            heartContainers[i].SetActive(i < maxHealth);
        }
    }

    void SetFilledHearts(float currentHealth)
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < currentHealth)
                heartFills[i].fillAmount = 1;
            else
                heartFills[i].fillAmount = 0;
        }

        // handle partial heart fill (e.g. 7.5)
        if (currentHealth % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(currentHealth);
            heartFills[lastPos].fillAmount = currentHealth % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < PlayerHealth.Instance.maxHealth; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab, heartsParent);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
}
