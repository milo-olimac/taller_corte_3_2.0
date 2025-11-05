// MetaTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaTrigger : MonoBehaviour
{
    [Header("Configuración")]
    public string escenaFinal = "PantallaFinal"; // Nombre de la escena final
    private bool yaActivado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (yaActivado) return;
        if (!collision.CompareTag("Player")) return;

        yaActivado = true;
        GameManager.instancia.JuegoCompletado(); // Llamamos al método del GameManager
        SceneManager.LoadScene(escenaFinal);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (yaActivado) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        yaActivado = true;
        GameManager.instancia.JuegoCompletado();
        SceneManager.LoadScene(escenaFinal);
    }
}