using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReelSpin : MonoBehaviour
{
    public float spinSpeed = 1000f;  // Speed at which the reel spins
    public float spinDuration = 2f;  // Total duration for the reel to spin
    public RectTransform reelContainer;  // The RectTransform that holds the icons (Reel1Container, etc.)
    public int numberOfIcons = 5;  // Number of icons in this reel

    private float containerHeight;  // The height of the container (used to loop the icons)
    private bool isSpinning = false;
    private Coroutine spinCoroutine;

    void Start()
    {
        // Calculate the height of the container based on the number of icons
        containerHeight = reelContainer.rect.height;
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            spinCoroutine = StartCoroutine(SpinReel());
        }
    }

    IEnumerator SpinReel()
    {
        isSpinning = true;

        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;

            // Move the container upwards to simulate spinning
            reelContainer.anchoredPosition += Vector2.up * spinSpeed * Time.deltaTime;

            // Loop the icons by resetting the position if it goes beyond the container's height
            if (reelContainer.anchoredPosition.y >= containerHeight)
            {
                reelContainer.anchoredPosition = new Vector2(reelContainer.anchoredPosition.x, 0);
            }

            yield return null;
        }

        // After spinning duration ends, snap to a specific final position
        SnapToFinalPosition();
        isSpinning = false;
    }

    private void SnapToFinalPosition()
    {
        // Find the closest icon to snap to (final result)
        float nearestIconPosition = Mathf.Round(reelContainer.anchoredPosition.y / (containerHeight / numberOfIcons)) * (containerHeight / numberOfIcons);
        reelContainer.anchoredPosition = new Vector2(reelContainer.anchoredPosition.x, nearestIconPosition);
    }

    public void StopSpin()
    {
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
        }
        SnapToFinalPosition();
        isSpinning = false;
    }
}
