using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/*Flower Box
 * Programmer: Patrick Naatz
 * 
 * Intention: Made this in c++ to explort and inport xml
 * files into programs, it has been revamped slightly for c#
 * 
 * Adjusted to enter the correct folder (Simulations)
 * this is done through hard coding because I do not believe we want the folder selecte dot be able to change
 * 
 */
public class XML
{
    #region Fields
    #region constant characters
    const char delimiter = '/';
	const char tagOpener = '<';
	const char tagCloser = '>';
    #endregion

    public string name;
	public List<XML> subObjects; //maybe subTags would be better for wording?
	public Dictionary<string, string> attributes;
    #endregion

    #region Constructors
    public XML()
	{
		subObjects = new List<XML>();
		attributes = new Dictionary<string, string>();
	}

	public XML(string tag) : this()
	{
		this.name = tag;
	}
    #endregion

    #region Find
    public XML FindChild(string childName) //TODO change how Find works to allow passing in an action to define the player
	{
		//checks the top row of subobjects so we find the highest relevant value of "Child"
		foreach (XML xml in subObjects)
		{
			if (xml.name == childName)
			{
				return xml;
			}
		}

		//Check subobjects to slowly move down the tree
		foreach (XML xml in subObjects)
		{
			XML ptr = xml.FindChild(childName);
			if (ptr != null)
			{
				return ptr;
			}
		}
		return null;
	}

	/// <summary>
	/// Use this function if you want to gaurentee that a child should exist
	/// </summary>
	/// <param name="childName"></param>
	/// <returns></returns>
	public XML FindOrMakeChild(string childName) //TODO rastaerize this function to accept find by attribute as well
	{
		XML child = FindChild(childName);

		if (child == null)
		{//couldnt find the child
			child = AddChild(childName);
		}

		return child;
	}

	/// <summary>
	/// finds child by an attribute and related value
	/// </summary>
	/// <param name="attribute"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public XML FindChildByAttribute(string attribute, string value)
	{
		//checks upper layer
		foreach (XML xml in subObjects)
		{
			if (xml.attributes[attribute] == value)
			{
				return xml;
			}
		}

		//proceeds to lower layers
		foreach (XML xml in subObjects)
		{
			XML ptr = xml.FindChildByAttribute(attribute, value);
			if (ptr != null)
			{
				return ptr;
			}
		}
		return null;
	}
    #endregion

    #region Add
    #region Add Child
    public XML AddChild(XML child)
	{
		subObjects.Add(child);

		return child;
	}

	public XML AddChild(string tagName)
	{
		XML child = new XML(tagName);
		AddChild(child); //delegation

		return child;
	}
    #endregion
    #region Add Attribute
    public void AddAttribute(KeyValuePair<string, string> p)
	{
		attributes.Add(p.Key, p.Value);
	}

	public void AddAttribute(string attribute, string value)
	{
		AddAttribute(new KeyValuePair<string, string>(attribute, value)); //delegation
	}
    #endregion
    #endregion

    #region Remove
    #region Remove Child
    public XML RemoveChild(XML child)
	{
		subObjects.Remove(child);

		return child;
	}

	public XML RemoveChild(string name)
	{
		XML child = FindChild(name);
		RemoveChild(child); //delegation

		return child;
	}

	public XML RemoveChildWithAttribute(string attribute, string value)
	{
		XML child = FindChildByAttribute(attribute, value);
		RemoveChild(child);//delegation

		return child;
	}
    #endregion
    #region Remove Attribute
    public void RemoveAttribute(string attribute)
	{
		attributes.Remove(attribute);
	}

	public void RemoveAttribute(KeyValuePair<string, string> p)
	{
		RemoveAttribute(p.Key); //delegation
	}
	#endregion
	#endregion

