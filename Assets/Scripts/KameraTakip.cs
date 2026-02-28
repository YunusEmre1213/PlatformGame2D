using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    public Transform hedef; 

    public float minY, maxY;


    private void Update()
    {
        transform.position = new Vector3(hedef.position.x,
            Mathf.Clamp(hedef.position.y,minY,maxY),transform.position.z);
    }
}