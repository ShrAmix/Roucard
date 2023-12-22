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

    // Инициализируем движение пули с заданной скоростью
    public void Initialize(Vector2 velocity)
    {
        rb.velocity = velocity;

        // Проверяем, на сервере мы или на клиенте
        if (IsServer)
        {
            // Запускаем корутину для удаления пули через некоторое время
            StartCoroutine(DestroyAfterDelay(5f)); // Пуля будет уничтожена через 5 секунд
        }
    }

    // Метод для удаления пули после задержки
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NetworkManager.Destroy(gameObject); // Уничтожаем пулю
    }
}
