using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{

    public BulletSpawner bulletSpawner;
    public float movementSpeed = 50f;
    public float rotationSpeed = 130f;
    public NetworkVariable<Color> playerColorNetVar;

    private Camera playerCamera;
    public GameObject playerBody;

    private void Start() {
        NetworkHelper.Log(this, "Start");
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        playerCamera.enabled = IsOwner;
        playerCamera.GetComponent<AudioListener>().enabled = IsOwner;

        //playerBody = transform.Find("PlayerBody").gameObject;
        ApplyColor();
    }

    private void Update() {
        if (IsOwner)
        {
            OwnerHandleInput();
            if (Input.GetButtonDown("Fire1")){
                NetworkHelper.Log("Requesting Fire");
                FireServerRpc();
            }
        }
    }


    public override void OnNetworkSpawn()
    {
        NetworkHelper.Log(this, "OnNetworkSpawn");
        Start();
        base.OnNetworkSpawn();
    }


    private void OwnerHandleInput()
    {
        Vector3 movement = CalcMovement();
        Vector3 rotation = CalcRotation();

        if (movement != Vector3.zero || rotation != Vector3.zero) {
            MoveServerRpc(movement, rotation, NetworkManager.LocalClientId);
        }
    }

    private void ApplyColor() {
        playerBody.GetComponent<MeshRenderer>().material.color = playerColorNetVar.Value;
    }

    [ServerRpc(RequireOwnership = true)]
    private void MoveServerRpc(Vector3 movement, Vector3 rotation, ulong clientId)
    {
        transform.Translate(movement);
        transform.Rotate(rotation);

        if(NetworkManager.LocalClientId != clientId)
        {
            if (playerBody.transform.position.x < -25)
            {
                transform.position = new Vector3(-25, transform.position.y, transform.position.z);
            }
            if(playerBody.transform.position.x > 25)
            {
                transform.position = new Vector3(25, transform.position.y, transform.position.z);
            }
            if (playerBody.transform.position.z < -25)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -25);
            }
            if (playerBody.transform.position.z > 25)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 25);
            }
        }
    }


    [ServerRpc]
    private void FireServerRpc()
    {
        NetworkHelper.Log("Fire");
        bulletSpawner.Fire();
    }



    // Rotate around the y axis when shift is not pressed
    private Vector3 CalcRotation()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        Vector3 rotVect = Vector3.zero;
        if (!isShiftKeyDown)
        {
            rotVect = new Vector3(0, Input.GetAxis("Horizontal"), 0);
            rotVect *= rotationSpeed * Time.deltaTime;
        }
        return rotVect;
    }


    // Move up and back, and strafe when shift is pressed
    private Vector3 CalcMovement()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float x_move = 0.0f;
        float z_move = Input.GetAxis("Vertical");

        if (isShiftKeyDown)
        {
            x_move = Input.GetAxis("Horizontal");
        }

        Vector3 moveVect = new Vector3(x_move, 0, z_move);
        moveVect *= movementSpeed * Time.deltaTime;

        return moveVect;
    }
}

