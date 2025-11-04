using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int puntaje = 3;
    public Text textoPuntaje;

    public static int sceneCount = 0;
    public void Start()
    {
        ActualizarUI();
    }

    public void RestarPuntos(int cantidad)
    {
        puntaje -= cantidad;
        if (puntaje <= 0 )
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void ActualizarUI()
    {
        textoPuntaje.text = "Puntaje: " + puntaje.ToString();
        Debug.Log("Puntaje actualizado: " + puntaje);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(sceneCount +=1);
    }
    
}
