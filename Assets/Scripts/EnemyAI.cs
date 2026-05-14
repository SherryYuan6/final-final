using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] string gameOverScene = "EndScene";

    Transform player;
    NavMeshAgent agent;
    float attackTimer;

    private bool isDead;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        agent.SetDestination(player.position);

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                SceneManager.LoadScene(gameOverScene);
            }
        }
        else
        {
            attackTimer = 0f;
        }
    }
    public void TryHitWithAxe()
    {
        if (isDead) return;

        ItemData selectedItem = ToolBarUI.Instance?.GetSelectedItem();

        if (selectedItem != null && selectedItem.itemID == "Axe")
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.enabled = false;
        Destroy(gameObject, 0.2f);
    }
}