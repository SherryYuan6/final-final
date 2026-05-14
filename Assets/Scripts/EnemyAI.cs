using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float interactRange = 2f;
    [SerializeField] string gameOverScene = "GameOver";
    Transform _player;
    NavMeshAgent _agent;
    float _attackTimer = 0f;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (_player == null) return;
        _agent.SetDestination(_player.position);
        float dist = Vector3.Distance(transform.position, _player.position);
        if (dist <= attackRange)
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= attackCooldown)
            {
                _attackTimer = 0f;
                UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverScene);
            }
        }
        if (dist <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            ItemData selectedItem = ToolBarUI.Instance.GetSelectedItem();
            if (selectedItem != null && selectedItem.itemID == "Axe")
                Destroy(gameObject);
        }
    }
}