using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FeedAnimation : MonoBehaviour
{

    Animator m_Animator;
    public bool isEating = false;
    public float animAddTime;
    private float animTime;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIsEating();
    }

    void UpdateIsEating()
    {
        Animation anim = GetComponent<Animation>();
        if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            StopAnimation();
        }
    }

    public void BeginAnimation()
    {
        m_Animator.Play("Eating", 0, 0f);
        isEating = true;
    }

    public void StopAnimation()
    {
        m_Animator.Stop();
        isEating = false;
    }
}