	#region Exporting
	public void ExportXML(string fileName)
	{
		//requires that XML has a xml tag as the highest level tag
		if (name == "" || name == null)
		{
			name = "XML";
		}

		if (name != "XML")
		{
			XML xml = new XML("XML");
			xml.AddChild(this);
			xml.ExportXML(fileName);

			return;
		}

		string dir = Directory.GetCurrentDirectory() + "\\Simulations\\";
		fileName = dir + fileName;

		StreamWriter file = new StreamWriter(fileName);
		ReadToFile(file, 0);
		file.Close();
	}

	public void ReadToFile(StreamWriter file, int tabation)
    {
        string tabs = Tab(tabation);

        file.WriteLine(tabs + ToLine());
        foreach (XML subTag in subObjects)
        {
            subTag.ReadToFile(file, tabation + 1);
        }

        if (subObjects.Count != 0)
        {
            file.WriteLine(tabs + tagOpener + delimiter + name + tagCloser);
        }
    }

    

    #region ToString Functionality
	private static string Tab(int tabation)
    {
        string tabs = "";
        for (int i = 0; i < tabation; i++)
        {
            tabs += "\t";
        }

        return tabs;
    }
    public string ToLine()
	{
		string line = tagOpener + name + AttributesToString();
		if (subObjects.Count == 0)
		{
			line += delimiter;
		}
		line += tagCloser;

		return line;
	}

	public string AttributesToString()
	{
		string line = "";
		foreach (KeyValuePair<string, string> p in attributes)
		{
			if (line != "")
			{
				line += " ";
			}

			line += p.Key + "=\"" + p.Value + "\"";
		}

		if (line != "")
		{
			line = " " + line;
		}

		return line;
	}
	#endregion
	#endregion

	#region Importing
	public static XML readfromfile(string fileName)
	{
		string dir = Directory.GetCurrentDirectory() + "\\Simulations\\";
		fileName = dir + fileName;
		Stack<XML> stck = new Stack<XML>();
		XML top = null;
		string line;

		//fix file name
		if (fileName.Substring(fileName.Length - 4, 4).ToLower() != ".xml")
		{
			fileName += ".xml";
		}

		StreamReader myfile = new StreamReader(fileName);

		while ((line = myfile.ReadLine()) != null)
		{
			XML current;
			if (stck.Count > 0)
			{
				current = stck.Peek();

				string tempLine = Gut(line);
				if (tempLine.ElementAt(0) == delimiter)
				{ //if end of a tag with objects in it ie </xml>
					stck.Pop();
				}
				else if (tempLine.ElementAt(tempLine.Length - 1) != delimiter)
				{// if this tag has sub tags and is not <xml/>
					XML newCurrent = stringToXML(line);
					current.subObjects.Add(newCurrent);
					stck.Push(newCurrent);
				}
				else
				{
					current.subObjects.Add(stringToXML(line));
				}
			}
			else if (top == null)
			{//if starting tag
				top = stringToXML(line);
				current = top;
				stck.Push(current);
			}
		}
		myfile.Close();

		return top;

	}

	public static XML stringToXML(string line)
	{
		XML tag = new XML();
		string internals = Gut(line);

		int pos = internals.IndexOf(" ");
		tag.name = (pos != -1) ? internals.Substring(0, pos) : internals;

		
		while (pos != -1 && internals != "")
		{//while there are attributes
			internals = internals.Substring(pos + 1); //removed used pieces
			pos = internals.IndexOf('"');//find begining of value
			if (pos != -1)
			{
				string name = internals.Substring(0, pos - 1); //attribute name
				internals = internals.Substring(pos + 1);//remove name

				int posLocal = internals.IndexOf('"'); //find end of value

				string value = internals.Substring(0, posLocal); //attribute value
				internals = internals.Substring(posLocal + 1);//remove value

				tag.attributes.Add(name, value);
			}

			pos = internals.IndexOf(" "); //position for next attribute
		}

		return tag;
	}

	public static string Gut(string tag)
	{
		tag = tag.Substring(tag.IndexOf(tagOpener));
		tag = tag.Substring(1, tag.Length - 2);

		return tag;
	}
    #endregion
}
