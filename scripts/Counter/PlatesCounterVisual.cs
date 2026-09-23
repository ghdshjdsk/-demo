using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterToTop;
    [SerializeField] private Transform platesPrefab;

    [SerializeField] private PlatesCounter platesCounter;

    private List<GameObject> plateVisualGameObjectsList;

    private void Awake()
    {
        plateVisualGameObjectsList = new List<GameObject>();
    }

    void Start()
    {
        platesCounter.OnPlateSpwaned += PlatesCounter_OnPlateSpwaned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemove;
    }

    private void PlatesCounter_OnPlateSpwaned(object sender,EventArgs e)
    {
        Transform plate = Instantiate(platesPrefab,counterToTop);
        float plateOffsetY = 0.1f;

        plate.localPosition = new Vector3(0,plateOffsetY * plateVisualGameObjectsList.Count,0);

        plateVisualGameObjectsList.Add(plate.gameObject);
    }

    private void PlatesCounter_OnPlateRemove(object sender,EventArgs e)
    {
        GameObject removePlate = plateVisualGameObjectsList[plateVisualGameObjectsList.Count - 1];

        plateVisualGameObjectsList.Remove(removePlate);

        Destroy(removePlate);
    }
}
