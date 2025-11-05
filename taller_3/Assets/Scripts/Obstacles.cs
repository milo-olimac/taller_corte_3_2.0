using System.Collections;
using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public float speed = 3f;
    public float stoppingDistance = 0.3f;
    public Transform[] waypoints;
    private int currentWaypoint = 0;

    public string playerTag = "Player";
    public string obstacleTag = "Obstacle";
    public float fuerzaEmpuje = 8f;
    public float fuerzaVertical = 3f;
    public float tiempoEsperaColision = 1f;

    private bool puedeEmpujar = true;
    private GameManager gameManager;

    [System.Obsolete]
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (waypoints.Length > 0)
            transform.position = waypoints[0].position;
    }

    private void Update()
    {
        Patrullar();
    }

    private void Patrullar()
    {
        if (waypoints.Length == 0) return;

        Transform objetivo = waypoints[currentWaypoint];
        Vector3 direccion = (objetivo.position - transform.position);
        float distancia = direccion.magnitude;

        if (distancia > stoppingDistance)
        {
            direccion.Normalize();
            transform.position += direccion * speed * Time.deltaTime;
        }
        else
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!puedeEmpujar) return;

        if (collision.gameObject.tag == playerTag)
        {
            Rigidbody rbJugador = collision.collider.GetComponent<Rigidbody>();
            if (rbJugador != null)
            {
                Vector3 direccionEmpuje = (collision.transform.position - transform.position).normalized;
                direccionEmpuje.y = 0.5f;
                Vector3 fuerzaTotal = direccionEmpuje.normalized * fuerzaEmpuje + Vector3.up * fuerzaVertical;

                rbJugador.AddForce(fuerzaTotal, ForceMode.Impulse);
            }

            if (CompareTag(obstacleTag) && gameManager != null)
                gameManager.PerderVida();

            StartCoroutine(EsperarParaEmpujar());
        }
    }

    private IEnumerator EsperarParaEmpujar()
    {
        puedeEmpujar = false;
        yield return new WaitForSeconds(tiempoEsperaColision);
        puedeEmpujar = true;
    }
}
