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
