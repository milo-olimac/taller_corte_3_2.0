using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaFinalUI : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoPuntos;
    public Canvas canvas;

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("NO HAY CANVAS EN LA ESCENA PANTALLA FINAL");
            return;
        }

        // INTENTO 1: Buscar por nombre exacto
        textoTiempo = GameObject.Find("Texto_Tiempo")?.GetComponent<TextMeshProUGUI>();
        textoPuntos = GameObject.Find("Texto_Puntos")?.GetComponent<TextMeshProUGUI>();

        // INTENTO 2: Si no, buscar por tag (si usaste tags)
        if (textoTiempo == null) textoTiempo = GameObject.FindWithTag("TextoTiempoFinal")?.GetComponent<TextMeshProUGUI>();
        if (textoPuntos == null) textoPuntos = GameObject.FindWithTag("TextoPuntosFinal")?.GetComponent<TextMeshProUGUI>();

        // INTENTO 3: Si no, buscar por componente en Canvas
        if (textoTiempo == null || textoPuntos == null)
        {
            var textos = canvas.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in textos)
            {
                if (t.gameObject.name.Contains("Tiempo") && textoTiempo == null)
                    textoTiempo = t;
                if (t.gameObject.name.Contains("Puntos") && textoPuntos == null)
                    textoPuntos = t;
            }
        }

        // SI AÚN NO HAY TEXTOS → LOS CREAMOS
        if (textoTiempo == null) textoTiempo = CrearTexto("Texto_Tiempo", new Vector2(0, 100));
        if (textoPuntos == null) textoPuntos = CrearTexto("Texto_Puntos", new Vector2(0, 0));
    }

    void Start()
    {
        MostrarEstadisticas();
    }

    void MostrarEstadisticas()
    {
        float tiempo = PlayerPrefs.GetFloat("TiempoTotal", 0f);
        int puntos = PlayerPrefs.GetInt("PuntajeFinal", 0);

        Debug.Log($"[PANTALLA FINAL] Tiempo: {tiempo:F1}s | Puntos: {puntos}");

        textoTiempo.text = $"Tiempo Total: {tiempo:F1} s";
        textoPuntos.text = $"Puntos: {puntos}";
    }

    TextMeshProUGUI CrearTexto(string nombre, Vector2 posicion)
    {
        GameObject go = new GameObject(nombre);
        go.transform.SetParent(canvas.transform);
        go.layer = canvas.gameObject.layer;

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = nombre;
        tmp.fontSize = 50;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        RectTransform rt = tmp.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = posicion;
        rt.sizeDelta = new Vector2(600, 100);

        Debug.Log($"[CREADO] {nombre} en posición {posicion}");
        return tmp;
    }
}