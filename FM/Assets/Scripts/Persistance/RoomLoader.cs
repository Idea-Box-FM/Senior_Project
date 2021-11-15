using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
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
    }

    #region Fields
    [SerializeField] Walls walls;
    [SerializeField] GameObject roof;
    [SerializeField] GameObject floor;
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
        roof.transform.localScale = new Vector3(Width, roof.transform.localScale.y, Length);
    }

    private void SetFloor()
    {
        Vector3 position = floor.transform.position;
        floor.transform.position = new Vector3(position.x, 0, position.z);
        floor.transform.localScale = new Vector3(Width, floor.transform.localScale.y, Length) / 10;

    }
    #endregion
}
