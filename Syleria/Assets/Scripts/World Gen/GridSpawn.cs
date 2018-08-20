using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpawn : MonoBehaviour {

    public GameObject grid;
    public int numberOfRooms;
    public int arraySize;
    public int roomSize;
    [Range(0, 100)]
    public int chanceReturnToCenter;

    private int counter;
    private int selection;
    private int xCounter;

    private Vector3 cameraGoal;
    public float cameraLerp;
    public float sizeMulti;
    public int slowMo;

    public InputField arraySizeField;
    public InputField roomsNumberField;
    public Slider centreResetSlider;
    public Slider zoomSlider;
    public Slider slowMoSlider;

    private List<GameObject> grids = new List<GameObject>();

    // Use this for initialization
    void Start () {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        counter = 0;
        selection = 0;
        xCounter = 0;

        roomsNumberField.text = numberOfRooms.ToString();
        arraySizeField.text = arraySize.ToString();
        centreResetSlider.value = chanceReturnToCenter;
        zoomSlider.value = sizeMulti;
        slowMoSlider.value = slowMo;

    }
	
	// Update is called once per frame
	void Update () {

        numberOfRooms = int.Parse(roomsNumberField.text);
        arraySize = int.Parse(arraySizeField.text);
        chanceReturnToCenter = (int)centreResetSlider.value;
        sizeMulti = zoomSlider.value;
        slowMo = (int)slowMoSlider.value;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            xCounter += arraySize;
            GameObject GO = Instantiate(grid, new Vector3(xCounter*roomSize, 0, 0), Quaternion.identity) as GameObject;
            grids.Add(GO);
            FloorGen floorGen = GO.GetComponent<FloorGen>();
            floorGen.numberOfRooms = numberOfRooms;
            floorGen.arraySize = arraySize;
            floorGen.roomSize = roomSize;
            floorGen.chanceReturnToCenter = chanceReturnToCenter;
            floorGen.slowMo = slowMo;
            counter++;
            cameraGoal = GO.transform.position;
            selection = counter-1;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.A))
        {
            if (selection > 0)
            {
                selection--;
                cameraGoal = grids[selection].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (selection < grids.Count-1)
            {
                selection++;
                cameraGoal = grids[selection].transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (sizeMulti > zoomSlider.minValue)
            {
                sizeMulti--;
                if (sizeMulti < zoomSlider.minValue)
                {
                    sizeMulti = zoomSlider.minValue;
                }
                zoomSlider.value = sizeMulti;
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (sizeMulti < zoomSlider.maxValue)
            {
                sizeMulti++;
                if (sizeMulti > zoomSlider.maxValue)
                {
                    sizeMulti = zoomSlider.maxValue;
                }
                zoomSlider.value = sizeMulti;
            }
        }

        foreach (var grid in grids)
        {
            grid.GetComponent<FloorGen>().slowMo = slowMo;
        }

        cameraGoal.z = -10;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraGoal, cameraLerp);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, grids[selection].GetComponent<FloorGen>().arraySize * sizeMulti, cameraLerp);
	}
}
