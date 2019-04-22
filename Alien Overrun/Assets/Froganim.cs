using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Froganim : MonoBehaviour
{

    static Animator anim;
    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    
    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
                
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if(translation !=0)
        {
            anim.SetBool("isMoving", true);
         }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("isAttacking");
        }

    }
}
