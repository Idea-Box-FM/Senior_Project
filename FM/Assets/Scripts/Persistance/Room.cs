using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Programmer Pat Naatz
 * Intention to make a script capable of saving and loading the room size to and from and xml tag
 * 
 * Made the room size save and load 11/15/2021
 * Made the materials save and load 11/15/2021
 * Added automatic grate generation to match room size 12/7/2021
 */

public class Room : MonoBehaviour
{
    [System.Serializable] struct Walls
    {
        [SerializeField] GameObject[] walls; //In NEWS Orintation

        enum Direction //NEWS
        {
            North,
            East,
            West,
            South
        }

        #region Properties
        public GameObject North
        {
            get
            {
                return walls[(int)Direction.North];
            }

            set
            {
                walls[(int)Direction.North] = value;
            }
        }

        public GameObject East
        {
            get
            {
                return walls[(int)Direction.East];
            }

            set
            {
                walls[(int)Direction.East] = value;
            }
        }

        public GameObject West
        {
            get
            {
                return walls[(int)Direction.West];
            }

            set
            {
                walls[(int)Direction.West] = value;
            }
        }

        public GameObject South
        {
            get
            {
                return walls[(int)Direction.South];
            }

            set
            {
                walls[(int)Direction.South] = value;
            }
        }
        #endregion

        public void SetMaterial(Material mat)
        {
            foreach(GameObject wall in walls)
            {
                wall.GetComponent<MeshRenderer>().materials[0] = mat;
            }
        }
    }

    #region Fields
    [SerializeField] Walls walls;
    public GameObject roof;
    public GameObject floor;

    [Tooltip("Add refernces to grate in scene")]
    [SerializeField] GameObject grate;

    [Tooltip("How many feet across one wall corresponds to one row set of grates")]
    [Range(1,10)][SerializeField] int gratePerSquareFoot;
    #endregion

    #region Properties
    /// <summary>
    /// Width, Height, Length
    /// </summary>
    public Vector3 RoomSize
    {
        get
        {
            return roomSize;
        }

        set
        {
            roomSize = value;

            SetWalls();

            SetRoof();

            SetFloor();
        }
    }

    Vector3 roomSize;

    float Width
    {
        get
        {
            return roomSize.x;
        }
    }

    float Height
    {
        get
        {
            return roomSize.y;
        }
    }

    float Length
    {
        get
        {
            return roomSize.z;
        }
    }
    #endregion

    #region Setters
    #region Wall Setters
    private void SetWalls()
    {
        SetWallScales();

        SetWallPositions();
    }

    private void SetWallScales()
    {
        SetWallScale(walls.North, Width);
        SetWallScale(walls.South, Width);

        SetWallScale(walls.East, Length);
        SetWallScale(walls.West, Length);
    }

    void SetWallScale(GameObject wall, float width, float? height = null)
    {
        float zScale = wall.transform.localScale.z;
        wall.transform.localScale = new Vector3(width, height ?? Height, zScale);
    }

    private void SetWallPositions()
    {
        walls.North.transform.position = new Vector3(0, Height / 2, Length / 2);
        walls.South.transform.position = new Vector3(0, Height / 2, -Length / 2);

        walls.East.transform.position = new Vector3(-Width / 2, Height / 2, 0);
        walls.West.transform.position = new Vector3(Width / 2, Height / 2, 0);
    }
    #endregion

    private void SetRoof()
    {
        Vector3 position = roof.transform.position;
        roof.transform.position = new Vector3(position.x, Height, position.z);
        roof.transform.localScale = new Vector3(Width, roof.transform.localScale.y * 10, Length) / 10;
    }

    private void SetFloor()
    {
        Vector3 position = floor.transform.position;
        floor.transform.position = new Vector3(position.x, 0, position.z);
        floor.transform.localScale = new Vector3(Width, floor.transform.localScale.y, Length) / 10;

        //grate logic
        grate.transform.position = new Vector3(position.x, 0, position.z);

        int maxGratesHorizontally = (int)(10 * floor.transform.localScale.x);
        int maxGratesVertically = (int)(10 * floor.transform.localScale.z);

        int gratesNeededHorizontally = (int)(gratePerSquareFoot * floor.transform.localScale.z);
        int gratesNeededVertically = (int)(gratePerSquareFoot * floor.transform.localScale.x);

        Vector3 grateScale = grate.transform.localScale;

        for (int i = 0; i < gratesNeededHorizontally; i++)
            for (int j = 0; j < maxGratesVertically; j++)
            {
                GameObject newGrate = Instantiate(grate);
                newGrate.transform.position += new Vector3(i, 0, j) - new Vector3(gratesNeededHorizontally / 2, 0, maxGratesVertically / 2);
            }

        for (int i = 0; i < gratesNeededVertically; i++)
            for (int j = 0; j < maxGratesHorizontally; j++)
            {
                GameObject newGrate = Instantiate(grate);
                newGrate.transform.position += new Vector3(j, 0, i) - new Vector3(maxGratesHorizontally / 2, 0, gratesNeededVertically / 2);
            }
    }
    #endregion

    #region Persistance
    public void ConvertToXML(ref XML xml)
    {
        XML roomXML = xml.AddChild("Room");
        roomXML.AddAttribute("RoomSize", RoomSize.ToString());

        XML Walls = roomXML.AddChild("Walls");
        Walls.AddAttribute("Material", walls.North.GetComponent<MeshRenderer>().materials[0].name);
        
        XML Roof = roomXML.AddChild("Roof");
        Roof.AddAttribute("Material", roof.GetComponent<MeshRenderer>().materials[0].name);
        
        XML Floor = roomXML.AddChild("Floor");
        Floor.AddAttribute("Material", floor.GetComponent<MeshRenderer>().materials[0].name);
    }

    public void LoadFromXML(XML xml)
    {
        XML roomXML = xml.FindChild("Room");
        RoomSize = FMPrefab.ConvertToVector3(roomXML.attributes["RoomSize"]);

        XML wallsXML = roomXML.FindChild("Walls");

        string materialName = wallsXML.attributes["Material"];
        Material material = (Material)Resources.Load(materialName, typeof(Material));
        if (material != null)
        {
            walls.SetMaterial(material);
        }

        XML roofXML = roomXML.FindChild("Roof");
        materialName = roofXML.attributes["Material"];
        material = (Material)Resources.Load(materialName, typeof(Material));
        if (material != null)
        {
            roof.GetComponent<MeshRenderer>().materials[0] = material;
        }

        XML floorXML = roomXML.FindChild("Floor");
        materialName = floorXML.attributes["Material"];
        material = (Material)Resources.Load(materialName, typeof(Material));

        if (material != null)
        {
            floor.GetComponent<MeshRenderer>().materials[0] = material;
        }
    }
    #endregion
}
