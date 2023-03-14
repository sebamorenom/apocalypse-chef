using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private struct TrapStates
    {
        public static int Activate = Animator.StringToHash("Activate");
        public static int Deactivate = Animator.StringToHash("Deactivate");
    }

    private Animator _animator;
    private AnimationClip[] _animationClips;

    private float _activatedAnimationTime;
    private float _deactivatedAnimationTime;

    private float _activatedTime;
    private float _deactivatedTime;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        GetAnimationClips();
    }

    public void SwitchState()
    {
        if (!isActive && Time.fixedTime > _activatedTime + _activatedAnimationTime)
        {
            _animator.Play(TrapStates.Activate);
            isActive = true;
        }

        if (isActive && Time.fixedTime > _deactivatedTime + _deactivatedAnimationTime)
        {
            _animator.Play(TrapStates.Deactivate);
            isActive = false;
        }
    }

    public void GetAnimationClips()
    {
        _animationClips = _animator.runtimeAnimatorController.animationClips;
        foreach (var animClip in _animationClips)
        {
            switch (animClip.name)
            {
                case "Activated":
                    _activatedAnimationTime = animClip.length;
                    break;
                case "Deactivated":
                    _deactivatedAnimationTime = animClip.length;
                    break;
            }
        }
    }
}