using Assets.Scripts;
using Assets.Scripts.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Share : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShareHandler);
    }

    private void ShareHandler()
    {
        GameObject camera = GameObject.Find("ARCamera");
        camera.GetComponent<MenuManager>().ShareTale();
    }
}
