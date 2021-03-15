using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    CharacterController m_characterController;
    [SerializeField]
    Transform arrowTransform;
    [SerializeField]
    GameObject platform;
    [SerializeField]
    GameObject screen;
    [SerializeField]
    float m_movementSpeed = 7.0f;
    [SerializeField]
    float m_fallingGravityScale = 2.5f;
    [SerializeField]
    float m_jumpGravityScale = 2.0f;
    [SerializeField]
    public bool m_player = true;

    float BOUND = 0.25f;
    float STEP = 0.02f;
    float originalY;
    float m_horizontalInput;
    float m_verticalInput;
    bool m_jump = false;
    bool m_IsInAir = false;
    bool control = true;
    float timerControl = -1.5f;
    public bool m_alive = true;
    public int oxygenCounter = 5;

    Vector3 m_movementInput;
    Vector3 m_movement;
    MeshRenderer m_renderTV;        // the material on TV

    // Computers part
    List<MeshRenderer> tilesMaterials = new List<MeshRenderer>();
    List<Vector3> tilesPosition = new List<Vector3>();
    List<int> memBuffer = new List<int>();      // memBuffer[0,1,2]: eagle, knife, glove
    List<string> nameMaterial = new List<string>();
    float timer = -1.5f;
    float updateTimer = 0.0f;
    int threRotation = 0;
    int indexTile = 0;
    int nTiles = 0;

    [SerializeField]
    List<Material> display;     // eagle, knife, glove
    
    // Audio
    AudioSource m_audioSource;

    [SerializeField]
    AudioClip m_jumpSound;
    [SerializeField]
    AudioClip gas;

    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_audioSource = GetComponent<AudioSource>();
        m_renderTV = screen.GetComponent<MeshRenderer>();
        if (m_player)
        {
            originalY = arrowTransform.position.y;
        }
        // Computers
        if (!m_player)
        {
            ComputersInit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.0f && m_alive)
        {
            Alive();
            CallControl();
        }
    }

    void CallControl()
    {
        timerControl += Time.deltaTime;
        timer += Time.deltaTime;
        if (timerControl > 15.0f && timerControl < 20.0f && control)
        {
            control = false;
            Still();
        }
        if (timerControl > 20.0f)
        {
            control = true;
            timerControl = -1.5f;
        }
        if (control)
        {
            Control();
        }
    }

    void ComputersInit()
    {
        threRotation = Random.RandomRange(0, 10);
        List<GameObject> tiles = platform.GetComponent<Platform>().tiles;
        nTiles = tiles.Count;
        for (int i = 0; i < nTiles; i++)
        {
            tilesPosition.Add(tiles[i].GetComponent<Transform>().position);
        }
        for(int i = 0; i < nTiles; i++)
        {
            tilesMaterials.Add(tiles[i].GetComponent<MeshRenderer>());
        }
        indexTile = Random.RandomRange(0, nTiles);

        // initialize the memory buffer
        for (int i = 0; i < 3; i++)
        {
            memBuffer.Add(Random.RandomRange(0, nTiles));
            nameMaterial.Add(display[i].name + " (Instance)");
        }
    }

    void Still()
    {
        m_horizontalInput = 0.0f;
        m_verticalInput = 0.0f;
        m_movement.x = m_horizontalInput * m_movementSpeed * Time.deltaTime;
        m_movement.z = m_verticalInput * m_movementSpeed * Time.deltaTime;
    }

    void Control()
    {
        // Movement input
        if (m_player)
        {
            m_horizontalInput = Input.GetAxis("Horizontal");
            m_verticalInput = Input.GetAxis("Vertical");
            if (Input.GetButtonDown("Jump"))
            {
                m_jump = true;
                m_audioSource.PlayOneShot(m_jumpSound);
            }

            RotateCharacterTowardsMouseCursor();
        }
        else
        {
            // Computers Logic
            Vector3 currentPosition = transform.position;
            ComputersUpdateIndex();

            // Rotation
            int dice = Random.RandomRange(0, threRotation * 35);
            if (dice < threRotation)
            {
                Vector3 v = new Vector3(Random.Range(-20.0f,20.0f), currentPosition.y, Random.Range(-20.0f, 20.0f));
                transform.LookAt(v);
            }

            // Jump
            dice = Random.RandomRange(0, threRotation * 60);
            if (dice < threRotation)
            {
                m_jump = true;
            }

            // Move to the indexed tile with error bound of [-0.1f,0.1f]
            m_horizontalInput = 0.0f;
            m_verticalInput = 0.0f;

            // x move
            if (tilesPosition[indexTile].x > currentPosition.x + 0.1f)
            {
                m_horizontalInput = 1.0f;
            }
            else if (tilesPosition[indexTile].x < currentPosition.x - 0.1f)
            {
                m_horizontalInput = -1.0f;
            }

            // y move
            if (tilesPosition[indexTile].z > currentPosition.z + 0.1f)
            {
                m_verticalInput = 1.0f;
            }
            else if (tilesPosition[indexTile].z < currentPosition.z - 0.1f)
            {
                m_verticalInput = -1.0f;
            }
        }
        m_movement.x = m_horizontalInput * m_movementSpeed * Time.deltaTime;
        m_movement.z = m_verticalInput * m_movementSpeed * Time.deltaTime;
    }

    bool CanJump()
    {
        return m_characterController.isGrounded;
    }

    void Alive()
    {
        if(this.gameObject.transform.position.y <= -4.0f)
        {
            m_alive = false;
            if (m_player)       // player die
            {
                Time.timeScale = 0.0f;
                m_renderTV.material = screen.GetComponent<TVScreen>().m_counter[6];
            }
        }
    }

    void ComputersUpdateIndex()
    {
        updateTimer += Time.deltaTime;
        List<int> accuracy = new List<int> {80, 85, 90};
        // shuffle memory accuracy
        for(int i = 0; i < 3; i++)
        {
            int temp = accuracy[i];
            int s = Random.RandomRange(0, 3);
            accuracy[i] = accuracy[s];
            accuracy[s] = temp;
        }

        // check all tiles and construct memory
        for (int i=0; i<nTiles; i++)
        {

            if (tilesMaterials[i].material.name == nameMaterial[0])
            {
                if (Random.RandomRange(0, 10) < 5)
                {
                    memBuffer[0] = i;
                    if (Random.RandomRange(0, 100) > accuracy[0])       // forget
                    {
                        memBuffer[0] = Random.RandomRange(0, nTiles);
                    }
                }
            }
            if (tilesMaterials[i].material.name == nameMaterial[1])
            {
                if (Random.RandomRange(0, 10) < 5)
                {
                    memBuffer[1] = i;
                    if (Random.RandomRange(0, 100) > accuracy[1])       // forget
                    {
                        memBuffer[1] = Random.RandomRange(0, nTiles);
                    }
                }
            }
            if (tilesMaterials[i].material.name == nameMaterial[2])
            {
                if (Random.RandomRange(0, 10) < 5)
                {
                    memBuffer[2] = i;
                    if (Random.RandomRange(0, 100) > accuracy[2])       // forget
                    {
                        memBuffer[2] = Random.RandomRange(0, nTiles);
                    }
                }
            }
        }
        
        if(timer < 10.0f && updateTimer>0.75f)
        {
            updateTimer = 0.0f;
            if (Random.RandomRange(0, 10) < 6)
            {
                indexTile = Random.RandomRange(0, nTiles);
            }
        }
        else if(timer >= 10.0f && timer <= 15.0f)
        {
            // If it cannot get on the tile, move to the next!
            for (int i = 0; i < 3; i++)
            {
                if (nameMaterial[i] == m_renderTV.material.name)
                {
                    indexTile = memBuffer[i];
                    break;
                }
            }
        }
        else if(timer > 20.0f)
        {
            timer = -1.5f;
        }
    }

    void RotateCharacterTowardsMouseCursor()
    {
        Vector3 mousePosInScreenSpace = Input.mousePosition;
        Vector3 playerPosInScreenSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 directionInScreenSpace = mousePosInScreenSpace - playerPosInScreenSpace;
        FloatArrow();
        float angle = Mathf.Atan2(directionInScreenSpace.y, directionInScreenSpace.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle + 90.0f, Vector3.up);
        arrowTransform.rotation = Quaternion.AngleAxis(0.0f, Vector3.up);
    }

    void FloatArrow()
    {
        arrowTransform.position = new Vector3(arrowTransform.position.x, arrowTransform.position.y + STEP, arrowTransform.position.z);
        if (arrowTransform.position.y < originalY - BOUND || arrowTransform.position.y > originalY + BOUND)
        {
            STEP *= -1.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Oxygen")
        {
            if (m_player)
            {
                m_audioSource.PlayOneShot(gas);
            }
            Destroy(other.gameObject);

            oxygenCounter -= 1;
        }
    }

    void UpdatePlayerVelocity()
    {
        // Horizontal Movement (X Axis - Left / Right)
        if (m_characterController.isGrounded)
        {
            if (m_jump)
            {
                m_movement.y = 0.3f;
            }
            else
            {
                m_movement.y = 0.0f;
            }
        }

        if (m_characterController.velocity.y < 0)
        {
            m_movement.y -= 0.5f * m_fallingGravityScale * Time.deltaTime;
        }
        else if (m_characterController.velocity.y >= 0 && !m_jump)
        {
            m_movement.y -= 0.5f * m_jumpGravityScale * Time.deltaTime;
        }

        // Apply Movement Input
        m_characterController.Move(m_movement);

        m_jump = false;
        m_IsInAir = !m_characterController.isGrounded;
    }

    void FixedUpdate()
    {
        if(Time.timeScale > 0.0f)
        {
            UpdatePlayerVelocity();
        }
    }
}
