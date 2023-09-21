using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerControle : MonoBehaviour
{
    [SerializeField] private Joystick joystick; // Поле для джойстика
    [SerializeField] private Transform feetPos;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;


    [SerializeField] private float moveSpeed = 5.0f; // Параметр для визначення швидкості руху персонажа
    [SerializeField] private float moveDown = 4.0f;
    [SerializeField] private float jumpHeight = 10.0f; // Параметр для визначення висоти прижка персонажа
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
        // Отримуємо значення з джойстика
        float horizontalInput = joystick.Horizontal;

        // Рухаємо персонажа вліво і вправо
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if ((horizontalInput * moveSpeed) < 0)
            spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;

        animator.SetFloat("Horizontal Move", Mathf.Abs(horizontalInput * moveSpeed));
        // Перевіряємо, чи персонаж на землі (ваш код може відрізнятися)
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadiusGround, whatIsGround);
        

        // Перевіряємо, чи натиснуто на джостик вниз
        if (joystick.Vertical < -0.2f)
        {
            // Збільшуємо гравітацію, щоб персонаж швидше падав
            rb.gravityScale = moveDown*joystick.Vertical;
        }
        else
        {
            // Повертаємо гравітацію до звичайного значення
            rb.gravityScale = 1.0f;
        }
        if (isGrounded == true)
            leftJump = maxDopJump;
        // Перевіряємо, чи джостик вгору (близько до самої гори)
        if (joystick.Vertical > 0.8f && leftTimeJump<=0)
        {
            // Перевіряємо, чи є доступні прижки
            if (isGrounded == true )
            {
                // Пригаємо
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
