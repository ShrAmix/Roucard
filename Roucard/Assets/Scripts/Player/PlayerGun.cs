using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public GameObject bulletPrefab; // ������ ���
    public Transform firePoint; // �������, ����� ����������
    public float bulletSpeed = 10f; // �������� ���
    public Joystick joystick; // ��������
    public float fireRate = 0.5f; // ������� �������
    private float nextFireTime = 0f; // ��� ���������� �������
    private bool canShoot = true; // ��������� �������
    private float lastRotation = 0f; // ������� ��� ���������

    // ��������� ����� ����
    void Update()
    {
        // �������� �������� ��������� �� �� X � Y
        float joystickHorizontal = joystick.Horizontal;
        float joystickVertical = joystick.Vertical;
        if (joystickHorizontal * joystickHorizontal + joystickVertical * joystickVertical >= 0.01f * 0.01f)
            lastRotation = Mathf.Atan2(joystickVertical, joystickHorizontal) * Mathf.Rad2Deg;
        // ����������, �� ����� ������� �� �������� �������
        if (Time.time >= nextFireTime)
        {
            canShoot = true;
        }
        
        // ����������, �� �������� �������� �� 0.8 �� ������ � �����
        if (joystickHorizontal * joystickHorizontal + joystickVertical * joystickVertical >= 0.8f * 0.8f)
        {
            if (canShoot)
            {
                
                Shoot(joystickHorizontal, joystickVertical); // ��������� ����� �������
                canShoot = false;
                nextFireTime = Time.time + fireRate;
            }
        }

        // ������� ��'���� �� 360 �������
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, lastRotation));
    }

    // ����� �������
    void Shoot(float horizontal, float vertical)
    {
        float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;

        // ��������� ���� � ������� � �������� �� � �������� ���������
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(new Vector3(0, 0, angle + 90)));

        // �������� ��������� Rigidbody2D ���
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // ����������� �������� ��� � ��������, � ����� �������� ��'���
        rb.velocity = transform.right * bulletSpeed;
    }
}
