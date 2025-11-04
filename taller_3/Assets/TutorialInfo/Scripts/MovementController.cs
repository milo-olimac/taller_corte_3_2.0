using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float JumpForce = 5f;
    public float sensibility = 2f;
    public float limitCamX = 45f;
    public Transform cam;
    public int salud = 3;

    private Rigidbody rb;
    private bool grounded;

    private float rotX = 0f;

    public Transform SpawnPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Raycast
        Debug.DrawRay(transform.position, Vector3.down * 1.8f, Color.red);
        Gizmos.color = Color.red;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1.8f);

        //Mover
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime);

        //Saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb = GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                //rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }

        //Camara
        rotX += -Input.GetAxis("Mouse Y") * sensibility;
        rotX = Mathf.Clamp(rotX, -limitCamX, limitCamX);
        cam.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensibility, 0);

        Debug.DrawRay(cam.position, cam.forward * 5f, Color.blue);
        
        //Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        { speed = 12f; }
        else
        { speed = 5f; }

    }

    //Collision con objetos
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
        if (collision.gameObject.tag == "DeathZone")
        {
            transform.position = SpawnPoint.position;
            salud -= 1;
        }
        if (collision.gameObject.tag == "WinZone")
        {
            GameManager gameManager = Object.FindFirstObjectByType<GameManager>();
            gameManager.NextLevel();
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

}
