using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveObj : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Camera camera;
    private TaleManager taleManager;

    public string ModelFilename;

    /*ActionType actionType;
    Vector2 mousePos;
    DateTime actionTime;*/

    void Start()
    {
        camera = GameObject.Find("ARCamera").GetComponent<Camera>();
        taleManager = GameObject.Find("ARCamera").GetComponent<TaleManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (camera == null)
            {
                return;
            }

            if (taleManager.CurrentMoveObj != gameObject)
            {
                return;
            }

            if (taleManager.IsViewMode)
            {
                return;
            }

            if (taleManager.actionType == ActionType.Move)
            {
                ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
            }
            else if (taleManager.actionType == ActionType.Rotate)
            {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y - Input.GetAxis("Mouse X") * 10,
                    transform.eulerAngles.z
                );
            }
            else if (taleManager.actionType == ActionType.Height)
            {
                ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                }
            }
        }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (taleManager.CurrentMoveObj == null)
        {
            taleManager.CurrentMoveObj = gameObject;
        }
    }

    void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        taleManager.CurrentMoveObj = null;
    }
}
