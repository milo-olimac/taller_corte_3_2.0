using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Interfaz de Usuario")]
    public TextMeshProUGUI textoPuntaje;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoColisiones;
    public GameObject[] corazones;

    [Header("Pantalla de Mensaje")]
    public GameObject panelNegro;
    public TextMeshProUGUI textoMensaje;

    [Header("Estadisticas Globales")]
    public int puntaje = 0;
    public int vidas = 3;
    public int colisiones = 0;
    private float tiempoTranscurrido = 0f;

    [Header("Limites")]
    public int maxColisionesAntesReinicio = 3;

    [Header("Control")]
    public bool juegoActivo = true;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
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

        // 🔥 AQUÍ ESTÁ LA MAGIA: Comprobación automática de nivel completado
        ComprobarCondicionesNivel();
    }

    private void ComprobarCondicionesNivel()
    {
        string escenaActual = SceneManager.GetActiveScene().name;

        // Solo en SceneFuego queremos pasar a SceneHielo
        if (escenaActual == "SceneFuego")
        {
            if (puntaje >= 80 && colisiones < 1 && juegoActivo)
            {
                juegoActivo = false; // Evita que se llame varias veces
                StartCoroutine(PasarAlSiguienteNivel("SceneHielo", "¡Nivel completado!\nPasando al mundo de hielo..."));
            }
        }
        // En SceneHielo pasamos al Final
        else if (escenaActual == "SceneHielo")
        {
            if (puntaje >= 100 && colisiones < 3 && juegoActivo) // Cambia 100 por los puntos que quieras
            {
                juegoActivo = false;
                StartCoroutine(PasarAlSiguienteNivel("Final", "¡Genial! Has completado el juego."));
            }
        }
    }

    private IEnumerator PasarAlSiguienteNivel(string siguienteEscena, string mensaje)
    {
        Time.timeScale = 0f;
        if (panelNegro != null) panelNegro.SetActive(true);
        if (textoMensaje != null)
        {
            textoMensaje.gameObject.SetActive(true);
            textoMensaje.text = mensaje;
        }

        yield return new WaitForSecondsRealtime(3f); // 3 segundos de gloria

        Time.timeScale = 1f;
        SceneManager.LoadScene(siguienteEscena);
    }

    public void AgregarPuntaje(int cantidad)
    {
        puntaje += cantidad;
        ActualizarUI();
    }

    public void PerderVida()
    {
        colisiones++;

        if (colisiones >= 3)
        {
            ReiniciarNivel();
            return;
        }

        if (vidas > 0)
        {
            vidas--;

            // Cambia el color del corazon perdido
            if (corazones != null && vidas >= 0 && vidas < corazones.Length)
            {
                Image imagen = corazones[vidas].GetComponent<Image>();
                if (imagen != null)
                {
                    imagen.color = Color.black; // cambia a negro los ♥
                }
            }
        }

        ActualizarUI();
    }



    void ActualizarUI()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = "Puntos: " + puntaje;

        if (textoColisiones != null)
            textoColisiones.text = "Colisiones: " + colisiones;
    }

    public void CargarSiguienteEscena()
    {
        string escenaActual = SceneManager.GetActiveScene().name;

        if (escenaActual == "SceneFuego")
        {
            if (puntaje >= 80 && colisiones < 3)
            {
                SceneManager.LoadScene("SceneHielo");
            }
            else
            {
                StartCoroutine(MostrarMensajeYPausar("Necesitas al menos 80 puntos y menos de 3 colisiones", 4f, escenaActual));
            }
        }
        else if (escenaActual == "SceneHielo")
        {
            SceneManager.LoadScene("Final");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reasignar textos
        textoPuntaje = GameObject.FindWithTag("TextoPuntaje")?.GetComponent<TextMeshProUGUI>();
        textoTiempo = GameObject.FindWithTag("TextoTiempo")?.GetComponent<TextMeshProUGUI>();
        textoColisiones = GameObject.FindWithTag("TextoColisiones")?.GetComponent<TextMeshProUGUI>();
        textoMensaje = GameObject.FindWithTag("TextoMensaje")?.GetComponent<TextMeshProUGUI>();

        // Reasignar panel negro
        GameObject panel = GameObject.FindWithTag("PanelNegro");
        if (panel != null)
            panelNegro = panel;

        // Busca los nuevos ♥ en la escena
        GameObject[] encontrados = GameObject.FindGameObjectsWithTag("Corazon");
        if (encontrados.Length > 0)
        {
            corazones = encontrados;

            // Restaurar el color de todos los ♥
            foreach (GameObject c in corazones)
            {
                if (c != null)
                {
                    Image img = c.GetComponent<Image>();
                    if (img != null)
                        img.color = Color.white;
                }
            }
        }

        ActualizarUI();
        juegoActivo = true;
        Time.timeScale = 1f;
    }



    private IEnumerator MostrarMensajeYPausar(string mensaje, float duracion, string escenaReiniciar)
    {
        juegoActivo = false;
        Time.timeScale = 0f; // pausa todo para el mensajito

        if (panelNegro != null) panelNegro.SetActive(true);
        if (textoMensaje != null)
        {
            textoMensaje.gameObject.SetActive(true);
            textoMensaje.text = mensaje;
        }

        float tiempoReal = 0f;
        while (tiempoReal < duracion)
        {
            tiempoReal += Time.unscaledDeltaTime; // usa tiempo real, este no se ve afectado por la pausa
            yield return null;
        }

        Time.timeScale = 1f; // reanuda la esceneeee
        SceneManager.LoadScene(escenaReiniciar);
        juegoActivo = true;
        Time.timeScale = 1f;
    }

    public void FinDelJuego()
    {
        juegoActivo = false;
        SceneManager.LoadScene("GameOver");
    }

    public void ReiniciarNivel()
    {
        puntaje = 0;
        vidas = 3;
        colisiones = 0;
        tiempoTranscurrido = 0f;

        ActualizarUI();
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name);

        if (corazones != null)
        {
            foreach (GameObject corazon in corazones)
            {
                if (corazon != null)
                {
                    Image img = corazon.GetComponent<Image>();
                    if (img != null)
                    {
                        img.color = Color.white; // restaura el color del ♥
                    }
                }
            }
        }

    }

public void JuegoCompletado()
{
    juegoActivo = false;

    // Guardamos los datos finales para la pantalla final
    PlayerPrefs.SetFloat("TiempoTotal", tiempoTranscurrido);
    PlayerPrefs.SetInt("PuntajeFinal", puntaje);
    PlayerPrefs.Save();

    Debug.Log($"Juego completado! Tiempo: {tiempoTranscurrido:F1}s | Puntos: {puntaje}");
}

}
