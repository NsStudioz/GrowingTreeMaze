using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Main Elements")]
    [SerializeField] private GameObject GamePanel;

    [Header("Maze Value Texts")]
    [SerializeField] private TMP_Text widthValueText;
    [SerializeField] private TMP_Text heightValueText;

    [Header("Sliders")]
    [SerializeField] private Slider widthSilder;
    [SerializeField] private Slider heightSilder;

    [Header("Buttons")]
    [SerializeField] private Button generateMazeBtn;
    [SerializeField] private Button backBtn;

    [Header("Toggle")]
    [SerializeField] private Toggle generationToggle;



    void Start()
    {
        
    }



}
