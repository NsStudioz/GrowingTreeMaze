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

    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button generateMazeBtn;

    [Header("Toggle")]
    [SerializeField] private Toggle generationToggle;
    [SerializeField] private bool isQuickestGenerate; // testing the bool is in sync with toggle.

    // Button Events:
    public static event Action OnClickGenerateTheMazeButton;
    public static event Action OnClickPlayButton;
    public static event Action OnClickBackButton;
    // Silders Events:
    public static event Action<int> OnWidthValueChange;
    public static event Action<int> OnHeightValueChange;
    // Toggle Events:
    public static event Action<bool> OnGenerationToggleChange;

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

    void Start()
    {
        // Buttons:
        playBtn.onClick.AddListener(ShowPlayMenu);
        backBtn.onClick.AddListener(ShowMainMenu);
        generateMazeBtn.onClick.AddListener(GenerateTheMaze);
    }

    #region Buttons:
    private void ShowPlayMenu()
    {
        MenuPanel.SetActive(false);
        PlayPanel.SetActive(true);
        OnClickPlayButton?.Invoke();
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



}
