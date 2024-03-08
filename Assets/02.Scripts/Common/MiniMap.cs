using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private GameObject miniMap;
    [SerializeField] private Image _player;
    private bool miniMapOnOff;
    

    void Start()
    {
        miniMap = GameObject.Find("Panel_Minimap").GetComponent<GameObject>();
        miniMapOnOff = true;
    }

    void Update()
    {
        if(miniMapOnOff && Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(false);
            miniMapOnOff = false;
        }
        if (!miniMapOnOff && Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(true);
            miniMapOnOff = true;
        }

    }
}
