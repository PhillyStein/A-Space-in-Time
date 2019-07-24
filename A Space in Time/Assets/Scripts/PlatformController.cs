using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformController : MonoBehaviour
{
    
    public GameObject[] platformArray;

    private bool canRespawn;

    private float[] ranArray = {0.75f, 1, 1.25f};

    // Start is called before the first frame update
    void Start()
    {
        canRespawn = true;
        if (gameObject != null)
        {
            gameObject.transform.parent = GameManager.instance.groundGroup.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x < -19 && canRespawn)
        {
            int ran = Random.Range(0, 2);
            GameObject newPlatform = Instantiate(platformArray[ran]) as GameObject;
            //newPlatform.transform.parent = groundGroup.transform;
            int ran2 = Random.Range(0, 2);
            float ranY = ranArray[ran2];
            newPlatform.transform.localScale = new Vector2(newPlatform.transform.localScale.x, ranY);
            float newY;

            if(ranY < 1)
            {
                newY = -3.5f;
            } else if(ranY > 1){
                newY = -2.4f;
            } else {
                newY = -3;
            }

            newPlatform.transform.position = new Vector2(35.5f, newY);
            canRespawn = false;
            DestroyObject(gameObject);
        }
    }
}
