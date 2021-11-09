using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Flower Box
 * Programmer: Patrick Naatz
 * Intention: to give an example of how to extend the FMPrefab class
 * This portion of code works alongside the Barell.cs script
 */

[CreateAssetMenu(fileName = "Barell", menuName = "FMPrefabs/Barell")]
public class ScriptableBarell : FMPrefab
{
    public override XML ConvertToXML(GameObject gameObject)
    {
        XML xml = base.ConvertToXML(gameObject);

        Barell barell = gameObject.GetComponent<Barell>();
        Barell.SDS sds = barell.sds;
        //xml.AddAttribute("Health", sds.Health.ToString());
        //xml.AddAttribute("Flamability", sds.Flamability.ToString());
        //xml.AddAttribute("Reaction", sds.Reaction.ToString());
        //xml.AddAttribute("PersonalProtection", sds.PersonalProtection.ToString());

        return xml;
    }

    public override GameObject InstanciatePrefab(XML xml)
    {
        Barell barell = base.InstanciatePrefab(xml).GetComponent<Barell>();
        //Barell.SDS sds = barell.sds;
        //sds.Health = int.Parse(xml.attributes["Health"]);
        //sds.Flamability = int.Parse(xml.attributes["Flamability"]);
        //sds.Reaction = int.Parse(xml.attributes["Reaction"]);
        //sds.PersonalProtection = int.Parse(xml.attributes["PersonalProtection"]);

        //barell.sds = sds;

        return barell.gameObject;
    }
}

