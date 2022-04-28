using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Manager manager;             //Game Manager
    public Material mat;                //Colour material
    public Color defaultColor;
    public GameObject spawnedTower;     //The tower object that we spawned on this node
    public GameObject ghostTower;       //The ghost that appears when hovering over the node.
    MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        //Get the material on the object to change its colour.
        mat = GetComponent<Renderer>().material;

        defaultColor = mat.color;

        //Find the manager to know what tower to spawn.
        manager = FindObjectOfType<Manager>();

        meshFilter = ghostTower.GetComponent<MeshFilter>();

        meshFilter.mesh = null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        /// If it doesn't have a tower already...
        /// Instantiate a gameobject on top of the node.
        /// The object will need to have a mesh renderer
        /// That mesh will match the currently selected tower
        /// The mesh will have a material that is transparent.

        meshFilter.sharedMesh = manager.towerPrefab.GetComponent<MeshFilter>().sharedMesh;

        if (Input.GetMouseButtonDown(0))
        {
            if(spawnedTower != null)        //This means we have a tower already
            {
                return;                     //...skip to the end
            }

            if(manager.BuySomething(manager.towerData.price) == false)
            {
                //if the price is too much for us, skip to the end.
                return;
            }

            //Debug.Log("I have been clicked");

            //Spawn the tower
            GameObject newObject = Instantiate(manager.towerPrefab);

            //Keep track of the spawned tower by placing it in the variable.
            spawnedTower = newObject;

            //Set the colour of the towers material to match the towerdata value
            spawnedTower.GetComponent<Renderer>().material.color = manager.towerData.towerColour;

            //Move it to the node.
            newObject.transform.position = transform.position;

            //Get the tower component.
            Tower tower = newObject.GetComponent<Tower>();

            //Give it all of its stats.
            tower.damage = manager.towerData.damage;
            tower.range = manager.towerData.range;
            tower.fireRate = manager.towerData.fireRate;
        }
    }

    private void OnMouseExit()
    {
        mat.color = defaultColor;
        meshFilter.mesh = null;
    }

}
