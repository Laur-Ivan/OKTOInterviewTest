using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DanceClipMovement : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private float swipeThreshold = 100f;
    [SerializeField] private float swipeSmoothTime = 0.2f;

    private ScrollView scrollView;
    private VisualElement root;
    private float targetScroll;
    private float currentScrollVelocity;
    private Vector2 lastMousePosition;

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
        lastMousePosition = evt.mousePosition;
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (evt.pressedButtons == 1) // Only consider mouse left button drag
        {
            Vector2 mouseDelta = evt.mousePosition - lastMousePosition;
            lastMousePosition = evt.mousePosition;

            // Update the target scroll position based on the mouse drag
            targetScroll -= mouseDelta.y;

            // Limit the target scroll position within the content range
            targetScroll = Mathf.Clamp(targetScroll, 0f, scrollView.contentContainer.layout.height - scrollView.layout.height);

            // Set the scroll position immediately to give immediate feedback during the drag
            scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, targetScroll);
        }
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        float currentScrollPosition = scrollView.scrollOffset.y;

        // Determine the nearest content item based on the current scroll position
        int nearestContentItem = Mathf.RoundToInt(currentScrollPosition / scrollView.layout.height);

        // Calculate the target scroll position based on the nearest content item
        targetScroll = nearestContentItem * scrollView.layout.height;

        // Check if the drag distance meets the swipe threshold
        float dragDistance = Mathf.Abs(lastMousePosition.y - evt.mousePosition.y);
        if (dragDistance >= swipeThreshold)
        {
            // Start the smooth scrolling coroutine
            StartCoroutine(SmoothScroll());
        }
        else
        {
            // Snap to the nearest content item without smooth scrolling
            scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, targetScroll);
        }
    }

    IEnumerator SmoothScroll()
    {
        while (Mathf.Abs(scrollView.scrollOffset.y - targetScroll) > 0.01f)
        {
            // Smoothly interpolate the scroll position towards the target
            scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, Mathf.SmoothDamp(scrollView.scrollOffset.y, targetScroll, ref currentScrollVelocity, swipeSmoothTime));
            yield return null;
        }
    }
}
