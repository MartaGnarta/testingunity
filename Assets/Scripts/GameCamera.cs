using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform gameManager;
    public float speed = 1.0f;


    GameManager gameManagerC;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerC = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManagerC.IsShowingDialog()) { return; }

        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
