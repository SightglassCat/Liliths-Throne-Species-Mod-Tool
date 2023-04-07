using System;
using System.Xml;
using System.Collections.Generic;

namespace ManagedXML
{
    abstract class ManagedXmlNode
    {
        public string name { get; }
        public Dictionary<string, string> attributes { get; set; }
        abstract public void writeToXml(XmlWriter w);
    }

    class StringNode : ManagedXmlNode
    {
        public string content { get; set; }
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
    }

    class CDataNode : ManagedXmlNode
    {
        public string content { get; set; }
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
    }

    class IntNode<T> : ManagedXmlNode
    {
        public int content { get; set; }
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
    }

    class BooleanNode : ManagedXmlNode
    {
        public bool content { get; set; }
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
    }

    class NestedNode: ManagedXmlNode
    {
        public Dictionary<string, ManagedXmlNode> content { get; set; }
        public override void writeToXml(XmlWriter w)
        {
            w.WriteStartElement(this.name);
            foreach (KeyValuePair<string, string> entry in this.attributes)
            {
                w.WriteAttributeString(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<string, ManagedXmlNode> subnode in this.content)
            {
                subnode.Value.writeToXml(w);
            }
            w.WriteEndElement();
        }

        public void AddSubnode(ManagedXmlNode subnode)
        {
            AddSubnode(subnode.name, subnode);
        }
        public void AddSubnode(string name, ManagedXmlNode subnode)
        {
            this.content.Add(name, subnode);
        }
        public bool DeleteSubnode(string name)
        {
            if (this.content.ContainsKey(name))
            {
                this.content.Remove(name);
                return true;
            }
            else return false;
        }
    }
}