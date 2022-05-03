//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;


///* Programmed By Patrick Naatz
// * 
// * Command Key:
// * 0:
// *  -Preview
// *  -FMPrefab
// * 1:
// *  RelativePosition
// */
//public class PreviewTool : MonoBehaviour, Monkey
//{
//    SelectorTool selectorTool;
//    [SerializeField] FMPrefab prefab; //TODO consider changing this to a public variable, so we can make this a singleton pattern

//    // Start is called before the first frame update
//    void Start()
//    {
//        selectorTool = SelectorTool.selectorTool;
//    }

//    // Update is called once per frame
//    void Update()
//    {   
//    }

//    public Command MakeCommand(bool instantInvoke = true)
//    {
//        Vector3 DistanceFromCenter = Vector3.zero; //Change this later on
//        return new Command(Preview, CancelPreview, false, instantInvoke: instantInvoke, prefab, DistanceFromCenter);
//    }

//    #region Preview
//    private ReturnStatus Preview(ref object[] parameters)
//    {
//        FMPrefab prefab = parameters[0] as FMPrefab;
//        Vector3 relativePostion = ExtractRelativePosition(ref parameters);
//        Preview preview = null;

//        ReturnStatus returnStatus = Preview(ref preview, prefab, relativePostion);

//        parameters[0] = preview;
//        return returnStatus;
//    }

//    public ReturnStatus Preview(ref Preview preview, FMPrefab prefab, Vector3 relativePosition)
//    {
//        preview = prefab.InstanciateExample().GetComponent<Preview>(); //TODO change InstanceExample to return Preview, also change the name to match naming conventions
//        preview.transform.position = LevelEditingManagerRedo.levelEditingManager.CalculateNewPosition(relativePosition);

//        return ReturnStatus.Success;
//    }

//    Vector3 ExtractRelativePosition(ref object[] parameters)
//    {
//        Vector3 relativePostiion;
//        if (parameters.Length == 1)
//        {
//            List<object> objs = parameters.ToList();
//            objs.Add(Vector3.zero);
//            parameters = objs.ToArray();

//            relativePostiion = Vector3.zero;
//        }
//        else
//        {
//            relativePostiion = (Vector3)parameters[1];
//        }

//        return relativePostiion;
//    }
//    #endregion
//    #region CancelPreview
//    private ReturnStatus CancelPreview(ref object[] parameters)
//    {
//        Preview preview = parameters[0] as Preview;
//        FMPrefab prefab = null;

//        ReturnStatus returnStatus = CancelPreview(preview, ref prefab);

//        parameters[0] = prefab;

//        return returnStatus;
//    }

//    public ReturnStatus CancelPreview(Preview preview, ref FMPrefab prefab)
//    {
//        if (preview == null)
//        {
//            Debug.LogError("Preview was already canceled");
//            return ReturnStatus.Failed;
//        }

//        Destroy(preview.gameObject);

//        return ReturnStatus.Success;
//    }
//    #endregion
//}