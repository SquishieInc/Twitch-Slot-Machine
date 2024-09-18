using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachineManager : MonoBehaviour
{
    public SlotReel[] reels;  // Reference to all slot reels
    public Button spinButton; // Spin button
    public TMP_Text resultText;   // Text to display result

    private int[] reelResults; // Array to store the symbol index results for each reel

    void Start()
    {
        spinButton.onClick.AddListener(SpinReels);
        reelResults = new int[reels.Length];
    }

    public void SpinReels()
    {
        resultText.text = "";  // Clear result text
        spinButton.interactable = false;  // Disable spin button during the spin

        // Spin each reel for a random duration
        for (int i = 0; i < reels.Length; i++)
        {
            float randomSpinDuration = Random.Range(1.5f, 3.0f);
            reels[i].StartSpinning(randomSpinDuration);
        }

        // Invoke the method to calculate result after all reels stop
        Invoke("CalculateResult", 3.5f);  // Slight delay for animation to finish
    }

    private void CalculateResult()
    {
        // Collect the current symbol index of each reel
        for (int i = 0; i < reels.Length; i++)
        {
            reelResults[i] = reels[i].currentSymbolIndex;
        }

        // Check if we have a winning combination
        if (IsWinningCombination())
        {
            resultText.text = "You Win!";
            CalculatePayout();
        }
        else
        {
            resultText.text = "You Lose!";
        }

        spinButton.interactable = true;  // Re-enable spin button
    }

    private bool IsWinningCombination()
    {
        // Example win condition: All reels must show the same symbol
        for (int i = 1; i < reelResults.Length; i++)
        {
            if (reelResults[i] != reelResults[0])
                return false;
        }
        return true;
    }

    private void CalculatePayout()
    {
        // Example payout: Payout depends on the symbol
        int payout = 0;

        switch (reelResults[0])
        {
            case 0: payout = 100; break;  // First symbol (e.g., cherry)
            case 1: payout = 200; break;  // Second symbol (e.g., lemon)
            case 2: payout = 500; break;  // Third symbol (e.g., 7)
                                          // Add more symbols and payouts as needed
        }

        resultText.text += "\nPayout: $" + payout.ToString();
    }
}
