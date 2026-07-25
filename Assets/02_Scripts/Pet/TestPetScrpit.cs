using UnityEngine;

public class TestPetScrpit : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private bool IsMove;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Stop();
    }

    private void Stop()
    {
        switch (IsMove)
        {
            case true:
                {
                    _animator.speed = 1;
                }
                break;
            case false:
                {
                    _animator.speed = 0;
                }
                break;
        }
}
}
