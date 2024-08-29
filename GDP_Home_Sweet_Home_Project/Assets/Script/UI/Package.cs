using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FurnitureType 
{
    Bar_Stool,
    Dining_Table,
    Dining_Chair,
    TV_Console,
    Mirror,
    Large_Cabinet,
    Study_Table,
    Office_Chair,
    Sofa
}


public class Package : MonoBehaviour
{
    public string furnitureName;
    public FurnitureType furnitureType;
    public Sprite furniturePhoto;
    
    public bool isAssemblyRequired;
    public Sprite toolRequired;
    public Sprite comicStrip;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetFurnitureTypeAsString()
    {
        return furnitureType.ToString().Replace('_', ' ');
    }
}
