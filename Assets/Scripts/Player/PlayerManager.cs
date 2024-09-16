using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;
    public static bool isGameStarted;
    public GameObject startingText;
    public TextMeshProUGUI coinsText;

    public static int numberOfCoins;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        gameOverPanel.SetActive(false);
        isGameStarted = false;
        startingText.SetActive(true);

        numberOfCoins = 0;
        
        Time.timeScale = 1;  // Used in PlayerController.cs
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        coinsText.text = "Coins: " + numberOfCoins;

        if (SwipeManager.tap)// && !isGameStarted)
        {
            isGameStarted = true;
            Destroy(startingText);
        }

    }
}
