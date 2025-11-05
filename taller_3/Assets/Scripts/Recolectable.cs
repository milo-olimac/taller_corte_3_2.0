using UnityEngine;

public class Recolectable : MonoBehaviour
{
    public int valor = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instancia.AgregarPuntaje(valor);
            Destroy(gameObject);
        }
    }
}
