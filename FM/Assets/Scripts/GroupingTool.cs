using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FMPrefabList))]
public class GroupingTool : MonoBehaviour
{
    #region Fields
    FMPrefabList prefabList;

    List<CopyInfo> copiedObjects = new List<CopyInfo>();
    Vector3 centerPoint = Vector3.positiveInfinity;
    #endregion
    CameraControl cameraControl;
    [SerializeField] Transform groupingArea;

    public enum State
    {
        Copy,
        PreviewPaste,
        Paste
    }
    public State currentState = State.Copy;

    class HighAndLow
    {
        #region Properties
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
        public float Low
        {
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
        #endregion

        #region Constructors
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
    }
    private void Awake()
    {
        cameraControl = new CameraControl();
    }

    private void OnEnable()
    {
        cameraControl.Enable();//enable every action map
        //controlScript.Player.Enable();//enable specific action map//Variation 1
        //controlScript.asset.actionMaps[0].Enable();//enabled specific action map//Variation 2
    }
    private void OnDisable()
    {
        cameraControl.Disable();//disable every action map
        //controlScript.Player.Disable();//disable specific action map//Variation 1
        //controlScript.asset.actionMaps[0].Disable();//disable specific action map/Variation 2
    }

    public void Start()
    {
        prefabList = GetComponent<FMPrefabList>();
        CopyInfo.group = groupingArea;
    }

    private void Update()
    {
        if (cameraControl.Editor.Control.triggered == true)
        {
            if (cameraControl.Editor.Copy.phase == InputActionPhase.Started)
            {
                Copy();
            }
            else if (currentState == State.Paste && cameraControl.Editor.Paste.triggered)
            {
                    Paste();
            }
        }
        else if (currentState == State.Paste && cameraControl.Editor.Cancel.triggered)
        {
            CancelPreview();
        }
    }

    #region functionality
    public void Copy()
    {
        bool cleared = false;

        //variables for center point calculation
        HighAndLow X = new HighAndLow();
        HighAndLow Y = new HighAndLow();
        HighAndLow Z = new HighAndLow();

        foreach(Selector selector in FindObjectsOfType<Selector>())
        {
            if (selector.IsSelected)
            {
                if(cleared == false)
                {//only clear the inputs after we are sure something else is copied
                    copiedObjects.Clear(); //forget about the old copied stuff
                    cleared = true;
                }
                //save copied objects info
                FMPrefab prefabType = prefabList.GetPrefabType(selector.gameObject);
                XML details = prefabType.ConvertToXML(selector.gameObject);
                CopyInfo copiedObject = new CopyInfo(prefabType, details);

                copiedObjects.Add(copiedObject);

                //center point variable update
                Vector3 position = selector.transform.position;
                X.Set(position.x);
                Y.Set(position.y);
                Z.Set(position.z);
            }
        }

        Vector3 centerPoint = new Vector3(X, Y, Z);

        foreach(CopyInfo copiedInfo in copiedObjects)
        {
            copiedInfo.CenterPoint = centerPoint;
        }

        currentState = State.PreviewPaste;
        Preview();
    }

    public void Preview()
    {
        foreach(CopyInfo copyInfo in copiedObjects)
        {
            copyInfo.Preview();
        }
        currentState = State.Paste;
    }

    public void CancelPreview()
    {
        while (groupingArea.childCount > 0)
        {
            Destroy(groupingArea.GetChild(0));
        }
    }

    public void Paste()
    {
        foreach(CopyInfo copyInfo in copiedObjects)
        {
            copyInfo.Instanciate();
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

    Transform example;

    public static Transform group;
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

    public GameObject Preview()
    {
        example = originalPrefab.InstanciateExample().transform;
        example.transform.position = group.position - offset;
        example.parent = group.transform;

        return example.gameObject;
    }

    public GameObject Instanciate()
    {
        GameObject newObject = originalPrefab.InstanciatePrefab(details);
        newObject.transform.position = example.position;

        return newObject;
    }
}