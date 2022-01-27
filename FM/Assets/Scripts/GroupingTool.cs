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

    CameraControl cameraControl;
    bool control = false;

    [SerializeField] Transform groupingArea;

    public enum State
    {
        Copy,
        PreviewPaste,
        Paste
    } public State currentState = State.Copy;
    #endregion

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

    #region Unity functions
    private void Awake()
    {
        cameraControl = new CameraControl();
        cameraControl.Editor.Control.performed += Control_performed;
        cameraControl.Editor.Control.canceled += Control_canceled;

        cameraControl.Editor.Copy.canceled += Copy_performed;//NOTE we add these function to the canceled function because we want to wait for them to release the command first
        cameraControl.Editor.Paste.canceled += Paste_performed;
    }

    private void OnEnable()
    {
        cameraControl.Enable();//enable every action map
    }

    private void OnDisable()
    {
        cameraControl.Disable();//disable every action map
    }

    public void Start()
    {
        prefabList = GetComponent<FMPrefabList>();
        CopyInfo.group = groupingArea;
    }
    #endregion

    #region event handling
    private void Paste_performed(InputAction.CallbackContext obj)
    {
        if (control)
        {
            switch (currentState)
            {
                case State.Paste:
                    Paste();
                    break;
                case State.PreviewPaste:
                    Preview();
                    break;
            }
        }
    }

    private void Copy_performed(InputAction.CallbackContext obj)
    {
        if(control)
            Copy();
    }

    #region Control handlers
    private void Control_canceled(InputAction.CallbackContext obj)
    {
        control = false;
    }

    private void Control_performed(InputAction.CallbackContext obj)
    {
        control = true;
    }
    #endregion
    #endregion

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
                    CancelPreview();//incase they are a real programmer who goes ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C, ctrl+V
                    cleared = true;
                }

                //save copied objects info
                FMPrefab prefabType = prefabList.GetPrefabType(selector.transform.parent.gameObject);
                XML details = prefabType.ConvertToXML(selector.transform.parent.gameObject);
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

    #region Pasting
    #region Preview
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
        for(int i = groupingArea.childCount; i > 0; i--)
        {
            Destroy(groupingArea.GetChild(i - 1).gameObject);
        }

        currentState = State.PreviewPaste;
    }
    #endregion

    public void Paste()
    {
        foreach(CopyInfo copyInfo in copiedObjects)
        {
            copyInfo.Instanciate();
        }
        CancelPreview();
    }
    #endregion
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

    #region Behaviors
    public GameObject Preview()
    {
        example = originalPrefab.InstanciateExample().transform;
        example.parent = group.transform;
        example.GetChild(0).transform.localPosition = offset;//NOTE for some reason example.transform.localPosition/Position does not actually change the location of the object. Unity Glitch

        return example.gameObject;
    }

    public GameObject Instanciate()
    {
        GameObject newObject = originalPrefab.InstanciatePrefab(details);
        newObject.transform.position = example.GetChild(0).transform.position;//See Preview for explanation
        newObject.SetActive(true);

        return newObject;
    }
    #endregion
}