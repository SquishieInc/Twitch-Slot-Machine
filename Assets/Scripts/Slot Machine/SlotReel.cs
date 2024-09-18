using UnityEngine;
using UnityEngine.UI;

public class SlotReel : MonoBehaviour
{
    public Image[] reelSymbols;  // Array of images for reel symbols
    public Sprite[] symbols;     // All available symbols (fruit, numbers, icons)

    public float spinSpeed = 5f;  // Base speed of reel spinning
    private bool isSpinning = false; // Flag to check if reel is spinning

    public int currentSymbolIndex;  // Current index of the visible symbol
    private float stopTime;          // Time at which to stop spinning
    public RectTransform[] symbolTransforms;  // RectTransforms of each symbol
    private float symbolHeight = 150f;        // Height of each symbol

    private float easingFactor = 2f;  // Factor that controls the easing (higher = more ease)
    private bool isBouncing = false;  // Flag to check if bounce effect is active
    private Vector2 bounceTarget;     // Target position for bounce animation
    private float bounceAmount = 15f; // How much bounce (adjust for effect strength)
    private float bounceSpeed = 8f;   // Speed of bounce back

    void Start()
    {
        // Cache RectTransforms of the symbols
        symbolTransforms = new RectTransform[reelSymbols.Length];
        for (int i = 0; i < reelSymbols.Length; i++)
        {
            symbolTransforms[i] = reelSymbols[i].GetComponent<RectTransform>();
        }
    }

    void Update()
    {
        if (isSpinning)
        {
            SpinReel();
        }

        if (isBouncing)
        {
            BounceEffect();
        }
    }

    // Method to start the spinning process
    public void StartSpinning(float duration)
    {
        isSpinning = true;
        stopTime = Time.time + duration;  // Set stop time for the spin
    }

    // Spins the reel with easing animation
    private void SpinReel()
    {
        float currentSpinSpeed;

        // Slow down the spin as we approach stop time
        if (Time.time >= stopTime - 1f) // Start easing 1 second before stop
        {
            // SmoothStep to reduce spin speed smoothly over time
            float t = (stopTime - Time.time);
            currentSpinSpeed = Mathf.SmoothStep(spinSpeed, 0f, 1f - t / 1f);  // Easing
        }
        else
        {
            currentSpinSpeed = spinSpeed;
        }

        // Spin the symbols
        for (int i = 0; i < symbolTransforms.Length; i++)
        {
            // Move the symbol down
            symbolTransforms[i].anchoredPosition = new Vector2(
                0, symbolTransforms[i].anchoredPosition.y - (currentSpinSpeed * Time.deltaTime * 200f)
            );

            // Reset symbol to the top if it moves past a certain point
            if (symbolTransforms[i].anchoredPosition.y <= -symbolHeight)
            {
                // Reposition symbol to the top
                symbolTransforms[i].anchoredPosition = new Vector2(0f, symbolHeight * (reelSymbols.Length - 1));

                // Assign the new symbol
                currentSymbolIndex = (currentSymbolIndex + 1) % symbols.Length;
                reelSymbols[i].sprite = symbols[currentSymbolIndex];
            }
        }

        // Stop spinning and start the bounce effect when time is up
        if (Time.time >= stopTime)
        {
            isSpinning = false;
            AlignSymbols();
            StartBounce();
        }
    }

    // Align symbols at the end of the spin to ensure they are perfectly centered
    private void AlignSymbols()
    {
        for (int i = 0; i < symbolTransforms.Length; i++)
        {
            // Snap the symbols to the nearest position (based on symbol height)
            float roundedPosition = Mathf.Round(symbolTransforms[i].anchoredPosition.y / symbolHeight) * symbolHeight;
            symbolTransforms[i].anchoredPosition = new Vector2(0, roundedPosition);
        }
    }

    // Starts the bounce effect when the reels stop spinning
    private void StartBounce()
    {
        isBouncing = true;

        // Choose one symbol (typically the center symbol) for the bounce effect
        int bounceSymbolIndex = 1; // Middle symbol (assuming 3 symbols vertically)
        RectTransform bouncingSymbol = symbolTransforms[bounceSymbolIndex];

        // Set target for bounce (slightly lower than final position, to overshoot)
        bounceTarget = new Vector2(
            bouncingSymbol.anchoredPosition.x,
            bouncingSymbol.anchoredPosition.y - bounceAmount
        );
    }

    // Bounce effect applied after spinning stops
    private void BounceEffect()
    {
        // Bounce towards the target
        for (int i = 0; i < symbolTransforms.Length; i++)
        {
            RectTransform symbol = symbolTransforms[i];
            symbol.anchoredPosition = Vector2.Lerp(
                symbol.anchoredPosition,
                bounceTarget,
                Time.deltaTime * bounceSpeed
            );
        }

        // Check if the bounce is close enough to stop
        if (Vector2.Distance(symbolTransforms[1].anchoredPosition, bounceTarget) < 0.1f)
        {
            // Snap to final position
            AlignSymbols();
            isBouncing = false;  // End bounce animation
        }
    }
}
