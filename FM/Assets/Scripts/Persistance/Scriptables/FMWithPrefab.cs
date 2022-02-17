using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FmPrefabWithSDS", menuName = "FMPrefabs/FmPrefabWithSDS")]
public class FMWithPrefab : FMPrefab
{
    public Texture diamondBase;
    public Texture numberBase;

    public override XML ConvertToXML(GameObject gameObject)
    {
        XML xml = base.ConvertToXML(gameObject);

        SDSInfo sds = gameObject.GetComponent<SDSInfo>();

        xml.AddAttribute("Health", sds.Health.ToString());
        xml.AddAttribute("Flamability", sds.Flamability.ToString());
        xml.AddAttribute("Reaction", sds.Reactivity.ToString());

        xml.AddAttribute("SDSSheet", sds.sdsPath);

        return xml;
    }

    public override GameObject InstanciatePrefab(XML xml)
    {
        GameObject gameObject = base.InstanciatePrefab(xml);

        SDSInfo sds = gameObject.GetComponent<SDSInfo>();
        sds.Health = int.Parse(xml.attributes["Health"]);
        sds.Flamability = int.Parse(xml.attributes["Flamability"]);
        sds.Reactivity = int.Parse(xml.attributes["Reaction"]);

        sds.sdsPath = xml.attributes["SDSSheet"];

        return gameObject;
    }
}
