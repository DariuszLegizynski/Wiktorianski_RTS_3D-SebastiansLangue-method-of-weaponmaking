using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(WeaponControllerNew))]
public class PlayerController : CharacterStats
{
    public Camera mainCam;
    Rigidbody rb;

    [Header("Movement")]
    public float movementSpeed = 5f;                 //movement speed in units per second

    WeaponControllerNew weaponController;
    //PlayerStats playerStats;

    protected override void Start()
    {
        base.Start();
        mainCam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();

        weaponController = GetComponent<WeaponControllerNew>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        Reload();
        Shoot();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * movementSpeed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    void LookAtMouse()
    {
        Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition); //creates a ray that goes from the camera to he point, where the mouse currently is
        //A plane is added to specify, where the ray is crossing the plane (an abstract, mathematical plane rather then a gameObject plane
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        //float rayLength;

        if(groundPlane.Raycast(cameraRay, out float rayLength)) //check, if the ray from the camera hits something else in the world (in this case the groundPlane)
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            weaponController.GunFire();
        }
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponController.Reload();
        }
    }
}
