using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {


    public float speed;

    private int counter;

    private Rigidbody rb;

    public Text pointsText;

    public Text winText;

    public ParticleSystem smoke_particles;

    public Slider chargeSlider;

    public float chargePercent;
    private float minChargePercent = 0.3f;

    private int totalCollectables = 0;


	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        counter = 0;
        setPointsCounter();
        winText.text = "";
        chargePercent = minChargePercent;
        chargeSlider.value = 0;
        Cursor.visible = false;

        totalCollectables = GameObject.FindGameObjectsWithTag("Collectable").Length;
    }
	
	// Update is called once per frame
	void Update () {

        if(transform.position.y <= -2)
        {
            Die();
        }

        chargeSlider.value = (chargePercent - minChargePercent) / (1 - minChargePercent);
         
        if (Physics.Raycast(transform.position + Vector3.down * 0.4f, Vector3.down, 0.2f))
        { 
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * 500 * rb.mass);

            }


            smoke_particles.transform.position = transform.position + Vector3.down * 0.5f;
            //smoke_particles.transform.LookAt(-rb.velocity);

            smoke_particles.transform.forward = -GetForwardDirection();


            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                smoke_particles.Play();
            }

            if (Input.GetKey(KeyCode.LeftShift)){
                rb.velocity *= 0.2f;
                chargePercent += Time.deltaTime / 3;
                if(chargePercent > 1)
                {
                    chargePercent = 1;
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                
                rb.AddForce(GetForwardDirection() * 2500 * rb.mass * chargePercent);
                chargePercent = minChargePercent;
                smoke_particles.Stop();
            }
        }

    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if(x != 0 || y != 0)
        {
            Vector3 direction = new Vector3(x, 0, y).normalized;
            float angle = Mathf.Atan2(direction.z , direction.x) * Mathf.Rad2Deg - 90;
             
            Vector3 forward = GetForwardDirection();

            direction = Quaternion.Euler(0, -angle, 0) * forward;

            rb.AddForce(direction * speed * Time.fixedDeltaTime);
            //rb.angularVelocity = Vector3.Cross(direction, Vector3.down) * 100;
        }
         

        
    }

    private Vector3 GetForwardDirection()
    {
        Vector3 forward = transform.position - Camera.main.transform.position;
        forward.y = 0;
        forward.Normalize();
        return forward;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable")){
            counter += 1;
            other.gameObject.SetActive(false);
            setPointsCounter();
            if(counter == totalCollectables)
            {
                winText.text = "You Win!!!";
            }
        }
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Spike"))
        {
            Die();
        }
    }

    private void setPointsCounter()
    {
        pointsText.text = "Points: " + counter.ToString();
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }


}
