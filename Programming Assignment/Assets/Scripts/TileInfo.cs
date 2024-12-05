using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public float X { get; private set; }
    public float Z { get; private set; }

    public void SetTilePosition(float x, float z)
    {
        X = x;
        Z = z;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
