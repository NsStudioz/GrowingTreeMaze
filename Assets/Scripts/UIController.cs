using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Main Elements")]
    [SerializeField] private GameObject PlayPanel;
    [SerializeField] private GameObject MenuPanel;

    [Header("Maze Value Texts")]
    [SerializeField] private TMP_Text widthValueText;
    [SerializeField] private TMP_Text heightValueText;

    [Header("Sliders")]
    [SerializeField] private Slider widthSilder;
    [SerializeField] private Slider heightSilder;
    private int initialWidth = 50;
    private int initialHeight = 50;

    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button generateMazeBtn;

    [Header("Toggle")]
    [SerializeField] private Toggle generationToggle;

    // Button Events:
    public static event Action OnClickGenerateTheMazeButton;
    public static event Action OnClickPlayButton;
    public static event Action OnClickBackButton;
    // Silders Events:
    public static event Action<int> OnWidthValueChange;
    public static event Action<int> OnHeightValueChange;
    // Toggle Events:
    public static event Action<bool> OnGenerationToggleChange;

    void Awake()
    {
        // Buttons:
        playBtn.onClick.AddListener(ShowPlayMenu);
        backBtn.onClick.AddListener(ShowMainMenu);
        generateMazeBtn.onClick.AddListener(GenerateTheMaze);
        // Sliders:
        widthSilder.onValueChanged.AddListener(ChangeMazeWidth);
        heightSilder.onValueChanged.AddListener(ChangeMazeHeight);
        // Toggle:
        generationToggle.onValueChanged.AddListener(SetGenerationToggle);
    }

    private void InitializeWidthAndHeightValues(int width, int height)
    {
        OnWidthValueChange?.Invoke(width);
        OnHeightValueChange?.Invoke(height);
    }

    #region Buttons:
    private void ShowPlayMenu()
    {
        MenuPanel.SetActive(false);
        PlayPanel.SetActive(true);
        InitializeWidthAndHeightValues(initialWidth, initialHeight);
    }

    private void ShowMainMenu()
    {
        PlayPanel.SetActive(false);
        MenuPanel.SetActive(true);
        OnClickBackButton?.Invoke();
    }

    private void GenerateTheMaze()
    {
        OnClickGenerateTheMazeButton?.Invoke();
    }

    #endregion

    #region Sliders:

    private void ChangeMazeWidth(float value) => SetWidth(Mathf.RoundToInt(value));

    private void ChangeMazeHeight(float value) => SetHeight(Mathf.RoundToInt(value));

    private void SetWidth(int width)
    {
        widthSilder.value = width;
        SyncWidthValueText(width);
        OnWidthValueChange?.Invoke(width);
    }

    private void SetHeight(int height)
    {
        heightSilder.value = height;
        SyncHeightValueText(height);
        OnHeightValueChange?.Invoke(height);
    }

    private void SyncWidthValueText(int width)
    {
        widthValueText.text = width.ToString();
    }
    private void SyncHeightValueText(int height)
    {
        heightValueText.text = height.ToString();
    }

    #endregion

    #region Toggle:

    private void SetGenerationToggle(bool state)
    {

    }

    #endregion


    // Removing all UI listeners.
    private void OnDestroy()
    {
        // Buttons:
        playBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
        generateMazeBtn.onClick.RemoveAllListeners();
        // Sliders:
        widthSilder.onValueChanged.RemoveAllListeners();
        heightSilder.onValueChanged.RemoveAllListeners();
        // Toggle:
        generationToggle.onValueChanged.RemoveAllListeners();
    }

}
