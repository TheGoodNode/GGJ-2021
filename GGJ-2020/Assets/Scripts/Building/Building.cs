﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public string buildingName;
    [SerializeField] float height;
    public float radius = 5;
    float originalHeight;
    [SerializeField] int totalWorkToComplete = 100;
    int currentWork;
    public int[] resourceCost = default;
    public Transform buildingTransform;
    [HideInInspector] public Damageable attackable;
    public bool isHover = false;
    [HideInInspector] public bool done;
    [SerializeField] private Color[] stateColors;

    MeshRenderer buildingRender;

    Cinemachine.CinemachineImpulseSource impulse;


    [HideInInspector] public UnityEvent buildIsDone = new UnityEvent();

    public MeshFilter[] allMeshForm;
    private void Awake()
    {
        attackable = gameObject.GetComponent<Damageable>();
    }


    private void Start()
    {
        buildingRender = buildingTransform.GetComponent<MeshRenderer>();
        // impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
        currentWork = 0;
        originalHeight = buildingTransform.localPosition.y;
        buildingTransform.localPosition = Vector3.down * height;
    }

    public void Build(int work)
    {
        currentWork += work;
        buildingTransform.localPosition = Vector3.Lerp(Vector3.down * height, new Vector3(0, originalHeight, 0), (float)currentWork / totalWorkToComplete);

        //visual
        buildingTransform.DOComplete();
        buildingTransform.DOShakeScale(.5f, .2f, 10, 90, true);
        // BuildingManager.instance.PlayParticle(transform.position);
    }

    public bool IsFinished()
    {
        Debug.Log("Progress: " + currentWork);
        if (currentWork >= totalWorkToComplete && !done)
        {
            done = true;
            buildIsDone.Invoke();
            Debug.LogWarning("Ran invoke");
            // buildingRender.material.DOColor(stateColors[1], "_EmissionColor", .1f).OnComplete(() => buildingRender.material.DOColor(stateColors[0], "_EmissionColor", .5f));
            if (impulse)
                impulse.GenerateImpulse();

            return true;
        }
        return currentWork >= totalWorkToComplete;
    }

    public bool CanBuild(int[] resources)
    {
        bool canBuild = true;
        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resources[i] < resourceCost[i])
            {
                canBuild = false;
                break;
            }
        }
        return canBuild;
    }

    public int[] Cost()
    {
        return resourceCost;
    }

    private void OnMouseEnter()
    {
        isHover = true;
    }

    private void OnMouseExit()
    {
        isHover = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
