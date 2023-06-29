using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DanceClipSpawner : MonoBehaviour
{
    [SerializeField] private UIDocument ParentContainer; // The UIDocument that holds our whole UI
    [SerializeField] private VisualTreeAsset DanceClip; // The UXML asset for a single DanceClip
    [SerializeField] private int numberOfDanceClips = 3;

    [SerializeField] private string contentContainerName = "ContentContainer";
    
    void Start()
    {
        // Get the root VisualElement of the ParentContainer (our UIDocument)
        VisualElement root = ParentContainer.rootVisualElement;

        // Find the ScrollView where we want to spawn the dance clips. This is our ContentContainer.
        ScrollView contentContainer = root.Q<ScrollView>(contentContainerName);

        // Instantiate the dance clips
        for (int i = 0; i < numberOfDanceClips; i++)
        {
            // Clone the DanceClip template
            VisualElement newDanceClip = DanceClip.CloneTree();

            // Add it to the ContentContainer
            contentContainer.Add(newDanceClip);
        }
    }
}
