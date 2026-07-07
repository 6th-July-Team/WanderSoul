using UnityEngine;

public class InputBinder
{
    PlayerInputHandle _inputHandle;

    CursorHandler _cursor = new();

    public InputBinder(PlayerInputHandle inputHandle)
    {
        _inputHandle = inputHandle;

        _cursor.Init();
    }

    public void Bind()
    {
        _inputHandle.OnLeftClickEvent += _cursor.OnLeftClickEffect;
    }

    public void UnBind()
    {
        _inputHandle.OnLeftClickEvent -= _cursor.OnLeftClickEffect;
    }
}
