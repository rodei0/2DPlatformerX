using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DashResetObject : MonoBehaviour
{
    public bool canUse = true;
    [SerializeField] private float useCd;

    private Animator animator;
    private LightOscile lightOscile;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //light = GetComponent<Light2D>();
        lightOscile = GetComponent<LightOscile>();
    }

    public void OnEnter()
    {
        animator.SetBool("isTouched", true);
        CameraShake.Instance.Shake(0.04f, 0.15f);
        StartCoroutine(DashResetCd());
        lightOscile.FadelightFunc();
    }
    private IEnumerator DashResetCd()
    {
        canUse = false;
        yield return new WaitForSeconds(useCd);
        animator.SetBool("isTouched", false);
        yield return StartCoroutine(lightOscile.FadeInLightFunc());
        canUse = true;
    }
}
