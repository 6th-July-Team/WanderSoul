using UnityEngine;

[RequireComponent(typeof(PlayerInputHandle))]
public class PlayerCombatController : MonoBehaviour
{
    // Components
    private PlayerInputHandle _inputHandle;
    private PlayerAimHandler _aimHandler;


    private void Awake()
    {
        _inputHandle = GetComponent<PlayerInputHandle>();
        _aimHandler = GetComponent<PlayerAimHandler>();
    }

    // TODO(김익환): 바인더 클래스를 만드는 것을 고려
    private void OnEnable()
    {
        _inputHandle.OnLeftClickEvent += OnBasicAttack;
    }

    private void OnDisable()
    {
        _inputHandle.OnLeftClickEvent -= OnBasicAttack;
    }

    private void Update()
    {
        if (GameManager.Time.IsPaused)
            return;
    }

    // TEST: 일반 공격 방향 체크
    private void OnBasicAttack()
    {
        var direction = _aimHandler.AimDirection;

        BasicAttackTestProjectile a = Instantiate(Utils.ResourcesLoad<BasicAttackTestProjectile>("BasicAttackTestProjectile"), transform.position, Quaternion.LookRotation(direction));
        a.Init(direction);
    }
}
