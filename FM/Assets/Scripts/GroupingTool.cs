using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FMPrefabList))]
public class GroupingTool : MonoBehaviour
{
    #region Fields
    FMPrefabList prefabList;

    List<CopyInfo> copiedObjects = new List<CopyInfo>();
    Vector3 centerPoint = Vector3.positiveInfinity;
    #endregion

    public void Start()
    {
        prefabList = GetComponent<FMPrefabList>();
    }

    #region functionality

    class HighAndLow
    {
        #region High
        public float High
        {
            get
            {
                return high;
            }

            set
            {
                high = Mathf.Max(value, high);
            }
        }
        float high = float.NegativeInfinity;
        #endregion

        #region Low
        public float Low {
            get
            {
                return low;
            }

            set
            {
                low = Mathf.Min(value, low);
            }
        }
        float low = float.PositiveInfinity;
        #endregion

        public float Center
        {
            get
            {
                return high - (low - high) / 2;
            }
        }

        #region Constructor
        public HighAndLow() { }

        public HighAndLow(float f)
        {
            Set(f);
        }
        #endregion

        public void Set(float value)
        {
            High = value;
            Low = value;
        }

        public static implicit operator float(HighAndLow hal) => hal.Center;
        //public static implicit operator HighAndLow(float f) => new HighAndLow(f);
        //public static HighAndLow operator =(HighAndLow hal, float f) { return hal.Set(f)};
    }

    public void Copy()
    {
        copiedObjects.Clear(); //forget about the old copied stuff

        //variables for center point calculation
        HighAndLow X = new HighAndLow();
        HighAndLow Y = new HighAndLow();
        HighAndLow Z = new HighAndLow();

        foreach(Selector selector in FindObjectsOfType<Selector>())
        {
            if (selector.IsSelected)
            {
                //save copied objects info
                FMPrefab prefabType = prefabList.GetPrefabType(selector.gameObject);
                XML details = prefabType.ConvertToXML(selector.gameObject);
                CopyInfo copiedObject = new CopyInfo(prefabType, details);

                copiedObjects.Add(copiedObject);

                //center point variable update
                Vector3 position = selector.transform.position;
                X.Set(position.x); //todo all = function instead of set
                Y.Set(position.y);
                Z.Set(position.z);
            }
        }

        Vector3 centerPoint = new Vector3(X, Y, Z);

        foreach(CopyInfo copiedInfo in copiedObjects)
        {
            copiedInfo.CenterPoint = centerPoint;
        }
    }

    public void Paste(Vector3 centerPoint)
    {
        foreach(CopyInfo copyInfo in copiedObjects)
        {
            copyInfo.Instanciate(centerPoint);
        }
    }
    #endregion
}

//todo move to it's own file
public class CopyInfo
{
    #region fields
    XML details;
    FMPrefab originalPrefab;
    Vector3 offset;
    #endregion

    public Vector3 CenterPoint{
        set
        {
            offset = value - FMPrefab.ConvertToVector3(details.attributes["Position"]);
        }
    }

    public CopyInfo(FMPrefab originalPrefab, XML details)
    {
        this.originalPrefab = originalPrefab;
        this.details = details;
    }

    public GameObject Instanciate(Vector3 centerPoint)
    {
        GameObject newObject = originalPrefab.InstanciatePrefab(details);
        newObject.transform.position = centerPoint - offset;

        return newObject;
    }
}