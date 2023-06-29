using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DanceClipSpawner : MonoBehaviour
{
    [SerializeField] private UIDocument parentContainer;
    [SerializeField] private int numberOfDanceClips = 3;

    [SerializeField] private string contentContainerName = "ContentContainer";
    [SerializeField] private string characterRenderTextureName = "CharacterRenderTexture";

    [SerializeField] private RenderTexture[] characterRenderTextures;

    [SerializeField] private Texture2D[] backgroundImages;
    
    void Start()
    {
        SpawnDanceClips();
    }

    private void SpawnDanceClips()
    {
        ScrollView contentContainer = parentContainer.rootVisualElement.Q<ScrollView>(contentContainerName);

        for (int i = 0; i < numberOfDanceClips; i++)
        {
            VisualElement danceClip = new VisualElement();
            danceClip.name = $"DanceClip{i}";
            danceClip.style.flexGrow = 1;
            danceClip.style.backgroundImage = new StyleBackground(backgroundImages[i]);
            
            VisualElement characterContainer = new VisualElement();
            characterContainer.name = characterRenderTextureName;
            characterContainer.style.flexGrow = 1;
            characterContainer.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(characterRenderTextures[i]));

            danceClip.Add(characterContainer);

            contentContainer.Add(danceClip);
        }
    }
}
