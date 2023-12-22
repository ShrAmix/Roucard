using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerGun : NetworkBehaviour
{
    public NetworkObject bulletPrefab; // Префаб пулі
    public Transform firePoint; // Позиція, звідки вистрілюємо
    public float bulletSpeed = 10f; // Швидкість пулі
    public Joystick joystick; // Джойстик
    public float fireRate = 0.5f; // Частота вистрілів
    private float nextFireTime = 0f; // Час наступного вистрілу
    private bool canShoot = true; // Можливість стріляти
    private float lastRotation = 0f; // Останній кут обертання

    private void Start()
    {
        joystick = GameObject.Find("Gun Joystick ").GetComponent<Joystick>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }

    // Оновлюємо кожен кадр
    void FixedUpdate()
    {
        // Отримуємо значення джойстика по осі X і Y
        float joystickHorizontal = joystick.Horizontal;
        float joystickVertical = joystick.Vertical;
        if (joystickHorizontal * joystickHorizontal + joystickVertical * joystickVertical >= 0.01f * 0.01f)
            lastRotation = Mathf.Atan2(joystickVertical, joystickHorizontal) * Mathf.Rad2Deg;
        // Перевіряємо, чи можна стріляти за частотою вистрілів
        if (Time.time >= nextFireTime)
        {
            canShoot = true;
        }

        // Перевіряємо, чи джойстик відведено на 0.8 від центру в радіусі
        if (joystickHorizontal * joystickHorizontal + joystickVertical * joystickVertical >= 0.8f * 0.8f)
        {
            if (canShoot)
            {

                Shoot(joystickHorizontal, joystickVertical); // Викликаємо метод вистрілу
                canShoot = false;
                nextFireTime = Time.time + fireRate;
            }
        }

        // Поворот об'єкта на 360 градусів
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, lastRotation));
    }

    // Метод вистрілу
    void Shoot(float horizontal, float vertical)
    {
        float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;

        // Создайте пулю на сервере с использованием NetworkManager.Instantiate
        NetworkObject bulletNetworkObject = NetworkManager.Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(new Vector3(0, 0, angle + 90)));

        // Получите компонент BulletNetwork и инициализируйте его начальной скоростью
        if (bulletNetworkObject != null)
        {
            BulletNetwork bullet = bulletNetworkObject.GetComponent<BulletNetwork>();
            bullet.Initialize(transform.right * bulletSpeed);
        }
        else
        {
            Debug.Log("Failed to instantiate bullet NetworkObject.");
        }
    }
}