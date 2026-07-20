#if UNITY_EDITOR
using UnityEngine;

public class WagonGizmo : MonoBehaviour
{
    public BoxCollider BoxCollider;
    public Vector3 Size = new Vector3(70f, 5f, 70f);
    public Color GizmoColor = Color.red;

    private void OnValidate()
    {
        BoxCollider.size = Size;
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 previousMatrix = Gizmos.matrix;

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = GizmoColor;
        Gizmos.DrawWireCube(Vector3.zero, Size);

        Gizmos.matrix = previousMatrix;
    }
}
#endif