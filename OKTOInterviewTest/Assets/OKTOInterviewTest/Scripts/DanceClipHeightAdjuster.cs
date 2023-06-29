using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DanceClipHeightAdjuster : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    [Range(0.3f, 1)] // Using a minimum of 0.3f just because that makes more sense than using 0 - we will always have a DanceClip to show
    // This is the % of height (relative to the screen size) allocated for the DanceClip. Our banner (the bottom part with the buttons) takes 10% of the screen size on height, so we have 90% remaining
    [SerializeField] private float heightAllocatedForDanceClip = 0.9f;

    void Start()
    {
        // I am using a Coroutine just to make sure that the UI sizes are setup and everything is rendered properly. Some research should be done to make sure this is needed or not
        StartCoroutine(AdjustHeight());
    }

    IEnumerator AdjustHeight()
    {
        yield return new WaitForEndOfFrame();

        ScrollView scrollView = uiDocument.rootVisualElement.Q<ScrollView>("ContentContainer");

        if (scrollView == null)
        {
            Debug.LogError("ScrollView 'ContentContainer' not found");
            yield break;
        }

        foreach (VisualElement danceClip in scrollView.contentContainer.Children())
        {
            if (danceClip == null)
            {
                Debug.LogError("DanceClip not found");
                continue;
            }

            float screenSpaceHeight = Screen.height * heightAllocatedForDanceClip; 
            danceClip.style.height = screenSpaceHeight;
            Debug.Log("DanceClip height adjusted to " + screenSpaceHeight);
        }
    }
}
