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

    // ---------------------- MOVIMIENTO ENTRE WAYPOINTS ----------------------
    private void Patrullar()
    {
        if (waypoints.Length == 0) return;

        Transform objetivo = waypoints[currentWaypoint];
        Vector3 direccion = (objetivo.position - transform.position);
        float distancia = direccion.magnitude;

        if (distancia > stoppingDistance)
        {
            direccion.Normalize();
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccion), Time.deltaTime * 5f);
            transform.position += direccion * speed * Time.deltaTime;
        }
        else
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    // ---------------------- COLISIÓN CON EL JUGADOR ----------------------
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("NPC colisionó con: " + collision.collider.name);
        if (!puedeEmpujar) return; // evita repetición inmediata

        if (collision.gameObject.tag == "Player")
        {
            Rigidbody rbJugador = collision.collider.GetComponent<Rigidbody>();
            Debug.Log("Rigidbody del jugador encontrado: " + (rbJugador != null));
            if (rbJugador != null)
            {
                // Calcular dirección opuesta + impulso hacia arriba
                Vector3 direccionEmpuje = (collision.transform.position - transform.position).normalized;
                direccionEmpuje.y = 0.5f; // pequeño impulso vertical
                Vector3 fuerzaTotal = direccionEmpuje.normalized * fuerzaEmpuje + Vector3.up * fuerzaVertical;

                rbJugador.AddForce(fuerzaTotal, ForceMode.Impulse);
                Debug.Log("NPC empujó al jugador con fuerza: " + fuerzaTotal);
            }

            // Si este objeto tiene el tag "Obstacle", resta puntos
            if (CompareTag(obstacleTag) && gameManager != null)
                gameManager.RestarPuntos(1);

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
