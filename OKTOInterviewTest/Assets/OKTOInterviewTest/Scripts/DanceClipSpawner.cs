using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DanceClipSpawner : MonoBehaviour
{
    [SerializeField] private UIDocument parentContainer; // The UIDocument that holds our whole UI
    [SerializeField] private VisualTreeAsset danceClipTemplate; // The UXML asset for a single DanceClip
    [SerializeField] private int numberOfDanceClips = 3;

    [SerializeField] private string contentContainerName = "ContentContainer";
    [SerializeField] private string characterRenderTextureName = "CharacterRenderTexture";
    
    [SerializeField] private RenderTexture[] characterRenderTextures;

    // Reference to your background images for the DanceClips
    [SerializeField] private Texture2D[] backgroundImages;

    void Start()
    {
        // Get the ContentContainer
        ScrollView contentContainer = parentContainer.rootVisualElement.Q<ScrollView>(contentContainerName);

        // Loop for the number of DanceClips you want to create
        for (int i = 0; i < 3; i++)
        {
            // Create a new DanceClip from the template
            VisualElement danceClip = danceClipTemplate.CloneTree().contentContainer;

            // Get the CharacterRenderTexture VisualElement in the DanceClip
            VisualElement characterContainer = danceClip.Q<VisualElement>(characterRenderTextureName);
            Debug.Log($"Character container {characterContainer}");
            
            // Set the background image of the DanceClip to one of your textures
            danceClip.style.backgroundImage = new StyleBackground(backgroundImages[i]);

            // Set the background image of the CharacterRenderTexture to one of your RenderTextures
            Debug.Log($"Character render texture {characterRenderTextures[i]}");
            characterContainer.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(characterRenderTextures[i]));

            // Add the DanceClip to the ContentContainer
            contentContainer.Add(danceClip);
        }
    }
}
