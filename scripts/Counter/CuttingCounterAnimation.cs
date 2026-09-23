using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAnimation : MonoBehaviour
{
    private const string OPEN_CLOSE = "Cut";

    private Animator animator;
    [SerializeField] private CuttingCounter cuttingCounter;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        cuttingCounter.OnCut += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object obj,System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
