using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3.0f;        // Velocidade de movimento
    private Transform player;         // Referência ao Transform do Player

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("EnemyFollow: Nenhum objeto com a tag 'Player' foi encontrado!");
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPos);
    }
}
