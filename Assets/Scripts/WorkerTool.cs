using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTool : MonoBehaviour
{
    /*
     * The tool the worker uses - is animated to show visual representation of his work
     */

    public Animator animator;

    public void StartAnimation()
    {
        animator.SetBool("Play", true);
    }

    public void StopAnimation()
    {
        animator.SetBool("Play", false);

    }
}
