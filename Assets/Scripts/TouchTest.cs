using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TouchTest : MonoBehaviour
{
    public GameObject touchPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("NFT"))
                {
                    Debug.Log("Touched: " + hit.transform.name);
                    GameObject touch = Instantiate(touchPrefab, hit.point, Quaternion.identity);
                    touch.transform.SetParent(hit.transform);
                    Destroy(touch, 15f); 
                }
                else
                {
                    Debug.Log("Touched: " + hit.transform.name + " but it's not a TouchObject.");
                }

                if (hit.transform.CompareTag("Map"))
                {
                    Destroy(hit.transform.gameObject); // Destroy the touched object
                // Destroy the touch object after 2 seconds
            }
        }
    }
    }
}
