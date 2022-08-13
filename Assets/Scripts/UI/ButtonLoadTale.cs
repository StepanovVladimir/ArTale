using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadTale : MonoBehaviour
{
    public string TaleName;
    public GameObject PanelEditOrView;
    public GameObject PanelMainMenu;

    public bool IsEdit = true;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnMouseClick);
    }

    void OnMouseClick()
    {
        GameObject camera = GameObject.Find("ARCamera");
        MenuManager mm = camera.GetComponent<MenuManager>();
        if (IsEdit)
        {
            mm.LoadTaleByTaleName(TaleName);
        }
        else
        {
            mm.PanelMenu = PanelMainMenu;
            mm.RunViewByTaleName(TaleName);
        }

        Utils.HideOtherPanels(PanelEditOrView);
    }
}
