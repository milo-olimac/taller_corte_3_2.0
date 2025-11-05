using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Interfaz de Usuario")]
    public TextMeshProUGUI textoPuntaje;
    public TextMeshProUGUI textoTiempo;
    public GameObject[] corazones;

    [Header("Estadisticas Globales")]
    public int puntaje = 0;
    public int vidas = 3;
    public int colisiones = 0;
    private float tiempoTranscurrido = 0f;

    [Header("Control")]
    public bool juegoActivo = true;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        ActualizarUI();
    }

    void Update()
    {
        if (!juegoActivo) return;

        tiempoTranscurrido += Time.deltaTime;
        if (textoTiempo != null)
            textoTiempo.text = "Tiempo: " + tiempoTranscurrido.ToString("F1") + "s";
    }

    public void AgregarPuntaje(int cantidad)
    {
        puntaje += cantidad;
        ActualizarUI();

        if (puntaje >= 80 && colisiones < 3 && SceneManager.GetActiveScene().name == "Fuego")
        {
            CargarSiguienteEscena();
        }
    }

    public void PerderVida()
    {
        if (vidas <= 0) return;

        vidas--;
        colisiones++;

        if (corazones != null && vidas >= 0 && vidas < corazones.Length)
            corazones[vidas].SetActive(false);

        if (vidas == 0)
        {
            FinDelJuego();
        }
        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = "Puntos: " + puntaje;
    }

   public void CargarSiguienteEscena()
{
    string escenaActual = SceneManager.GetActiveScene().name;
    Debug.Log("Cambiando desde: " + escenaActual);

    if (escenaActual == "SceneFuego")
    {
        SceneManager.LoadScene("SceneHielo");
        Debug.Log("Cargando SceneHielo");
    }
    else if (escenaActual == "SceneHielo")
    {
        SceneManager.LoadScene("Final");
        Debug.Log("Cargando Final");
    }
}



    public void FinDelJuego()
    {
        juegoActivo = false;
        Debug.Log("Fin del juego: sin vidas restantes");
        SceneManager.LoadScene("GameOver");
    }

    public void ReiniciarJuego()
    {
        puntaje = 0;
        vidas = 3;
        colisiones = 0;
        tiempoTranscurrido = 0f;
        juegoActivo = true;
        ActualizarUI();
    }
}
