using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DanceClipMovement : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    // How long the animation should take, in seconds
    [SerializeField] private float swipeSmoothTime = 0.2f;

    private ScrollView scrollView;
    private VisualElement root;
    private Vector2 lastMousePosition;
    
    private float targetScroll;
    private float currentScrollVelocity;
    private float totalDrag;

    private bool isAnimating;

    [SerializeField] private string contentContainerName = "ContentContainer";

    void Awake()
    {
        root = uiDocument.rootVisualElement;
        scrollView = root.Q<ScrollView>(contentContainerName);
    }

    void OnEnable()
    {
        scrollView.RegisterCallback<MouseDownEvent>(OnMouseDown);
        scrollView.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        scrollView.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    private void OnDisable()
    {
        scrollView.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        scrollView.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        scrollView.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (!isAnimating)
        {
            lastMousePosition = evt.mousePosition;
        }
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (isAnimating) 
        {
            // I ignore mouse move events during animation
            return; 
        }

        // So we only consider mouse left button drag
        if (evt.pressedButtons == 1) 
        {
            Vector2 mouseDelta = evt.mousePosition - lastMousePosition;
            lastMousePosition = evt.mousePosition;

            // Update the target scroll position based on the mouse drag
            float updatedScroll = scrollView.scrollOffset.y - mouseDelta.y;

            // Limit the target scroll position within the content range
            updatedScroll = Mathf.Clamp(updatedScroll, 0f, scrollView.contentContainer.layout.height - scrollView.layout.height);

            // Set the scroll position immediately to give immediate feedback during the drag
            scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, updatedScroll);
        }
    }
    
    private void OnMouseUp(MouseUpEvent evt)
    {
        if (!isAnimating)
        {
            // Determine the nearest content item based on the current scroll position
            int nearestContentItem = Mathf.RoundToInt(scrollView.scrollOffset.y / scrollView.layout.height);

            // If the total drag amount is non-zero, move to the next/previous item
            if (totalDrag != 0)
            {
                nearestContentItem += Math.Sign(totalDrag);
            }

            // Reset the total drag amount for the next swipe
            totalDrag = 0f;

            // Calculate the target scroll position based on the nearest content item
            targetScroll = nearestContentItem * scrollView.layout.height;

            isAnimating = true;
            
            StartCoroutine(SmoothScroll());
        }
    }

    IEnumerator SmoothScroll()
    {
        float elapsedTime = 0f;
        float duration = swipeSmoothTime;
        float startScroll = scrollView.scrollOffset.y;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration; 
            t = --t * t * t + 1; // Cubic ease-out, math is cool!

            float currentScroll = Mathf.Lerp(startScroll, targetScroll, t);

            scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, currentScroll);

            yield return null;
        }

        // Just so I make sure we reach the target at the end
        scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, targetScroll);

        // Reset targetScroll
        targetScroll = scrollView.scrollOffset.y;

        //I reset the animation flag, making sure we can swipe again
        isAnimating = false;
    }
}
