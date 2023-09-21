using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerControle : MonoBehaviour
{
    [SerializeField] private Joystick joystick; // ���� ��� ���������
    [SerializeField] private Transform feetPos;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform Body;



    [SerializeField] private float moveSpeed = 5.0f; // �������� ��� ���������� �������� ���� ���������
    [SerializeField] private float moveDown = 4.0f;
    [SerializeField] private float jumpHeight = 10.0f; // �������� ��� ���������� ������ ������ ���������
    [SerializeField] private float maxTimeDopJump=0.2f;
    [SerializeField] private int maxDopJump = 2;
    [SerializeField] private float checkRadiusGround;

   
    
    

    private Rigidbody2D rb;
    private bool isGrounded;
    private float leftTimeJump = 0.2f;
    private int leftJump = 2;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �������� �������� � ���������
        float horizontalInput = joystick.Horizontal;

        // ������ ��������� ���� � ������
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if ((horizontalInput * moveSpeed) < 0)
        {
            Vector3 newScale = Body.transform.localScale;
            newScale.x = -1;
            Body.transform.localScale = newScale;
        }
        else if((horizontalInput * moveSpeed) > 0) {

            Vector3 newScale = Body.transform.localScale;
            newScale.x = 1;
            Body.transform.localScale = newScale;
        }

        animator.SetFloat("Horizontal Move", Mathf.Abs(horizontalInput * moveSpeed));
        // ����������, �� �������� �� ���� (��� ��� ���� ����������)
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadiusGround, whatIsGround);
        

        // ����������, �� ��������� �� ������� ����
        if (joystick.Vertical < -0.2f)
        {
            // �������� ���������, ��� �������� ������ �����
            rb.gravityScale = moveDown*joystick.Vertical;
        }
        else
        {
            // ��������� ��������� �� ���������� ��������
            rb.gravityScale = 1.0f;
        }
        if (isGrounded == true)
            leftJump = maxDopJump;
        // ����������, �� ������� ����� (������� �� ���� ����)
        if (joystick.Vertical > 0.8f && leftTimeJump<=0)
        {
            // ����������, �� � �������� ������
            if (isGrounded == true )
            {
                // �������
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                leftTimeJump = maxTimeDopJump;
                leftJump--;
            }
            else if (leftJump != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                leftTimeJump = maxTimeDopJump;
                leftJump--;
            }
        }


        leftTimeJump-=Time.deltaTime;
    }
}
