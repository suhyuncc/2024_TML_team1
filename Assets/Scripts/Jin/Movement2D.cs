using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector2 moveDirection = Vector2.zero;
    [SerializeField]
    private Animator _Animator;                              // 애니메이터
    private float baseMoveSpeed;
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }
    public void MoveTo(Vector2 direction)
    {
        moveDirection = direction;

        if (this.transform.CompareTag("Enemy"))
        {
            // 우측
            if (moveDirection.x > 0)
            {
                _Animator.SetTrigger("Right");
            }
            // 좌측
            else if (moveDirection.x < 0)
            {
                _Animator.SetTrigger("Left");
            }
            // 위
            else if (moveDirection.y > 0)
            {
                _Animator.SetTrigger("Up");
            }
            // 아래
            else if (moveDirection.y < 0)
            {
                _Animator.SetTrigger("Down");
            }
        }
        
    }
    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
}
