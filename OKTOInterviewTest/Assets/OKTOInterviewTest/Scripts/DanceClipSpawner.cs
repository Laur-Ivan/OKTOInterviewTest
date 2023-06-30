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
    
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Camera[] cameraPrefabs;
    [SerializeField] private string[] layerNames;
    
    [SerializeField] private Vector3 characterBasePosition = new Vector3(5, 0, -6);
    [SerializeField] private Vector3 cameraBasePosition = new Vector3(5, 0, -10);
    [SerializeField] private float positionOffset = 5f;

    
    void Start()
    {
        if (numberOfDanceClips > characterPrefabs.Length)
        {
            Debug.Log($"Trying to spawn more dance clips than possible, spawning only as many as we have");
            numberOfDanceClips = characterPrefabs.Length;
        }
    
        SpawnCharactersAndCameras();
        SpawnDanceClips();
    }

    private void SpawnDanceClips()
    {
        ScrollView contentContainer = parentContainer.rootVisualElement.Q<ScrollView>(contentContainerName);

        if (contentContainer == null)
        {
            Debug.LogError("ScrollView 'ContentContainer' not found");
        }
        
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

    private void SpawnCharactersAndCameras()
    {
        for (int i = 0; i < numberOfDanceClips; i++)
        {
            GameObject character = Instantiate(characterPrefabs[i]);
            Vector3 characterOffset = new Vector3(i * positionOffset, 0, 0);
            character.transform.position = characterBasePosition + characterOffset;
            
            int layer = LayerMask.NameToLayer(layerNames[i]);
            SetLayerRecursively(character, layer);

            Camera camera = Instantiate(cameraPrefabs[i]);
            Vector3 cameraOffset = new Vector3(i * positionOffset, 0, 0);
            camera.transform.position = cameraBasePosition + cameraOffset;
            camera.cullingMask = 1 << layer;
            camera.targetTexture = characterRenderTextures[i];
        }
    }
    
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;
   
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
