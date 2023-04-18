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
    public GameObject BtnWholeTextBack;
    public GameObject BtnMenuBack;
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
    public GameObject BtnWholeText;
    public GameObject BtnMove;
    public GameObject BtnRotate;
    public GameObject BtnHeight;
    public ActionType actionType;

    public GameObject BtnGenerateText;

    public GameObject TextOnPanelWholeText;
    public GameObject PlaceholderOnPanelWholeText;
    public GameObject InputFieldOnPanelWholeText;
    public GameObject ButtonOnPanelWholeText;
    private int indexSceneOnWhichMoveText;

    public int LastSceneNumber;

    public GameObject CurrentMoveObj = null;

    public List<GameObject> Scenes;

    public InputField WholeTextInputField;

    [HideInInspector]
    public List<string> SceneNames;
    [HideInInspector]
    public List<string> SceneScripts;
    [HideInInspector]
    public List<string> SceneDescriptions;

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
        BtnWholeTextBack.GetComponent<Button>().onClick.AddListener(BtnBackOnClick);
        BtnMenuBack.GetComponent<Button>().onClick.AddListener(BtnBackOnClick);

        BtnBack.GetComponent<Button>().onClick.AddListener(BtnBackOnClick);
        BtnAdd.GetComponent<Button>().onClick.AddListener(BtnAddOnClick);
        BtnShow.GetComponent<Button>().onClick.AddListener(BtnShowOnClick);
        BtnRemove.GetComponent<Button>().onClick.AddListener(BtnRemoveOnClick);

        BtnAddLink.GetComponent<Button>().onClick.AddListener(BtnAddLinkOnClick);
        BtnRemoveLink.GetComponent<Button>().onClick.AddListener(BtnRemoveLinkClick);

        BtnScript.GetComponent<Button>().onClick.AddListener(BtnShowPanelScriptOnClick);
        BtnWholeText.GetComponent<Button>().onClick.AddListener(BtnShowPanelWholeTextOnClick);
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

    public void BtnShowPanelWholeTextOnClick()
    {
        IsViewMode = true;
        Utils.HideOtherPanels(GetComponent<MenuManager>().PanelWholeText);
    }

    public void BtnShowPanelMenuOnClick()
    {
        IsViewMode = true;
        Utils.HideOtherPanels(GetComponent<MenuManager>().PanelEditorMenu);
    }

    public void BtnShowPanelScenesOnClick()
    {
        IsViewMode = true;
        Utils.HideOtherPanels(GetComponent<MenuManager>().PanelScenes);
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
        ClearTaleWithoutClearObjectsForScene();

        DrawPreviewSceneObjects drawerPreview = GetComponent<DrawPreviewSceneObjects>();
        drawerPreview.ClearObjectsForScene();
        foreach (Transform sc in drawerPreview.ContentScroll.transform)
        {
            Destroy(sc.gameObject);
            Destroy(sc.gameObject.GetComponent<PreviewSceneObject>().sceneObject);
        }
        drawerPreview.ClearObjectsForScene();
    }

    public void ClearTaleWithoutClearObjectsForScene()
    {
        Links = new Dictionary<int, List<int>>();
        SceneNames = new List<string>();
        SceneScripts = new List<string>();
        SceneDescriptions = new List<string>();
        WholeTextInputField.text = "";
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

        BtnGenerateText.GetComponent<Button>().interactable = false;
        BtnWholeText.SetActive(false);
    }

    public void BtnAddOnClick()
    {
        CreateScene("Сцена " + LastSceneNumber);
        SceneNames.Add("");
        SceneScripts.Add("");
    }

    public ButtonScene CreateScene(string btnName)
    {
        GameObject scene = Instantiate(new GameObject(), ImgTarget.transform);
        return CreateSceneImpl(btnName, scene);
    }

    public ButtonScene CreateScene(string btnName, int sceneIndex)
    {
        GameObject scene = Instantiate(Scenes[sceneIndex], ImgTarget.transform);
        return CreateSceneImpl(btnName, scene);
    }

    private ButtonScene CreateSceneImpl(string btnName, GameObject scene)
    {
        GameObject btnScene = Instantiate(TmplBtnScene, PanelScenesGraph.transform);
        btnScene.GetComponentInChildren<Text>().text = btnName;

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
        RenderScene(SelectedBtnScene.SceneId);
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
        TextSceneNumber.GetComponent<Text>().text = "Сцена " + id;
        TextScriptSceneNumber.GetComponent<Text>().text = "Сцена " + id;
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
        SetOnWhichSceneMoveText();
    }

    public void ActivateBtnGenerateText()
    {
        BtnGenerateText.GetComponent<Button>().interactable = true;
    }

    public void ActivateBtnWholeText()
    {
        BtnWholeText.SetActive(true);
    }

    public void SetOnWhichSceneMoveText()
    {
        for (int i = 0; i < SceneScripts.Count; i++)
        {
            if (SceneScripts[i].Equals(""))
            {
                TextOnPanelWholeText.GetComponent<Text>().text = $"Скопируйте часть текста для сцены \"{i + 1}. {SceneNames[i]}\" сюда:";
                PlaceholderOnPanelWholeText.GetComponent<Text>().text = $"Часть текста о том, как \"{SceneDescriptions[i]}\"";
                indexSceneOnWhichMoveText = i;
                InputFieldOnPanelWholeText.GetComponent<InputField>().interactable = true;
                ButtonOnPanelWholeText.GetComponent<Button>().interactable = true;
                return;
            }
        }

        TextOnPanelWholeText.GetComponent<Text>().text = "Текст во всех сценах заполнен";
        PlaceholderOnPanelWholeText.GetComponent<Text>().text = "";
        InputFieldOnPanelWholeText.GetComponent<InputField>().interactable = false;
        ButtonOnPanelWholeText.GetComponent<Button>().interactable = false;
    }

    public void MoveText()
    {
        SceneScripts[indexSceneOnWhichMoveText] = InputFieldOnPanelWholeText.GetComponent<InputField>().text;
        UpdateVisibleScenes();
        GetComponent<MenuManager>().OnClickSaveTale();
        InputFieldOnPanelWholeText.GetComponent<InputField>().text = "";
        SetOnWhichSceneMoveText();
    }

    public GameObject InstantiateObj(GameObject gameObject)
    {
        return Instantiate(gameObject);
    }

    public string GetMessage()
    {
        string message = "Напиши текст с диалогами для русской народной сказки на основе данной сюжетной линии:\n";
        foreach (string description in SceneDescriptions)
        {
            message += $"{description}\n";
        }

        return message;
    }
}
