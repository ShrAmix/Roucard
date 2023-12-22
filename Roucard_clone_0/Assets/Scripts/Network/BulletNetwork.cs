using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletNetwork : NetworkBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // �������������� �������� ���� � �������� ���������
    public void Initialize(Vector2 velocity)
    {
        rb.velocity = velocity;

        // ���������, �� ������� �� ��� �� �������
        if (IsServer)
        {
            // ��������� �������� ��� �������� ���� ����� ��������� �����
            StartCoroutine(DestroyAfterDelay(5f)); // ���� ����� ���������� ����� 5 ������
        }
    }

    // ����� ��� �������� ���� ����� ��������
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NetworkManager.Destroy(gameObject); // ���������� ����
    }
}
