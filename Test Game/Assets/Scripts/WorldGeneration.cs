using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldGeneration : MonoBehaviour
{
    private List<List<GameObject>> gameGeneration = new List<List<GameObject>>();
    private List<List<GameObject>> subGameGeneration = new List<List<GameObject>>();

    public List<GameObject> tutorial = new List<GameObject>();
    public List<GameObject> worldOne = new List<GameObject>();

    public List<GameObject> subWorldOne = new List<GameObject>();

    public List<GameObject> world = new List<GameObject>();
    public GameObject startingPlatform;
    public Camera playerCamera;

    public int progression = 0;
    public int currentMilestone = 0;
    private int[] milestones = { 1 };

    public PlayerMovement player;

    private void Start()
    {
        gameGeneration.Add(tutorial);
        gameGeneration.Add(worldOne);

        subGameGeneration.Add(subWorldOne);

        GameObject start = Instantiate(GenerateWorld(0), startingPlatform.GetComponent<Section>().end.transform);
        world.Add(start);
    }

    private void FixedUpdate()
    {
        if (world[0].GetComponent<Section>().end.transform.position.z < playerCamera.transform.position.z)
        {
            progression++;

            if (currentMilestone < milestones.Length && progression >= milestones[currentMilestone])
            {
                progression = 0;
                currentMilestone++;
            }

            if (Random.Range(0, 2) < 1 && currentMilestone != 0)
            {
                GameObject newPlatform = Instantiate(GenerateWorld(currentMilestone), world[world.Count - 1].GetComponent<Section>().end.transform);
                world.Add(newPlatform);

                world.RemoveAt(0);
            }
            else
            {
                int randomOutcome = Random.Range(0, 10);
                
                for(int i = 0; i < randomOutcome; i += 2)
                {
                    GameObject newPlatform = Instantiate(GenerateSubWorld(currentMilestone - 1), world[world.Count - 1].GetComponent<Section>().end.transform);
                    world.Add(newPlatform);

                    world.RemoveAt(0);
                }
            }
        }

        //Debug.Log(playerCamera.transform.position.y + ", " + (world[0].GetComponent<Section>().end.transform.position.y - 10));

        if (playerCamera.transform.position.y < world[0].GetComponent<Section>().end.transform.position.y - 10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private GameObject GenerateWorld(int index)
    {
        return gameGeneration[index][Random.Range(0, gameGeneration[index].Count)];
    }
    private GameObject GenerateSubWorld(int index)
    {
        return subGameGeneration[index][Random.Range(0, subGameGeneration[index].Count)];
    }
}
