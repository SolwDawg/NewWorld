using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void SetAnimValues(float horizontalValue, float verticalValue)
    {
        animator.SetFloat("Vertical", verticalValue);
        animator.SetFloat("Horizontal", horizontalValue);
    }
}
