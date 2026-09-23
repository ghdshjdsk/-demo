using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    void Start()
    {
        BaseCounter.ResetStaticData();
        CuttingCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}
