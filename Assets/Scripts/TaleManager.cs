using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType { Move, Rotate, Height };

public class TaleManager : MonoBehaviour
{
    public GameObject TextSceneNumber;
    public GameObject TextScriptSceneNumber;
    public GameObject TextCurrentTale;

    public GameObject CurrentScene;
    public int CurrentSceneId;

    public ButtonScene SelectedBtnScene;

    public GameObject ImgTarget;

    public GameObject BtnScriptBack;
    public GameObject InputSceneName;
    public GameObject InputSceneScript;

    public GameObject BtnBack;
    public GameObject BtnAdd;
    public GameObject BtnShow;
    public GameObject BtnRemove;

    public GameObject Lines;
    public GameObject BtnAddLink;
    public GameObject BtnRemoveLink;
    public bool IsModeLink = false;
    public int LinkFirstScene;
    public Dictionary<int, List<int>> Links { get; set; }

    public GameObject PanelScenesManager;
    public GameObject PanelScenesGraph;
    public GameObject TmplBtnScene;

    public Color ColorActionSelected;
    public Color ColorActionUnselected;
    public GameObject BtnScript;
    public GameObject BtnMove;
    public GameObject BtnRotate;
    public GameObject BtnHeight;
    public ActionType actionType;

    public int LastSceneNumber;

    public GameObject CurrentMoveObj = null;

    public List<string> SceneNames;
    public List<string> SceneScripts;

    public bool IsViewMode { get; set; } = false;

    private string _taleName;
    public string TaleName
    {
        get
        {
            return _taleName;
        }
        set
        {
            TextCurrentTale.GetComponent<Text>().text = "Сказка: " + value;
            _taleName = value;
        }
    }

    void Start()
    {
        ClearTale();

        PanelScenesManager.SetActive(false);

        BtnScriptBack.GetComponent<Button>().onClick.AddListener(BtnBackOnClick);

        BtnBack.GetComponent<Button>().onClick.AddListener(BtnBackOnClick);
        BtnAdd.GetComponent<Button>().onClick.AddListener(BtnAddOnClick);
        BtnShow.GetComponent<Button>().onClick.AddListener(BtnShowOnClick);
        BtnRemove.GetComponent<Button>().onClick.AddListener(BtnRemoveOnClick);

        BtnAddLink.GetComponent<Button>().onClick.AddListener(BtnAddLinkOnClick);
        BtnRemoveLink.GetComponent<Button>().onClick.AddListener(BtnRemoveLinkClick);

        BtnScript.GetComponent<Button>().onClick.AddListener(BtnShowPanelScriptOnClick);
        BtnMove.GetComponent<Button>().onClick.AddListener(() => SetActionType(ActionType.Move));
        BtnRotate.GetComponent<Button>().onClick.AddListener(() => SetActionType(ActionType.Rotate));
        BtnHeight.GetComponent<Button>().onClick.AddListener(() => SetActionType(ActionType.Height));

        BtnAddOnClick(); // first scene

        RenderScene();
    }

    internal void CreateLink(int secondScene)
    {
        IsModeLink = false;

        int a = Math.Min(LinkFirstScene, secondScene);
        int b = Math.Max(LinkFirstScene, secondScene);

        LinkFirstScene = -1;

        Debug.Log("create link " + a + " " + b);

        if (!Links.ContainsKey(a))
        {
            Links.Add(a, new List<int>());
        }
        if (Links[a].Contains(b))
        {
            return;
        }
        Links[a].Add(b);
        RenderLinks();
    }

    public void ShowSceneById(int id)
    {
        Debug.Log("ShowSceneById " + id);
        foreach (Transform btn in PanelScenesGraph.transform)
        {
            var btnScene = btn.gameObject.GetComponent<ButtonScene>();
            if (btnScene && btnScene.SceneId == id)
            {
                btnScene.IsCurrent = true;
                SelectedBtnScene = btnScene;
                btnScene.Scene.SetActive(true);
                //break;
            }
            else if (btnScene && btnScene.Scene)
            {
                btnScene.Scene.SetActive(false);
            }
        }
        //UpdateVisibleScenes();
        //BtnShowOnClick();
    }

    public void RenderLinks()
    {
        foreach (Transform t in Lines.transform)
        {
            Destroy(t.gameObject);
        }
        foreach (var kv in Links)
        {
            int a = kv.Key;
            foreach (int b in kv.Value)
            {
                GameObject obj = new GameObject();
                obj.transform.SetParent(Lines.transform);
                var lineImage = obj.AddComponent(typeof(Image)) as Image;
                lineImage.color = ColorActionUnselected;
                ButtonScene objA = FindButtonById(a);
                ButtonScene objB = FindButtonById(b);
                Vector3 delta = objA.transform.position - objB.transform.position;
                float width = delta.magnitude;
                lineImage.rectTransform.sizeDelta = new Vector2(width, 2);
                lineImage.rectTransform.pivot = new Vector2(0, 0);
                lineImage.rectTransform.localPosition = objA.transform.position;
                var rad = (float)Math.Asin(delta.normalized.y);
                lineImage.rectTransform.Rotate(new Vector3(0, 0, -rad * 180 / 3.14f));
            }
        }
    }

    public ButtonScene FindButtonById(int id)
    {
        foreach (Transform t in PanelScenesGraph.transform)
        {
            var bs = t.GetComponent<ButtonScene>();
            if (bs && bs.SceneId == id)
            {
                return bs;
            }
        }
        return null;
    }

    public ButtonScene FindButtonByScene(Transform scene)
    {
        foreach (Transform t in PanelScenesGraph.transform)
        {
            var bs = t.GetComponent<ButtonScene>();
            if (bs && bs.Scene == scene.gameObject)
            {
                return bs;
            }
        }
        return null;
    }

