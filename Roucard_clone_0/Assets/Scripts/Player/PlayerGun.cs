using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerGun : NetworkBehaviour
{
    public NetworkObject bulletPrefab; // ������ ���
    public Transform firePoint; // �������, ����� ����������
    public float bulletSpeed = 10f; // �������� ���
    public Joystick joystick; // ��������
    public float fireRate = 0.5f; // ������� �������
    private float nextFireTime = 0f; // ��� ���������� �������
    private bool canShoot = true; // ��������� �������
    private float lastRotation = 0f; // ������� ��� ���������

    private void Start()
    {
        joystick = GameObject.Find("Gun Joystick ").GetComponent<Joystick>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }

    // ��������� ����� ����
    void FixedUpdate()
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

        // �������� ���� �� ������� � �������������� NetworkManager.Instantiate
        NetworkObject bulletNetworkObject = NetworkManager.Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(new Vector3(0, 0, angle + 90)));

        // �������� ��������� BulletNetwork � ��������������� ��� ��������� ���������
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