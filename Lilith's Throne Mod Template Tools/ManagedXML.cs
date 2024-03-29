﻿using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace ManagedXML
{
    abstract class ManagedXmlNode
    {
        public string name { set; get; }
        public Dictionary<string, string> attributes { get; set; }
        abstract public void writeToXml(XmlWriter w);
        public bool required { set; get; }
    }

    class StringNode : ManagedXmlNode
    {
        public string content { get; set; }
        public string DefaultValue { get; set; }
        override public void writeToXml(XmlWriter w)
        {
            w.WriteStartElement(this.name);
            foreach (KeyValuePair<string,string> entry in this.attributes)
            {
                w.WriteAttributeString(entry.Key, entry.Value);
            }
            w.WriteValue(this.content.ToString());
            w.WriteEndElement();
        }

        public StringNode(string name, string dv, bool required=true)
        {
            this.name = name;
            this.DefaultValue = dv;
            this.content = dv;
            this.required = required;
        }
    }

    class CDataNode : ManagedXmlNode
    {
        public string content { get; set; }
        public string DefaultValue { get; set; }
        override public void writeToXml(XmlWriter w)
        {
            w.WriteStartElement(this.name);
            foreach (KeyValuePair<string, string> entry in this.attributes)
            {
                w.WriteAttributeString(entry.Key, entry.Value);
            }
            w.WriteCData(this.content.ToString());
            w.WriteEndElement();
        }
        public CDataNode(string name, string dv, bool required = true)
        {
            this.name = name;
            this.DefaultValue = dv;
            this.content = dv;
            this.required = required;
        }
    }

    class IntNode : ManagedXmlNode
    {
        public int content { get; set; }
        public int DefaultValue { get; set; }
        public override void writeToXml(XmlWriter w)
        {
            w.WriteStartElement(this.name);
            foreach (KeyValuePair<string, string> entry in this.attributes)
            {
                w.WriteAttributeString(entry.Key, entry.Value);
            }
            w.WriteValue(this.content);
            w.WriteEndElement();
        }

        public IntNode(string name, int dv, bool required = true)
        {
            this.name = name;
            this.DefaultValue = dv;
            this.content = dv;
            this.required = required;
        }
    }

    class BooleanNode : ManagedXmlNode
    {
        public bool? content { get; set; }
        public bool? DefaultValue { get; set; }
        public override void writeToXml(XmlWriter w)
        {
            w.WriteStartElement(this.name);
            foreach (KeyValuePair<string, string> entry in this.attributes)
            {
                w.WriteAttributeString(entry.Key, entry.Value);
            }
            w.WriteValue(this.content);
            w.WriteEndElement();
        }

        public BooleanNode(string name, bool dv, bool required=true)
        {
            this.name = name;
            this.required = required;
            this.DefaultValue = dv;
            this.content = dv;
        }
    }

    class NestedNode: ManagedXmlNode
    {
        public List<ManagedXmlNode> content { get; set; }
        public List<string> SubnodeNames { get; set; }
        public List<ManagedXmlNode> DefaultValue { get; set; }
        public List<string> DefaultSubnodeNames { get; set; }
        public override void writeToXml(XmlWriter w)
        {
            w.WriteStartElement(this.name);
            foreach (KeyValuePair<string, string> entry in this.attributes)
            {
                w.WriteAttributeString(entry.Key, entry.Value);
            }
            foreach (var entry in Enumerable.Zip(this.SubnodeNames, this.content, (name, subnode) => new { name, subnode }))
            {
                entry.subnode.writeToXml(w);
            }
            w.WriteEndElement();
        }

        /*public void AddStringSubnode(string subnodeName, string def, bool required = true, bool cdata = false)
        {
            if (cdata)
            {
                AddCDataSubnode(subnodeName, def, required);
            }
            else
            {
                AddSubnode(new StringNode(subnodeName, def, required));
            }
        }*/

        public StringNode AddStringSubnode(string subnodeName, string def, bool required=true)
        {
            StringNode sn = new StringNode(subnodeName, def, required);
            AddSubnode(sn);
            return sn;
        }

        public CDataNode AddCDataSubnode(string subnodeName, string def, bool required=true)
        {
            CDataNode cdn = new CDataNode(subnodeName, def, required);
            AddSubnode(cdn);
            return cdn;
        }

        public IntNode AddIntSubnode(string subnodeName, int def, bool required = true)
        {
            IntNode intn = new IntNode(subnodeName, def, required);
            AddSubnode(intn);
            return intn;
        }
        public BooleanNode AddBooleanSubnode(string subnodeName, bool def, bool required=true)
        {
            BooleanNode bn = new BooleanNode(subnodeName, def, required);
            AddSubnode(bn);
            return bn;
        }

        public NestedNode AddNestedSubnode(string subnodeName, List<ManagedXmlNode> def, bool required = true)
        {
            NestedNode nn = new NestedNode(subnodeName, def, required);
            AddSubnode(nn);
            return nn;
        }

        /*public void AddSubnode(string subnodeName, string typeName, object def, bool required = true, bool cdata = false)
        {
            switch (typeName.ToLower())
            {
                case "string":
                    AddStringSubnode(subnodeName, (string)def, required, cdata);
                    break;
                case "int":
                    AddIntSubnode(subnodeName, (int)def, required);
                    break;
                case "bool":
                case "boolean":
                    AddBooleanSubnode(subnodeName, (bool)def, required);
                    break;
                case "node":
                case "nestednode":
                    AddNestedSubnode(subnodeName, new Dictionary<string, ManagedXmlNode>(), required);
                    break;
                default:
                    throw new ArgumentException(String.Format("Invalid typeName: {0}", typeName), "typeName");
            }
        }*/

        public ManagedXmlNode AddSubnode(ManagedXmlNode subnode)
        {
            this.content.Append(subnode);
            this.SubnodeNames.Append(subnode.name);
            return subnode;
        }

        public ManagedXmlNode GetSubnode(string subnodeName)
        {
            foreach (var entry in Enumerable.Zip(this.SubnodeNames, this.content, (name, subnode) => new { name, subnode }))
            {
                if (entry.name == subnodeName)
                {
                    return entry.subnode;
                }
            }
            return null;
        }
        public bool DeleteSubnode(string subnodeName)
        {
            for (int i = 0; i < this.content.Count; i++)
            {
                string thisname = this.SubnodeNames[i];
                if (thisname == subnodeName)
                {
                    this.content.RemoveAt(i);
                    this.SubnodeNames.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            this.content.Clear();
            this.SubnodeNames.Clear();
        }

        public NestedNode(string name, List<ManagedXmlNode> dv, bool required = true)
        {
            this.name = name;
            this.required = required;
            this.DefaultValue = dv;
            this.content = new List<ManagedXmlNode>(dv);
            foreach (ManagedXmlNode subnode in dv)
            {
                this.DefaultSubnodeNames.Append(subnode.name);
                this.SubnodeNames.Append(subnode.name);
            }
            
        }
    }
}