﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    bool isPlacing = false;
    int currentIndex = 0;
    public Transform resourceGroup;

    Mesh buildingPreviewMesh;
    [SerializeField] Material buildingPreviewMat;


    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[index].onClick.AddListener(() => SelectBuilding(index));

            Building b = BuildingManager.instance.buildingPrefabs[index];

            // buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = GetButtonText(b);
        }
    }

    private void Update()
    {
        if (isPlacing)
        {
            Vector3 position = Utils.MouseToTerrainPosition();
            Graphics.DrawMesh(buildingPreviewMesh, position, Quaternion.identity, buildingPreviewMat, 0);
            if (Input.GetMouseButtonDown(0))
            {
                BuildingManager.instance.SpawnBuilding(currentIndex, position);
                // canvasGroup.alpha = 1;
                isPlacing = false;
            }
        }
    }


    void SelectBuilding(int index)
    {
        currentIndex = index;
        ActorManager.instance.DeselectActors();
        // canvasGroup.alpha = 0;
        isPlacing = true;
        buildingPreviewMesh = BuildingManager.instance.GetPrefab(index).GetComponentInChildren<MeshFilter>().sharedMesh;

    }




    public void RefreshResources()
    {
        // for (int i = 0; i < resourceGroup.childCount; i++)
        //     resourceGroup.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = BuildingManager.instance.currentResources[i].ToString();
    }
}
