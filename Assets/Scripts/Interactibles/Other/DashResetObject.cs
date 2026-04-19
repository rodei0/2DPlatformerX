using System.Collections;
using UnityEngine;

public class DashResetObject : MonoBehaviour
{
    public bool canUse = true;
    [SerializeField] private float useCd;
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnEnter()
    {
            animator.SetBool("isTouched", true);
            StartCoroutine(DashResetCd());
    }
    private IEnumerator DashResetCd()
    {
        canUse = false;
        yield return new WaitForSeconds(useCd);
        animator.SetBool("isTouched", false);
        canUse = true;
    }
}
