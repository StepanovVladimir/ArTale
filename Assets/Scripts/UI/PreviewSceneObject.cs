using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewSceneObject : MonoBehaviour
{
    public GameObject sceneObject;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnMouseClick);
    }
    
    void OnMouseClick()
    {
        Debug.Log("Drag ended!");
        Vector3 Ray_start_position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Ray_start_position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        var CurrentScene = camera.GetComponent<TaleManager>().CurrentScene;
        Debug.Log(sceneObject);
        GameObject obj = Instantiate(sceneObject, CurrentScene.transform);
        obj.SetActive(true);
        obj.transform.position = new Vector3(hit.point.x, -1f, hit.point.z);
        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, -1f, obj.transform.localPosition.z);
        //Debug.Log(hit.point);
        //Debug.Log(obj.transform.position);
    }
}
