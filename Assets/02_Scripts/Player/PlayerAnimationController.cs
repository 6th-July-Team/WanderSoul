using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private static readonly int _isMoveHash = Animator.StringToHash("isMove");

    private static readonly int _isBasicAttackHash = Animator.StringToHash("isBasicAttack");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMove(bool isMove)
    {
        _animator.SetBool(_isMoveHash, isMove);
    }

    public void PlayBasicAttack()
    {
        _animator.SetTrigger(_isBasicAttackHash);
    }
}