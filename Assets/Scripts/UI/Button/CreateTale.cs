using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTale : MonoBehaviour
{
    public GameObject PanelTale;
    public GameObject InputTaleName;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateTaleHandler);
    }

    private void CreateTaleHandler()
    {
        string taleName = InputTaleName.GetComponent<InputField>().text;
        if (taleName.Length == 0)
        {
            return;
        }

        GameObject camera = GameObject.Find("ARCamera");

        MenuManager mm = camera.GetComponent<MenuManager>();

        mm.CreateTale(taleName);

        mm.LoadModels();
        mm.OnClickSaveTale();

        Utils.HideOtherPanels(PanelTale);
    }
}