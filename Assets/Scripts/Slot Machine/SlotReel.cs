using UnityEngine;
using UnityEngine.UI;

public class SlotReel : MonoBehaviour
{
    public Image[] reelSymbols;  // Array of images for reel symbols
    public Sprite[] symbols;     // All available symbols (fruit, numbers, icons)

    public float spinSpeed = 1000f;  // Speed of reel spinning
    private bool isSpinning = false; // Flag to check if reel is spinning

    private int currentSymbolIndex;  // Current index of the visible symbol
    private float stopTime;          // Time at which to stop spinning

    void Update()
    {
        if (isSpinning)
        {
            SpinReel();
        }
    }

    // Method to start the spinning process
    public void StartSpinning(float duration)
    {
        isSpinning = true;
        stopTime = Time.time + duration;  // Set stop time for the spin
    }

    // Spins the reel
    private void SpinReel()
    {
        for (int i = 0; i < reelSymbols.Length; i++)
        {
            // Move each symbol image down by speed * deltaTime
            reelSymbols[i].transform.Translate(Vector3.down * spinSpeed * Time.deltaTime);

            // Reset symbol to top if it moves past a certain point
            if (reelSymbols[i].transform.localPosition.y < -150f)
            {
                reelSymbols[i].transform.localPosition = new Vector3(0f, 150f, 0f);
                currentSymbolIndex = (currentSymbolIndex + 1) % symbols.Length;
                reelSymbols[i].sprite = symbols[currentSymbolIndex];  // Assign the new symbol
            }
        }

        // Stop spinning when time is up
        if (Time.time >= stopTime)
        {
            isSpinning = false;
        }
    }
}
