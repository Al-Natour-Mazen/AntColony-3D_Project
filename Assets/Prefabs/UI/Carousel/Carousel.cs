using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO.Pipes;
using System;


/// <summary>
/// Controls a carousel UI element that can display content horizontally or vertically.
/// </summary>
public class Carousel : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    /// <summary>
    /// Enum defining the layout type of the carousel.
    /// </summary>
    public enum LayoutType
    {
        Vertical,
        Horizontal
    }
    [Header("Layout Settings")]
    [Tooltip("Select the layout type for the content carousel.")]
    public LayoutType layoutType = LayoutType.Vertical;

    [Header("Content Viewport")]
    [Tooltip("Specify the size of each page in the carousel.")]
    public float pageSize = 60f;

    [Tooltip("The threshold for swipe detection. Adjust for sensitivity.")]
    public float swipeThreshold = 0.2f;

    [Tooltip("The speed at which the content snaps to the target position.")]
    public float snapSpeed = 8f;

    [Header("Navigation Buttons")]
    [Tooltip("Click to move to the next page.")]
    public Button nextButton;

    [Tooltip("Click to move to the previous page.")]
    public Button prevButton;

    [Header("Carousel Mode Settings")]
    [Tooltip("Enables automatic cycling through pages.")]
    public bool carouselMode = true;

    [Tooltip("Starts automatic page cycling when enabled.")]
    public bool autoMove = false;

    [Tooltip("Adjusts the time interval between automatic page transitions (in seconds).")]
    public float autoMoveTimer = 5f;

    [Header("Navigation Dots Settings")]
    [Tooltip("Prefab for navigation dots.")]
    public GameObject dotPrefab;

    [Tooltip("Container to hold the navigation dots.")]
    public GameObject dotsContainer;

    [Tooltip("Sets the color of the dot representing the current page.")]
    public Color activeDotColor = Color.cyan;

    [Tooltip("Sets the color of the dots representing inactive pages.")]
    public Color inactiveDotColor = Color.grey;

    [Tooltip("Controls the speed of the color transition between dots (in seconds).")]
    public float dotColorTransitionSpeed = 5f;

    [Header("Infinite Looping Settings")]
    [Tooltip("Enable or disable infinite looping for the carousel.")]
    public bool infiniteLooping = true;

    [Header("Checks")]
    [Tooltip("Indicates the total number of pages.")]
    public int totalPages;

    [Tooltip("Specifies the index of the currently displayed page.")]
    public int currentIndex = 0;

    [Tooltip("Manages the layout of page elements.")]
    public GridLayoutGroup gridLayoutGroup;

    //private variables
    private RectTransform contentRectTransform;
    private float targetPosition;
    private bool isDragging = false;
    private Vector2 dragStartPos;
    private float lastDragTime;
    private float autoMoveTimerCountdown;
    private enum PageDirection
    {
        Previous,
        Next
    }

    /// <summary>
    /// Initializes the carousel.
    /// </summary>
    void Start()
    {
        // Find and configure the GridLayoutGroup component.
        FindGridLayoutGroup();
        if (gridLayoutGroup == null)
            return;

        // Set the start axis of the GridLayoutGroup based on the layout type.
        SetGridLayoutAxis();

        // Calculate the total number of pages in the carousel.
        CalculateTotalPages();

        // Set the initial target position for snapping.
        SetSnapTarget(0);

        // Setup navigation buttons.
        SetupNavigationButtons();

        // Initialize navigation dots if carousel mode is enabled.
        if (carouselMode)
            InitializeNavigationDots();
    }

    /// <summary>
    /// Updates the carousel's state.
    /// </summary>
    void Update()
    {
        // Handle automatic page transitions if autoMove is enabled
        if (autoMove)
        {
            // Reduce autoMoveTimerCountdown by deltaTime
            autoMoveTimerCountdown -= Time.deltaTime;

            // If the countdown is finished, move to the next page and reset the timer
            if (autoMoveTimerCountdown <= 0f)
            {
                MoveToPage(PageDirection.Next);
                autoMoveTimerCountdown = autoMoveTimer;
            }
        }

        // Smoothly move content towards the target position when not dragging
        if (!isDragging)
        {
            // Interpolate between the current anchored position and the target position
            contentRectTransform.anchoredPosition = Vector2.Lerp(
                contentRectTransform.anchoredPosition,
                new Vector2(targetPosition, contentRectTransform.anchoredPosition.y),
                Time.deltaTime * snapSpeed
            );

            // Smoothly update dot color based on the current index
            UpdateDotColor();
        }
    }

    // Find the GridLayoutGroup component in children of the carousel.
    private void FindGridLayoutGroup()
    {
        // Attempt to find the GridLayoutGroup component among the children of the current GameObject
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

        // If the GridLayoutGroup component is not found
        if (gridLayoutGroup == null)
        {
            // Log an error message
            Debug.LogError("GridLayoutGroup not found in children. Make sure it is present.");
        }
        else // If the GridLayoutGroup component is found
        {
            // Get the RectTransform component of the gridLayoutGroup's transform
            contentRectTransform = gridLayoutGroup.transform as RectTransform;

            // If the RectTransform component is not found
            if (contentRectTransform == null)
            {
                // Log an error message
                Debug.LogError("RectTransform not found on the GridLayoutGroup. Make sure it is present.");
                // Set gridLayoutGroup to null to indicate failure
                gridLayoutGroup = null;
            }
        }
    }


    // Set the start axis of the GridLayoutGroup based on the layout type.
    private void SetGridLayoutAxis()
    {
        // Check if the gridLayoutGroup is not null
        if (gridLayoutGroup != null)
        {
            // Set the start axis of the grid layout group based on the layout type
            gridLayoutGroup.startAxis = (layoutType == LayoutType.Vertical) ? GridLayoutGroup.Axis.Vertical : GridLayoutGroup.Axis.Horizontal;
        }
    }


    // Setup navigation buttons click listeners.
    private void SetupNavigationButtons()
    {
        // If the nextButton is not null, add a listener to its click event
        if (nextButton != null)
        {
            // Add a listener to the nextButton's click event to move to the next page
            nextButton.onClick.AddListener(() => MoveToPage(PageDirection.Next));
        }

        // If the prevButton is not null, add a listener to its click event
        if (prevButton != null)
        {
            // Add a listener to the prevButton's click event to move to the previous page
            prevButton.onClick.AddListener(() => MoveToPage(PageDirection.Previous));
        }
    }


    // Initialize navigation dots based on the total number of pages.
    private void InitializeNavigationDots()
    {
        // Loop through each page
        for (int i = 0; i < totalPages; i++)
        {
            // Instantiate a dot prefab as a child of the dots container
            GameObject dot = Instantiate(dotPrefab, dotsContainer.transform);

            // Assign the current index to a local variable for the click event
            int dotIndex = i;

            // Set the color of the dot based on its index
            if (dot != null && dot.GetComponent<Image>() != null)
            {
                Image dotImage = dot.GetComponent<Image>();
                dotImage.color = dotIndex == currentIndex ? activeDotColor : inactiveDotColor;
            }

            // Set click event for the dot to move to its corresponding page
            if (dot != null && dot.GetComponent<Button>() != null)
            {
                Button dotButton = dot.GetComponent<Button>();
                // Add a listener to the dot's click event to move to its corresponding page
                dotButton.onClick.AddListener(() => SetSnapTarget(dotIndex));
            }
        }
    }

    // Calculate the total number of pages based on the number of items in the GridLayoutGroup.
    private void CalculateTotalPages()
    {
        // Get the total number of items in the grid layout group
        int itemCount = gridLayoutGroup.transform.childCount;

        // Calculate the total number of pages needed to display all items
        // by dividing the total item count by the number of items per row (constraint count)
        totalPages = Mathf.CeilToInt((float)itemCount / gridLayoutGroup.constraintCount);
    }

    // Set the target position for snapping based on the specified page.
    private void SetSnapTarget(int page)
    {
        // If infinite looping is enabled
        if (infiniteLooping)
        {
            // Calculate total visible pages (doubled for looping effect)
            int totalVisiblePages = totalPages * 2;

            // Calculate the offset page index for looping effect
            int offsetPage = (page + totalVisiblePages) % totalPages;

            // Calculate the target position based on the offset page
            targetPosition = -pageSize * offsetPage;
        }
        else // If infinite looping is disabled
        {
            // Calculate the target position based on the current page
            targetPosition = -pageSize * page;
        }

        // Update the currentIndex to the provided page
        currentIndex = page;
    }

    // Handle the beginning of a drag event.
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Set isDragging to true as the drag operation has begun
        isDragging = true;

        // Record the starting position of the drag
        dragStartPos = eventData.position;

        // Record the time when the drag started
        lastDragTime = Time.unscaledTime;
    }


    // Handle the end of a drag event.
    public void OnEndDrag(PointerEventData eventData)
    {
        // Set isDragging to false as the drag operation has ended
        isDragging = false;

        // Calculate the distance of the drag
        float dragDistance = Mathf.Abs(eventData.position.x - dragStartPos.x);

        // Calculate the speed of the drag
        float dragSpeed = eventData.delta.x / (Time.unscaledTime - lastDragTime);

        // If autoMove is enabled, reset the autoMoveTimerCountdown
        if (autoMove)
        {
            autoMoveTimerCountdown = autoMoveTimer;
        }

        // If carouselMode is enabled, handle the swipe based on drag distance and speed
        if (carouselMode)
        {
            HandleSwipe(dragDistance, dragSpeed);
        }
    }

    // Handle swipe gestures during drag events.
    private void HandleSwipe(float dragDistance, float dragSpeed)
    {
        // Check if the drag distance or drag speed is greater than the swipe threshold
        if (dragDistance > pageSize * swipeThreshold || Mathf.Abs(dragSpeed) > swipeThreshold)
        {
            // If the drag speed is positive, move to the previous page
            if (dragSpeed > 0)
            {
                MoveToPage(PageDirection.Previous);
            }
            else // If the drag speed is negative, move to the next page
            {
                MoveToPage(PageDirection.Next);
            }
        }
        else // If the drag distance or speed is not sufficient for a swipe
        {
            // Snap back to the current page
            SetSnapTarget(currentIndex);
        }
    }


    // Move to the previous or next page based on the direction.
    private void MoveToPage(PageDirection direction)
    {
        // Variable to store the target page index
        int targetPage;

        // If infinite looping is enabled
        if (infiniteLooping)
        {
            // Calculate the target page index based on the direction
            // If moving to the next page, calculate modulo to wrap around
            // If moving to the previous page, add totalPages to ensure positive result
            targetPage = direction == PageDirection.Next ? (currentIndex + 1) % totalPages : (currentIndex - 1 + totalPages) % totalPages;
        }
        else // If infinite looping is disabled
        {
            // Calculate the target page index based on the direction
            // Ensure the target page index stays within the range of 0 to totalPages - 1
            targetPage = direction == PageDirection.Next ? Mathf.Clamp(currentIndex + 1, 0, totalPages - 1) : Mathf.Clamp(currentIndex - 1, 0, totalPages - 1);
        }

        // Call a method to set the target page for snapping
        SetSnapTarget(targetPage);
    }


    // Update the color of navigation dots based on the current page index.
    private void UpdateDotColor()
    {
        // Loop through each child of the dotsContainer
        for (int i = 0; i < dotsContainer.transform.childCount; i++)
        {
            // Get the GameObject of the current dot
            GameObject dot = dotsContainer.transform.GetChild(i).gameObject;

            // Smoothly interpolate between current color and target color for the dot
            Image dotImage = dot.GetComponent<Image>(); // Get the Image component of the dot
            if (dotImage != null) // Check if the dot has an Image component
            {
                // Determine the target color for the dot based on its index
                Color targetColor = i == currentIndex ? activeDotColor : inactiveDotColor;

                // Smoothly transition the dot's color towards the target color over time
                dotImage.color = Color.Lerp(dotImage.color, targetColor, Time.deltaTime * dotColorTransitionSpeed);
            }
        }
    }


}