using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SyncAnimation : MonoBehaviour
{
    public double beatsPerMinute;

    private const float StartsAt = 5.6f;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(Play(StartsAt));
    }

    private IEnumerator Play(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.Play("Pulse");
        // speed up OR slow down the animation to match the BPM independently of its length
        _animator.speed = (float) beatsPerMinute / 60 * _animator.GetCurrentAnimatorClipInfo(0).Length;
    }
}