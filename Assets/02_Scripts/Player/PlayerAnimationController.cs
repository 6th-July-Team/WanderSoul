using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    public bool CanStartUpperBodyAction => !IsUpperBodyActionPlaying();

    // Animator parameter hashes
    private static readonly int _isMoveHash = Animator.StringToHash("MoveSpeed");
    private static readonly int _isSpecialAttackHash = Animator.StringToHash("isSpecialAttack");
    private static readonly int _isUltimateAttackHash = Animator.StringToHash("isUltimateAttack");

    private const string _upperBodyActionTag = "UpperBodyAction";
    [SerializeField] private int _upperBodyActionLayerIndex = 1;
    [SerializeField] private float _moveDampTime = 0.1f;


    private Animator _animator;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        moveSpeed = Mathf.Clamp01(moveSpeed);
        _animator.SetFloat(_isMoveHash, moveSpeed, _moveDampTime, Time.deltaTime);
    }

    public void PlayAction(PlayerAnimationAction anim)
    {

        int triggerHash = anim switch
        {
            PlayerAnimationAction.SpecialAttack => _isSpecialAttackHash,
            PlayerAnimationAction.UltimateAttack => _isUltimateAttackHash,
            _ => throw new System.ArgumentOutOfRangeException(nameof(anim), anim, null)
        };

        UpdateActionTrigger();
        _animator.SetTrigger(triggerHash);
    }

    public bool IsUpperBodyActionPlaying()
    {
        AnimatorStateInfo currentInfo = _animator.GetCurrentAnimatorStateInfo(_upperBodyActionLayerIndex);

        if(currentInfo.IsTag(_upperBodyActionTag))
        {
            return true;
        }

        if(!_animator.IsInTransition(_upperBodyActionLayerIndex))
        {
            return false;
        }

        AnimatorStateInfo nextInfo = _animator.GetNextAnimatorStateInfo(_upperBodyActionLayerIndex);

        return nextInfo.IsTag(_upperBodyActionTag);
    }

    private void UpdateActionTrigger()
    {
        _animator.ResetTrigger(_isSpecialAttackHash);
        _animator.ResetTrigger(_isUltimateAttackHash);
    }
}

public enum PlayerAnimationAction
{
    SpecialAttack,
    UltimateAttack
}