using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

public class playerMoviment : NetworkBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Transform myCameraTransform;

    private float yVelocity;
    private bool inGround;

    [SerializeField] private Transform foot;
    [SerializeField] private LayerMask layerColision;
    [SerializeField] private Camera myCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myCameraTransform = myCamera.transform;

        if (!isLocalPlayer)
        {
            myCamera.gameObject.SetActive(false);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return; // Apenas o jogador local controla esse script

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moviment = new Vector3(horizontal, 0, vertical);
        moviment = myCameraTransform.TransformDirection(moviment);
        moviment.y = 0;

        controller.Move(moviment * Time.deltaTime * 4);

        if(moviment != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moviment), Time.deltaTime * 15);

        }

        animator.SetBool("isMoving", moviment != Vector3.zero);
        inGround = Physics.CheckSphere(foot.position, 0.2f, layerColision);

        animator.SetBool("inGround", inGround);

        if (Input.GetKeyDown(KeyCode.Space) && inGround)
        {
            yVelocity = 8;
            animator.SetTrigger("jump");
        }

        if(yVelocity > -8.1f)
        {
            yVelocity -= 8.1f * Time.deltaTime;
        }

        controller.Move(new Vector3(0, yVelocity, 0) * Time.deltaTime);
    }
}