    private void BtnAddLinkOnClick()
    {
        IsModeLink = true;
    }

    private void BtnRemoveLinkClick()
    {

    }

    private void BtnShowPanelScriptOnClick()
    {
        IsViewMode = true;
        Utils.HideOtherPanels(GetComponent<MenuManager>().PanelScript);
    }

    private void BtnBackOnClick()
    {
        IsViewMode = false;
        Utils.HideOtherPanels(GetComponent<MenuManager>().PanelTale);
    }

    private void SetActionType(ActionType actionType)
    {
        this.actionType = actionType;
        BtnMove.GetComponent<Image>().color = ColorActionUnselected;
        BtnRotate.GetComponent<Image>().color = ColorActionUnselected;
        BtnHeight.GetComponent<Image>().color = ColorActionUnselected;
        if (actionType == ActionType.Move)
        {
            BtnMove.GetComponent<Image>().color = ColorActionSelected;
        }
        else if (actionType == ActionType.Rotate)
        {
            BtnRotate.GetComponent<Image>().color = ColorActionSelected;
        }
        else if (actionType == ActionType.Height)
        {
            BtnHeight.GetComponent<Image>().color = ColorActionSelected;
        }
    }

    public void ClearTale()
    {
        Links = new Dictionary<int, List<int>>();
        SceneNames = new List<string>();
        SceneScripts = new List<string>();
        LinkFirstScene = -1;
        IsModeLink = false;
        LastSceneNumber = 1;
        SelectedBtnScene = null;
        CurrentScene = null;
        CurrentSceneId = 0;
        foreach (Transform sc in ImgTarget.transform)
        {
            Destroy(sc.gameObject);
        }

        foreach (Transform sc in PanelScenesGraph.transform)
        {
            Destroy(sc.gameObject);
        }
        Lines = new GameObject();
        Lines.transform.SetParent(PanelScenesGraph.transform);

        DrawPreviewSceneObjects drawerPreview = GetComponent<DrawPreviewSceneObjects>();
        drawerPreview.ClearObjectsForScene();
        foreach (Transform sc in drawerPreview.ContentScroll.transform)
        {
            Destroy(sc.gameObject);
            Destroy(sc.gameObject.GetComponent<PreviewSceneObject>().sceneObject);
        }
        drawerPreview.ClearObjectsForScene();
    }

    public void BtnAddOnClick()
    {
        CreateScene("Scene " + LastSceneNumber);
        SceneNames.Add("");
        SceneScripts.Add("");
    }

    public ButtonScene CreateScene(string btnName)
    {
        GameObject btnScene = Instantiate(TmplBtnScene, PanelScenesGraph.transform);
        btnScene.GetComponentInChildren<Text>().text = btnName;

        GameObject scene = Instantiate(new GameObject(), ImgTarget.transform);

        bool currentSceneIsNull = CurrentScene == null;

        ButtonScene bs = btnScene.GetComponent<ButtonScene>();
        bs.Init(scene, this, currentSceneIsNull, LastSceneNumber);

        if (currentSceneIsNull)
        {
            CurrentScene = scene;
            CurrentSceneId = LastSceneNumber;
            SelectedBtnScene = bs;
        }

        LastSceneNumber++;

        return bs;
    }

    private void BtnShowOnClick()
    {
        if (SelectedBtnScene == null)
        {
            BtnBackOnClick();
            return;
        }
        Debug.Log("SelectedId " + SelectedBtnScene.SceneId);
        CurrentScene = SelectedBtnScene.Scene;
        CurrentSceneId = SelectedBtnScene.SceneId;
        RenderScene(SelectedBtnScene.SceneId);
        UpdateVisibleScenes();
        foreach (Transform sceneBtn in PanelScenesGraph.transform)
        {
            var tmpBtnScene = sceneBtn.GetComponent<ButtonScene>();
            if (tmpBtnScene)
            {
                tmpBtnScene.IsCurrent = tmpBtnScene == SelectedBtnScene;
                tmpBtnScene.toggleSelection(false);
            }
        }
        BtnBackOnClick();
    }

    public void UpdateVisibleScenes()
    {
        foreach (Transform scene in ImgTarget.transform)
        {
            scene.gameObject.SetActive(scene.gameObject == SelectedBtnScene.Scene);
        }

        InputSceneName.GetComponent<InputField>().text = SceneNames[SelectedBtnScene.SceneId - 1];
        InputSceneScript.GetComponent<InputField>().text = SceneScripts[SelectedBtnScene.SceneId - 1];
    }

    private void BtnRemoveOnClick()
    {
        if (SelectedBtnScene == null)
        {
            return;
        }
        Destroy(SelectedBtnScene.Scene);
        Destroy(SelectedBtnScene.gameObject);
    }

    private void RenderScene(int id = 1)
    {
        TextSceneNumber.GetComponent<Text>().text = "Scene " + id;
        TextScriptSceneNumber.GetComponent<Text>().text = "Scene " + id;
    }

    internal void SelectSceneBtn(ButtonScene buttonScene)
    {
        SelectedBtnScene = buttonScene;
        foreach (Transform btn in PanelScenesGraph.transform)
        {
            var btnScene = btn.gameObject.GetComponent<ButtonScene>();
            if (btnScene != null)
            {
                btnScene.toggleSelection(btnScene == buttonScene);
            }
        }
    }

    public void OnInputSceneNameChanged()
    {
        SceneNames[CurrentSceneId - 1] = InputSceneName.GetComponent<InputField>().text;
    }

    public void OnInputSceneScriptChanged()
    {
        SceneScripts[CurrentSceneId - 1] = InputSceneScript.GetComponent<InputField>().text;
    }
}
