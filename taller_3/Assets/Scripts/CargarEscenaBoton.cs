using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargarEscenaBoton : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string nombreEscena = "NombreDeTuEscena"; // Cambia esto por el nombre exacto de tu escena

    private Button boton;

    void Start()
    {
        // Obtiene el componente Button del objeto al que está adjunto este script
        boton = GetComponent<Button>();

        // Asegura que el botón tenga un listener
        if (boton != null)
        {
            boton.onClick.AddListener(CargarEscena);
        }
        else
        {
            Debug.LogError("Este script debe estar en un GameObject con componente Button.");
        }
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}