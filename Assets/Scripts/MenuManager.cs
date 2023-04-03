using Assets.Scripts;
using Assets.Scripts.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject BtnCloseMenu;
    public GameObject PanelMenu;
    public GameObject PanelEditorMenu;

    public GameObject BtnSaveTale;

    public GameObject TalesList;
    public GameObject TalesListView;

    public GameObject TalesListItem;

    public GameObject TaleLinkOutput;
    public GameObject ButtonCopyLink;

    public GameObject TaleLinkInput;
    public GameObject BtnLoadTaleFromServer;

    public GameObject ButtonLoadModels;

    public GameObject PanelMessage;
    public GameObject ButtonOk;
    public GameObject LabelMessage;

    public GameObject PanelSelection;
    public GameObject LabelSelection;

    public GameObject BtnRunView;
    public GameObject PanelTale;
    public GameObject PanelScript;
    public GameObject PanelWholeText;
    public GameObject PanelTaleView;

    private TaleModel _taleModel;

    public GameObject PanelMainMenu;

    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

        Utils.Init();
        Utils.HideOtherPanels(PanelMainMenu);

        BtnCloseMenu.GetComponent<Button>().onClick.AddListener(MenuClose);

        BtnSaveTale.GetComponent<Button>().onClick.AddListener(OnClickSaveTale);
        ButtonCopyLink.GetComponent<Button>().onClick.AddListener(OnClickCopyLink);

        ButtonLoadModels.GetComponent<Button>().onClick.AddListener(LoadModels);

        BtnLoadTaleFromServer.GetComponent<Button>().onClick.AddListener(OnClickLoadFromServer);

        ButtonOk.GetComponent<Button>().onClick.AddListener(OnClickOk);

        BtnRunView.GetComponent<Button>().onClick.AddListener(RunView);

        _taleModel = new TaleModel(GetComponent<TaleManager>());

        UpdateScrollLoadTale();

        /*AnimationClip[] animationClips;
        var i = new Siccity.GLTFUtility.ImportSettings();
        i.useLegacyClips = true;
        GameObject model = Siccity.GLTFUtility.Importer.LoadFromFile(Application.persistentDataPath + "/Bee.glb", i, out animationClips);
        model.AddComponent<BoxCollider>();
        Debug.Log("animationClips " + animationClips.Length + " " + animationClips[0].name);

        Animation animation = model.AddComponent<Animation>();
        animationClips[0].legacy = true;
        animation.AddClip(animationClips[0], animationClips[0].name);
        animation.clip = animation.GetClip(animationClips[0].name);
        animation.wrapMode = WrapMode.Loop;
        animation.Play();*/

        //PlayableGraph playable = new PlayableGraph();
        //var animator = model.AddComponent<Animator>();
        //AnimationPlayableUtilities.PlayClip(animator, animationClips[2], out playable);
    }

    private void RunView()
    {
        GetComponent<MenuManager>().PanelMenu = PanelEditorMenu;
        GetComponent<ViewManager>().Run(_taleModel.TaleName);
    }

    public void RunViewByTaleName(string taleName)
    {
        GetComponent<ViewManager>().Run(taleName);
    }

    public void LoadTaleByTaleName(string taleName)
    {
        _taleModel.TaleName = taleName;
        _taleModel.Load();
    }

    public void CreateTale(string taleName)
    {
        _taleModel.TaleName = taleName;
        _taleModel.Create();
    }

    void UpdateScrollLoadTale()
    {
        foreach (Transform child in TalesList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in TalesListView.transform)
        {
            Destroy(child.gameObject);
        }

        List<string> talesNames = _taleModel.LoadTaleList();

        int i = 0;
        foreach (string name in talesNames)
        {
            GameObject btn = Instantiate(TalesListItem, TalesList.transform);
            btn.GetComponent<ButtonLoadTale>().TaleName = name;
            btn.GetComponent<ButtonLoadTale>().PanelEditOrView = PanelTale;
            btn.GetComponent<ButtonLoadTale>().IsEdit = true;
            btn.GetComponentInChildren<Text>().text = name;
            btn.SetActive(false);
            RectTransform pos = btn.GetComponent<RectTransform>();
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.anchoredPosition.x, pos.anchoredPosition.y - i * 35);
            btn.SetActive(true);

            GameObject btnView = Instantiate(TalesListItem, TalesListView.transform);
            btnView.GetComponent<ButtonLoadTale>().TaleName = name;
            btnView.GetComponent<ButtonLoadTale>().PanelEditOrView = PanelTaleView;
            btnView.GetComponent<ButtonLoadTale>().PanelMainMenu = PanelMainMenu;
            btnView.GetComponent<ButtonLoadTale>().IsEdit = false;
            btnView.GetComponentInChildren<Text>().text = name;
            btnView.SetActive(false);
            RectTransform posView = btnView.GetComponent<RectTransform>();
            btnView.GetComponent<RectTransform>().anchoredPosition = new Vector2(posView.anchoredPosition.x, posView.anchoredPosition.y - i * 35);
            btnView.SetActive(true);

            i++;
        }
    }

    private void OnClickLoadFromServer()
    {
        try
        {
            Utils.DisableSSL();
            string link = TaleLinkInput.GetComponent<InputField>().text;
            string taleName = _taleModel.Download(link);
            UpdateScrollLoadTale();

            // load scenes
            _taleModel.TaleName = taleName;

            RunView();
        }
        catch (Exception ex)
        {
            //ShowMessage(ex.Message + "  " + ex.StackTrace);
            ex = ex.InnerException;
            ShowMessage(ex.Message);
        }
    }

    public void ShareTale()
    {
        TaleLinkOutput.GetComponent<InputField>().text = "";

        try
        {
            ShareResponse shareResp = _taleModel.Share();

            if (shareResp.status)
            {
                TaleLinkOutput.GetComponent<InputField>().text = shareResp.link;
            }
            else
            {
                ShowMessage(shareResp.message);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message + "  " + ex.StackTrace);
        }
    }

    private void OnClickOk()
    {
        PanelMessage.SetActive(false);
    }

    public void LoadModels()
    {
        string TaleName = _taleModel.TaleName;
        if (TaleName == null || TaleName.Length == 0)
        {
            Debug.Log("Set a name for the tale and save it before doing so.");
            ShowMessage("Set a name for the tale and save it before doing so.");
            return;
        }

        try
        {
            _taleModel.LoadModels(Utils.CalcModelsLoadPath());
        }
        catch (Exception ex)
        {
            Debug.Log("2" + " " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
        }
    }

    public void ShowMessage(string v)
    {
        PanelMessage.SetActive(true);
        LabelMessage.GetComponent<Text>().text = v;
    }

    public void ShowSelection()
    {
        PanelSelection.SetActive(true);
    }

    public void CloseSelection()
    {
        PanelSelection.SetActive(false);
    }

    public void DeleteTale()
    {
        _taleModel.Delete();
        UpdateScrollLoadTale();
        Utils.HideOtherPanels(PanelMainMenu);
    }

    private void OnClickCopyLink()
    {
        TextEditor editor = new TextEditor
        {
            text = TaleLinkOutput.GetComponent<InputField>().text
        };
        editor.SelectAll();
        editor.Copy();
    }

    public void OnClickSaveTale()
    {
        string taleName = _taleModel.TaleName;
        if (taleName.Length == 0)
        {
            return;
        }

        _taleModel.Save();

        UpdateScrollLoadTale();
    }

    public void MenuClose()
    {
        PanelMenu.SetActive(false);
    }
}
