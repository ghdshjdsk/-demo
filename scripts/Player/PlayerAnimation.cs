using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private Player player;
    private const string IS_WALKING = "IsWalking";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(IS_WALKING,player.IsWalking());
    }
}
