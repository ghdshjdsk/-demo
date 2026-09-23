using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimation : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    private Animator animator;
    [SerializeField] private ContainerCounter containerCounter;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        containerCounter.OnplayGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object obj,System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
