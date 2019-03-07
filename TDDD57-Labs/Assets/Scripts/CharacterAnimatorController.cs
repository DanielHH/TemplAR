using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    private Animator animator;
    private FollowPath pathFollowScript;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Grounded", true);

        pathFollowScript = GetComponent<FollowPath>();
        
    }

    void Update()
    {
        animator.SetFloat("MoveSpeed", pathFollowScript.getSpeed());
    }
}
