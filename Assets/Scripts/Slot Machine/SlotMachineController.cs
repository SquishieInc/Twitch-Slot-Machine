using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotMachineController : MonoBehaviour
{
    public ReelSpin[] reelScripts;  // Reference to each ReelSpin script for the three reels
    public TMP_Text resultText;     // Display result (win/lose)
    public TMP_Text payoutText;     // Display payout amount

    // Dictionary to store payout values for each symbol
    public Dictionary<Sprite, int> symbolPayouts = new Dictionary<Sprite, int>();

    public Sprite redBar;  // Example of symbols (set these in the Inspector)
    public Sprite blueBar;
    public Sprite greenBar;
    public Sprite yellowStar;
    public Sprite copperCoin;
    public Sprite silverCoin;
    public Sprite goldCoin;

    private bool isSpinning = false;

    void Start()
    {
        // Set up payout values for each symbol
        // Example symbols: You can add more symbols and customize payouts here
        symbolPayouts.Add(redBar, 100);  // Payout for Cherry symbol
        symbolPayouts.Add(blueBar, 200);    // Payout for Bell symbol
        symbolPayouts.Add(greenBar, 500);   // Payout for Seven symbol
        symbolPayouts.Add(yellowStar, 100);  // Payout for Cherry symbol
        symbolPayouts.Add(copperCoin, 200);    // Payout for Bell symbol
        symbolPayouts.Add(silverCoin, 500);   // Payout for Seven symbol
        symbolPayouts.Add(goldCoin, 500);   // Payout for Seven symbol
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinReels());
        }
    }

    IEnumerator SpinReels()
    {
        isSpinning = true;

        // Start all reels spinning
        foreach (ReelSpin reel in reelScripts)
        {
            reel.StartSpin();
        }

        // Wait for the specified spin duration
        yield return new WaitForSeconds(reelScripts[0].spinDuration);

        // Stop all reels
        foreach (ReelSpin reel in reelScripts)
        {
            reel.StopSpin();
        }

        // Check the results after stopping
        CheckResults();
        isSpinning = false;
    }

    void CheckResults()
    {
        // For simplicity, let's assume we are checking the first visible child icon of each reel
        Sprite reel1Icon = reelScripts[0].reelContainer.GetChild(0).GetComponent<Image>().sprite;
        Sprite reel2Icon = reelScripts[1].reelContainer.GetChild(0).GetComponent<Image>().sprite;
        Sprite reel3Icon = reelScripts[2].reelContainer.GetChild(0).GetComponent<Image>().sprite;

        // Check for "three of a kind" winning condition
        if (reel1Icon == reel2Icon && reel2Icon == reel3Icon)
        {
            // Three of a kind: Check the symbol's specific payout
            if (symbolPayouts.ContainsKey(reel1Icon))
            {
                int payout = symbolPayouts[reel1Icon];
                resultText.text = "Jackpot! Three of a kind!";
                Payout(payout);
            }
        }
        else
        {
            // No match or two of a kind
            resultText.text = "No win, try again!";
            Payout(0);  // No payout for no match
        }
    }

    void Payout(int amount)
    {
        // Display the payout amount if any
        if (amount > 0)
        {
            payoutText.text = "Payout: $" + amount.ToString();
        }
        else
        {
            payoutText.text = "Payout: $0";
        }
        // Optionally, handle player's balance here (e.g., playerBalance += amount)
    }
}
