using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachineController : MonoBehaviour
{
    public Image[] reels;  // Reels to spin
    public Sprite[] reelIcons;  // Available icons for reels
    public float spinDuration = 2f;  // Duration of the spin
    public TMP_Text resultText;  // Text to show results
    public Button spinButton;  // Button to start spin

    private bool isSpinning = false;

    private void Start()
    {
        spinButton.onClick.AddListener(StartSpin);
    }

    void StartSpin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine(SpinReels());
        }
    }

    IEnumerator SpinReels()
    {
        float elapsedTime = 0;
        float interval = 0.1f;  // Interval between each icon change

        while (elapsedTime < spinDuration)
        {
            elapsedTime += interval;
            for (int i = 0; i < reels.Length; i++)
            {
                reels[i].sprite = reelIcons[Random.Range(0, reelIcons.Length)];
            }
            yield return new WaitForSeconds(interval);
        }

        // Spin finished, check results
        CheckResults();
        isSpinning = false;
    }

    void CheckResults()
    {
        // Example logic: if all reels show the same icon, player wins
        if (reels[0].sprite == reels[1].sprite && reels[1].sprite == reels[2].sprite)
        {
            resultText.text = "You Win!";
        }
        else
        {
            resultText.text = "Try Again!";
        }
    }
}
