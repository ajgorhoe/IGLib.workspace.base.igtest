
    // INTERPRETATION AND CREATION OF MESSAGES for communication with Bis.

using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using IG.Lib;
// using System.Configuration;

namespace IG.Lib
{


    /// <summary>Base class for various utilities operating on XmlDocumnt.</summary>
    public class XmlUtilityBase
    // $A Igor Aug08 Apr09;
    {


        IReporter _R = null;

        /// <summary>Reporter for this class.</summary>
        public virtual IReporter R { get {

            // TODO: Change this!

            if (_R == null)
                _R = new ReporterConsole();
            return _R; } }


        #region NameSpaceResolution


        private bool _hasDefaultNameSpace = false;


        public virtual bool HasDefaultNameSpace
        {
            get { return _hasDefaultNameSpace; }
            protected set { _hasDefaultNameSpace=value; }
        }

        public string DefaultNameSpace = null;

        public string DefaultNameSpacePrefix = "defaultnsprefix";



        private const string XmlNsAttribute = "xmlns";

        /// <summary>Gets the default namespace URI if defined, for the specified Xml element.</summary>
        /// <param name="element"></param>
        public static string GetDefaultNameSpaceUri(XmlElement element)
        {
            string ret = null;
            if (element != null)
            {
                try
                {
                    ret = element.GetAttribute(XmlNsAttribute);
                }
                catch { }
                if (ret == null)
                {
                    // The element does not directly contain the attribute defining the default namespace.
                    // Try with its ancestor nodes:
                    try
                    {
                        XmlElement parent = element.ParentNode as XmlElement;
                        if (parent != null)
                            ret = GetDefaultNameSpaceUri(parent);
                    }
                    catch { }
                }
            }
            return ret;
        }

        /// <summary>Gets the default namespace URI of teh Xml document.</summary>
        /// <param name="doc">Xml document to which the default namespace might apply.</param>
        /// <returns>Tha default namespace URI that applies to the document, or null if there is no
        /// such namespace.</returns>
        public static string GetDefaultNameSpaceUri(XmlDocument doc)
        {
            string ret = null;
            if (doc != null)
            {
                ret = GetDefaultNameSpaceUri(doc.DocumentElement);

            }
            return ret;
        }


        /// <summary>Returns the default namespace URI that applies to the specified Xml node.</summary>
        /// <param name="node">Xml node to which namespace URI applies.</param>
        /// <returns>The defaulut namespace URI atht applies to the node, or null if there is no default namespace.</returns>
        public static string GetDefaultNameSpaceUri(XmlNode node)
        {
            if (node == null)
                return null;
            string ret = null;
            XmlElement el = node as XmlElement;
            if (el == null)
                el = node.ParentNode as XmlElement;
            if (el != null)
                ret = GetDefaultNameSpaceUri(el);
            return ret;
        }



        private static string GetNamespaceAttributeName(string prefix)
        {
            return (XmlNsAttribute + ":" + prefix);
        }

        /// <summary>Gets the namespace URI introduced by a particular attribute, if defined, 
        /// for the specified Xml element.</summary>
        /// <param name="element">Element for which rhe specific namespace URI is searched for.</param>
        /// <param name="NamespaceAttributeName">Attribute name that introduces that namespace.</param>
        private static string GetNameSpaceUri0(XmlElement element, string NamespaceAttributeName)
        {
            string ret = null;
            if (element != null)
            {
                try
                {
                    ret = element.GetAttribute(NamespaceAttributeName);
                }
                catch { }
                if (ret == null)
                {
                    // The element does not directly contain the attribute defining the default namespace.
                    // Try with its ancestor nodes:
                    try
                    {
                        XmlElement parent = element.ParentNode as XmlElement;
                        if (parent != null)
                            ret = GetNameSpaceUri0(parent, NamespaceAttributeName);
                    }
                    catch { }
                }
            }
            return ret;
        }

        /// <summary>Returns the namespace URI associated with a specific prefix that applies to
        /// the specified Xml element.
        /// URI information is obtained from the corresponding attribute of the specified node and
        /// eventually its parent nodes.</summary>
        /// <param name="element">Element for which namespace URI is searched for.</param>
        /// <param name="prefix">Prefix for which teh namespace is searched for. If null or empty string
        /// then a default namespace URI is searched for.</param>
        /// <returns>The namespace URI corresponding to the specified prefix at the level of a specified element,
        /// or null  if the particular namespace URI is not defined.</returns>
        private static string GetNameSpaceUri(XmlElement element, string prefix)
        {
            if (element == null)
                return null;
            else if (string.IsNullOrEmpty(prefix))
                return GetDefaultNameSpaceUri(element);
            else
                return GetNameSpaceUri0(element, GetNamespaceAttributeName(prefix));
        }


        /// <summary>Returns the namespace URI associated with a specific prefix that applies to
        /// the specified Xml document.</summary>
        /// <param name="doc">Xml document to which namespace URI applies.</param>
        /// <returns>The namespace URI if found, null otherwise.</returns>
        public static string GetNameSpaceUri(XmlDocument doc, string prefix)
        {
            string ret = null;
            if (doc != null)
            {
                ret = GetNameSpaceUri(doc.DocumentElement, prefix);

            }
            return ret;
        }

        /// <summary>Returns the namespace URI associated with a specific prefix that applies to
        /// the specified Xml document.</summary>
        /// <param name="node">Xml node to which namespace URI applies.</param>
        /// <returns>The namespace URI if found, null otherwise.</returns>
        public static string GetNameSpaceUri(XmlNode node, string prefix)
        {
            if (node == null)
                return null;
            string ret = null;
            XmlElement el = node as XmlElement;
            if (el == null)
                el = node.ParentNode as XmlElement;
            if (el != null)
                ret = GetNameSpaceUri(el, prefix);
            return ret;
        }





        #endregion  // NameSpaceResolution


        #region Initialization

        public virtual void SetDocument(XmlDataDocument doc)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("XmlParser.Load()", "Started...");
            try
            {
                Doc = doc;
                FileName = null;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("XmlParser.Load()", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Loads an Xml document from a file.</summary>
        /// <param name="filename">Name of the file from which XML document is loaded.</param>
        public virtual void Load(string filename)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("XmlParser.Load()", "Started...");
            try
            {
                XmlDocument doctmp = new XmlDocument();
                doctmp.Load(filename);
                Doc = doctmp;
                FileName = filename;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("XmlParser.Load()", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Loads Xml document from a string.</summary>
        /// <param name="docstr">String containing the XML document.</param>
        public virtual void LoadXml(string docstr)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("XmlParser.LoadXml()", "Started...");
            try
            {
                XmlDocument doctmp = new XmlDocument();
                doctmp.LoadXml(docstr);
                Doc = doctmp;
                FileName = null;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("XmlParser.LoadXml()", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Loads Xml document.</summary>
        /// <param name="doc">XML document that is loaded.</param>
        public virtual void LoadXml(XmlDocument doc)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("XmlParser.LoadXml()", "Started...");
            try
            {
                Doc = doc;
                FileName = null;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("XmlParser.LoadXml()", "Finished.");
                --R.Depth;
            }
        }

        string _filename = null;

        public string FileName
        {
            get { return _filename; }
            protected set { _filename = value; }
        }




        #endregion  // Initialization


        private XmlDocument _doc = null;

        /// <summary>Xml document that represents the msg.</summary>
        public virtual XmlDocument Doc
        {
            get { return _doc; }
            set
            {
                _doc = value;
                if (_doc != null)
                {
                    Root = _doc.DocumentElement;
                } else
                    Root = null;
            }
        }



        private XmlNode _root = null;
        /// <summary>Root node of the current document.</summary>
        public XmlNode Root
        {
            get { return _root; }
            private set
            {
                XmlNode nd = value;
                if (value != null)
                {
                    // Only a node that are contained in the document can be set as Root:
                    if (!ContainedInDocument(value, _doc))
                    {
                        R.ReportError("XmlParser.Root.set", "Attempt to set the node that is not contained in the current document.");
                        value = null;
                    }
                }
                _root = value;
            }
        }



        /// <summary>Returns true if an Xml node is contained in the specified Xml document (false if any is null).</summary>
        protected bool ContainedInDocument(XmlNode node, XmlDocument doc)
        {
            if (node == null || doc == null)
                return false;
            XmlNode current = node, parent = node;
            while (parent != null)
            {
                parent = current.ParentNode;
                if (parent != null)
                    current = parent;
            }
            if (current == doc)
                return true;
            else
                return false;
        }

        /// <summary>Returns null if an XML node (first argument) is contained in the specified note (second argument).
        /// The node can be contained in another node at an arbitrary depth for the function to return true.
        /// If any of the nodes is null then the function returns false.</summary>
        /// <param name="node">Node that might be contained in another node.</param>
        /// <param name="container">The node that might contain another node.</param>
        /// <returns>true if container contains node, false othwrwise.</returns>
        protected bool ContainedInNode(XmlNode node, XmlNode container)
        {
            if (node == null || container == null)
                return false;
            else if (node == container)
                return true;
            XmlNode current = node, parent = node;
            while (parent != null)
            {
                parent = current.ParentNode;
                if (parent != null)
                {
                    if (parent == container)
                        return true;
                    current = parent;
                }
            }
            return false;
        }



    }  // class XmlUtilityBase


    /// <summary>Base class for classes taht contain an Xml document that can be parsed.
    /// Provides comfortable utilities for transversing the document and for querying the value, name, and 
    /// attributes of the current node.</summary>
    public class XmlParser : XmlUtilityBase
    // $A Igor Aug08 Mar09;
    {


        /// <summary>Xml document that represents the msg.</summary>
        public override XmlDocument Doc
        {
            get { return base.Doc; }
            set
            {
                base.Doc = value;
                Current = Previous = Root;
                marks.Clear();
            }
        }



        #region Querying


        private XmlNode _current = null;
        /// <summary>The current node on which all queries are performed.</summary>
        public XmlNode Current
        {
            get { return _current;  }
            set
            {
                XmlNode nd = value;
                if (nd != null)
                {
                    // Only a node that are contained in the document can be set as Current:
                    if (!ContainedInDocument(nd, Doc))
                    {
                        R.ReportError("XmlParser.Current.set", "Attempt to set the node that is not contained in the current document.");
                        nd = null;
                    }
                }
                _current = nd;
            }
        }

        private XmlNode _previous = null;

        /// <summary>The node that was previously set to the current node.</summary>
        public XmlNode Previous 
        { 
            get  { return _previous; }
            set
            {
                XmlNode nd = value;
                if (nd!=null)
                {
                    // Only a node that are contained in the document can be set as Previous:
                    if (!ContainedInDocument(nd, Doc))
                    {
                        R.ReportError("XmlParser.Previous.set","Attempt to set the node that is not contained in the current document.");
                        nd=null;
                    }
                }
                _previous = nd;
            }
        }


        XmlNode _parent = null;
        /// <summary>Parent of Current. If Current = null then Parent can still return something, 
        /// which happens in particular when StepIn was performed before.
        /// Parent can be set to a specific node. That node will be returned by Parent when the Current
        /// is null (otherwise, the parent of Current is still returned). This is useful when current becomes
        /// null, e.g. because of running out of index bounds.</summary>
        public XmlNode Parent 
        { 
            get 
            {
                if (Current != null)
                    return Current.ParentNode;
                else
                    return _parent;
            }
            private set
            {
                XmlNode nd = value;
                if (nd != null)
                {
                    // Only a node that are contained in the document can be set as parent node:
                    if (!ContainedInDocument(nd, Doc))
                    {
                        R.ReportError("XmlParser.Parent.set", "Attempt to set1 the node that is not contained in the current document.");
                        nd = null;
                    }
                }
                _parent = nd;
            }
        }

        public XmlElement ParentElement { get { return Parent as XmlElement; } }
        public XmlElement CurrentElement { get  { return (Current as XmlElement); } }
        public XmlElement PreviousElement { get  { return (Previous as XmlElement); } }

        /// <summary>Returns the value of the specified attribute of the current node.
        /// If the attribute does not exist then null is returned.</summary>
        public string Attribute(string key)
        {
            string ret = null;
            try
            {
                ret = CurrentElement.GetAttribute(key);
            }
            catch { }
            return ret;
        }

        /// <summary>Returns the name of the current node.</summary>
        public string Name
        { 
            get 
            {
                if (Current == null)
                    return null;
                else
                    return Current.LocalName;
            } 
        }

        /// <summary>Returns the value of the current node.
        /// If the current node is an element then the value of its first Text child element is returned.</summary>
        public string Value
        {
            get
            {
                try
                {
                    if (Current == null)
                        return null;
                    else
                    {
                        if (Current.NodeType == XmlNodeType.Element)
                        {
                            // Remark: this method may not be implemented well, please reconsider & correct!
                            // We find the first Text child node of the current node and return its value:
                            XmlNode nd = Current.FirstChild;
                            while (nd != null && nd.NodeType != XmlNodeType.Text)
                                nd = nd.NextSibling;
                            if (nd != null)
                                return nd.Value;
                        } else
                            return Current.Value;
                    }
                }
                catch { }
                return null;
            }
        }

        /// <summary>Returns the inner Text of the current node.</summary>
        public string InnerText
        {
            get
            {
                try
                {
                    if (Current == null)
                        return null;
                    else
                        return Current.InnerText;
                }
                catch { }
                return null;
            }
        }


        #region Searches

        // Searches:
        // Functions that return results of different queries but do not change the current position:

        /// <summary>Returns the first node that satisfies a given XPath expression relative to the root node.
        /// It does not report any errors (just returns null in case of errors)</summary>
        /// <param name="path">XPath expression.</param>
        /// <returns>The first node that satisfies the path, or null if no such node is found.</returns>
        public XmlNode GetNode(string path)
        {
            XmlNode ret = null;
            try
            {
                ret = Root.SelectSingleNode(path);
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>Returns the first node that satisfies a given XPath expression relative to the current node.
        /// It does not report any errors.</summary>
        /// <param name="path">XPath expression.</param>
        /// <returns>The first node that satisfies the path, or null if no such node is found.</returns>
        public XmlNode GetRelative(string path)
        {
            XmlNode ret = null;
            try
            {
                ret = Current.SelectSingleNode(path);
            }
            catch
            {
            }
            return ret;
        }


        // REMARK: To mora biti implementirano tako, da so ekvivalentne zmogljivosti kot pri NextOrCurrentElement in NextElement!

        /// <summary>Finds the first sibling node of the starting node (or the current node itself) that satisfies the 
        /// specified conditions, and returns that node or null if such a node could not be found.
        /// This method does not report errors, it just returns null if the node satisfying conditions can not be found.</summary>
        /// <param name="StartingNode">Starting node at which the search is started.</param>
        /// <param name="NodeType">If not XmlNodeType.None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        /// <param name="IncludeCurrent">If true then the returned node can also be the current node
        /// (if it satisfies the conditions), otherwise the search starts at the next sibling of the current node.</param>
        /// <returns></returns>
        public static XmlNode GetNextNode(XmlNode StartingNode,XmlNodeType NodeType, string NodeName, 
                string NodeValue, bool IncludeCurrent)
        {
            XmlNode ret = null;
            try
            {
                if (StartingNode != null)
                {
                    XmlNode cur = StartingNode;
                    bool stop = false;
                    if (!IncludeCurrent)
                        cur=cur.NextSibling;
                    while (!stop)
                    {
                        if (cur == null)
                            stop = true;
                        else
                        {
                            stop = true;
                            if (NodeType != XmlNodeType.None) // type should be considered
                                if (cur.NodeType != NodeType)
                                    stop = false;
                            if (NodeName != null)  // Node name should also be considered
                                if (cur.LocalName != NodeName)
                                    stop = false;
                            if (NodeValue != null)
                                if (cur.Value != NodeValue)
                                    stop = false;
                        }
                        if (!stop)
                            cur=cur.NextSibling;
                    }
                    ret = cur;
                }
            }
            catch { }
            return ret;
        }



        // Changing the current node to the next OR the current node that satisfies given contitions:

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode GetNextNode(XmlNodeType NodeType, string NodeName, string NodeValue)
        {
            return GetNextNode(Current  /* StartingNode */, NodeType, NodeName, NodeValue, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode GetNextNode(XmlNodeType NodeType, string NodeName)
        {
            return GetNextNode(Current  /* StartingNode */, NodeType, NodeName, null /* NodeValue */, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that 
        /// is of the specified type.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        public XmlNode GetNextNode(XmlNodeType NodeType)
        {
            return GetNextNode(Current  /* StartingNode */, NodeType, null /* NodeName */, null /* NodeValue */, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node. Node type is not important.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode GetNextNode(string NodeName, string NodeValue)
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.None /* NodeType */, NodeName, NodeValue, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node. Node type is not important.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode GetNextNode(string NodeName)
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.None /* NodeType */, NodeName, null /* NodeValue */, false /* IncludeCurrent */);
        }


        // Changing the current node to the next OR the current node that satisfies given contitions:

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode GetNextOrCurrentNode(XmlNodeType NodeType, string NodeName, string NodeValue)
        {
            return GetNextNode(Current  /* StartingNode */, NodeType, NodeName, NodeValue, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode GetNextOrCurrentNode(XmlNodeType NodeType, string NodeName)
        {
            return GetNextNode(Current  /* StartingNode */, NodeType, NodeName, null /* NodeValue */, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that 
        /// is of the specified type.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        public XmlNode GetNextOrCurrentNode(XmlNodeType NodeType)
        {
            return GetNextNode(Current  /* StartingNode */, NodeType, null /* NodeName */, null /* NodeValue */, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node. Node type is not important.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode GetNextOrCurrentNode(string NodeName, string NodeValue)
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.None /* NodeType */, NodeName, NodeValue, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node. Node type is not important.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode GetNextOrCurrentNode(string NodeName)
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.None /* NodeType */, NodeName, null /* NodeValue */, true /* IncludeCurrent */);
        }

        // These two groups of functions that only cosider element nodes:

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode GetNextElement(string NodeName, string NodeValue)
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.Element /* NodeType */, NodeName, NodeValue, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode GetNextElement(string NodeName)
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.Element /* NodeType */, NodeName, null /* NodeValue */, false /* IncludeCurrent */);
        }


        /// <summary>Moves the current node to the first sibling element after the current node.</summary>
        public XmlNode GetNextElement()
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.Element /* NodeType */, null /* NodeName */, null /* NodeValue */, false /* IncludeCurrent */);
        }


        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode GetNextOrCurrentElement(string NodeName, string NodeValue)
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.Element /* NodeType */, NodeName, NodeValue, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode GetNextOrCurrentElement(string NodeName)
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.Element /* NodeType */, NodeName, null /* NodeValue */, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling element after the current node.</summary>
        public XmlNode GetNextOrCurrentElement()
        {
            return GetNextNode(Current  /* StartingNode */, XmlNodeType.Element /* NodeType */, null /* NodeName */, null /* NodeValue */, true /* IncludeCurrent */);
        }


        #endregion  // Searches

        #endregion Querying


        #region Traversing

        /// <summary>Position mark in the XML document</summary>
        public class Mark
        {
            public string name = null;
            public XmlNode Current = null, Previous = null, Parent = null;
        }



        private List<Mark> marks = new List<Mark>();

        /// <summary>Marks the current state of the XmlParser and sets mark name to name.
        /// Position is stored on a stack such that previous stored positions can be restored, either in a reverse way.</summary>
        /// <param name="name">Name assigned to the mark.</param>
        public void SetMark(string name)
        {
            XmlNode pos=null;
            try
            {
                Mark mark = new Mark();
                pos=mark.Current = Current;
                mark.Previous = Previous;
                mark.Parent = Parent;
                if (name==null)
                    name = "";
                mark.name = name;
                marks.Add(mark);
            }
            catch { }
            finally
            {
                if (pos==null)
                    R.ReportError("XmlParser.SetMark()","Invalid current position, could not set a mark.");
            }
        }

        /// <summary>Marks the current state of the XmlParser. The mark set is not named.
        /// Position is stored on a stack such that previous stored positions can be restored, either in a reverse way.</summary>
        /// <param name="name">Name assigned to the mark.</param>
        public void SetMark()
        {
            SetMark(null);
        }
        /// <summary>Restores the parser state that is stored in the specified mark. The current node after operation
        /// is returned, or null if mark is not specified (i.e., it is null).</summary>
        /// <param name="mark">The mark containing the state that is restored.</param>
        /// <returns>The current node after operation.</returns>
        protected XmlNode GoToMark(Mark mark)
        {
            XmlNode ret = null;
            try
            {
                if (mark != null)
                {
                    MoveTo(mark.Current);
                    ret=Current;
                    Previous = mark.Previous;
                    Parent = mark.Parent;
                }
            }
            catch{  }
            return ret;
        }

        /// <summary>Restores the parser state to the state contained in the last mark. The current node after operation
        /// is returned, or null if there are no marks. 
        /// If required then the last mark is removed from the list.</summary>
        /// <param name="removemarks">If true then all marks from the specified one on (inclusively) are removed.</param>
        /// <returns>The current node after operation or null if there is no valid last mark.</returns>
        public XmlNode GoToMark(bool removemark)
        {
            XmlNode ret = null;
            try
            {
                Mark mark = null;
                if (marks.Count > 0)
                {
                    mark = marks[marks.Count - 1];  // marks.Last();
                    if (removemark)
                    {
                        marks.RemoveAt(marks.Count-1);
                    }
                    ret = GoToMark(mark);
                }
            }
            catch { }
            return ret;
        }

        /// <summary>Restores the parser state to the state contained in the last mark with the specified name. 
        /// The current node after operation is returned, or null if there are no marks with such a name.
        /// If required then the targeted and all subsequent marks are removed from the list.</summary>
        /// <param name="name">Name of the mark whose state is restored.</param>
        /// <param name="removemarks">If true then all marks from the specified one on (inclusively) are removed.</param>
        /// <returns>The current node after operation or null if the specified mark has not been found.</returns>
        public XmlNode GoToMark(string name, bool removemarks)
        {
            XmlNode ret = null; 
            if (string.IsNullOrEmpty(name))
                throw new Exception("XmlParser.GoToMark(name, ...): mark name is not specified.");
            try
            {
                Mark mark = null;
                if (marks.Count>0)
                {
                    int which = marks.Count - 1;
                    // find the last mark with the specified name:
                    while (which >= 0 && mark == null)
                    {
                        Mark aux = marks[which];
                        if (aux != null) if (aux.name == name)
                            mark = aux;
                        if (mark == null)
                            --which;
                    }
                    // mark=marks.Last(m => m.name == name);
                    if (mark!=null)
                    {
                        if (removemarks)
                            // Remove all marks from the specified one (inclusively):
                            marks.RemoveRange(which,marks.Count-which);
                        ret = GoToMark(mark);
                    }
                }
            }
            catch { }
            return ret;
        }

        /// <summary>Restores the parser state to the state contained in the last mark. The current node after operation
        /// is returned, or null if there are no marks. 
        /// The last mark is LEFT on the list.</summary>
        /// <param name="removemarks">If true then all marks from the specified one on (inclusively) are removed.</param>
        /// <returns>The current node after operation or null if there is no valid last mark.</returns>
        public XmlNode GoToMark()
        {
            return GoToMark(false);
        }

        /// <summary>Restores the parser state to the state contained in the last mark with the specified name. 
        /// The current node after operation is returned, or null if there are no marks with such a name.
        /// Marks are NOT REMOVED from the stacks.</summary>
        /// <param name="name">Name of the mark whose state is restored.</param>
        /// <returns>The current node after operation or null if the specified mark has not been found.</returns>
        public XmlNode GoToMark(string name)
        {
            return GoToMark(name, false);
        }

        /// <summary>Restores the parser state to the state contained in the last mark, and 
        /// REMOVES that mark. 
        /// The current node after operation is returned, or null if there are no marks.</summary>
        /// <returns>The current node after operation or null if there is no valid last mark.</returns>
        public XmlNode BackToMark()
        {
            return GoToMark(true);
        }

        /// <summary>Restores the parser state to the state contained in the last mark with the specified name, and
        /// REMOVES all marks from that mark on (includively). 
        /// The current node after operation is returned, or null if there are no marks with such a name.</summary>
        /// <param name="name">Name of the mark whose state is restored.</param>
        /// <returns>The current node after operation or null if the specified mark has not been found.</returns>
        public XmlNode BackToMark(string name)
        {
            return GoToMark(name, true);
        }

        /// <summary>Removes the last mark and returns its current node.
        /// Position is not affected.</summary>
        /// <returns>The current node stored on the last mark, or null if no marks are defined.</returns>
        public XmlNode RemoveMark()
        {
            XmlNode ret = null;
            try
            {
                Mark mark = null;
                if (marks.Count > 0)
                {
                    mark = marks[marks.Count - 1];  // marks.Last();
                    marks.RemoveAt(marks.Count);
                    if (mark!=null) 
                        ret = mark.Current;
                }
            }
            catch { }
            return ret;
        }


        /// <summary>Removes all marks from the last mark with the specified name on the list and returns 
        /// the current node of the specified mark.
        /// Position is not affected.</summary>
        /// <returns>The current node stored on the last mark with the specified name, or null if such a mark is not found. </returns>
        public XmlNode RemoveMarks(string name)
        {
            XmlNode ret = null;
            if (string.IsNullOrEmpty(name))
                throw new Exception("XmlParser.RemoveMarks(name, ...): mark name is not specified.");
            try
            {
                Mark mark = null;
                if (marks.Count > 0)
                {
                    int which = marks.Count - 1;
                    // find the last mark with the specified name:
                    while (which >= 0 && mark == null)
                    {
                        Mark aux = marks[which];
                        if (aux != null) if (aux.name == name)
                                mark = aux;
                        if (mark == null)
                            --which;
                    }
                    // mark=marks.Last(m => m.name == name);
                    if (mark != null)
                    {
                        // Remove all marks from the specified one (inclusively):
                        marks.RemoveRange(which, marks.Count - which);
                        ret = mark.Current;
                    }
                }
            }
            catch { }
            return ret;
        }



        /// <summary>Moves the current to the previous node, if that node exists.
        /// Only one step backwards is enabled. Information about Previous node is lost when this method is called.</summary>
        public XmlNode Back()
        {
            XmlNode ret = null;
            try
            {
                if ( /* Current != null && */ Previous != null)
                {
                    XmlNode prev = Current;
                    ret = Current = Previous;
                    // Remark: 
                    // Consider whether the statement below is appropriate! maybe it would be better if Previous is 
                    // automatically set to null after every Back() operation!
                    // According to current experience, things work well without the statement below
                    //if (prev!=null)  
                    //    Previous = prev;
                }
            }
            catch { }
            return ret;
        }


        /// <summary>Moves the current position to the specified node.</summary>
        public XmlNode MoveTo(XmlNode node)
        {
            XmlNode ret = null;
            try
            {
                if (node == null)
                    R.ReportError("XmlParser.MoveTo()", "Invalid node to jump to (null reference).");
                else if (!ContainedInDocument(node, Doc))
                {
                    node = null;
                    R.ReportError("XmlParser.MoveTo()", "Invalid node to jump to. Node name: " + node.Name);
                }
                else
                {
                    ret = node;
                    XmlNode prev = Current;
                    Current = ret;
                    Parent = Current.ParentNode;  // update parent information because the context may have been changed
                    if (prev != null && Current != prev)  // update previous only if the current node has changed and it was not null before
                        Previous = prev;
                }
            }
            catch { }
            return ret;
        }

        /// <summary>Moves the current position to the first node that satisfies the absolute path 
        /// specified as an XPath string.</summary>
        /// <param name="path">XPath string that specifies the node position relative to the document root.</param>
        /// <returns>The node to which the current position has moved, or null if not successful.</returns>
        public XmlNode MoveTo(string path)
        {
            XmlNode ret = null;
            try
            {
                try
                {
                    ret = GetNode(path);
                }
                catch{  }
                if (ret != null)
                    ret = MoveTo(ret);
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>Moves the current position to the first node that satisfies the relative path 
        /// specified as an XPath string relative to the current node.</summary>
        /// <param name="path">XPath string that specifies the node position relative to the current node.</param>
        /// <returns>The node to which the current position has moved, or null if not successful.</returns>
        public XmlNode MoveRelative(string path)
        {
            XmlNode ret = null;
            try
            {
                try
                {
                    ret = GetRelative(path);
                }
                catch{  }
                if (ret != null)
                    ret = MoveTo(ret);
            }
            catch
            {
            }
            return ret;
        }


        /// <summary>Sets the current node to the parent node of the current node and returns it.
        /// This also works if the Current node is null because the advancing fell out of range or because
        /// StepIn was executed but there were no child nodes of the current node.</summary>
        /// <returns>The current node.</returns>
        public XmlNode GoToParent()
        {
            XmlNode ret = null;
            try
            {
                XmlNode newparent = Parent.ParentNode;
                XmlNode prev = Current;
                ret = Current = Parent;  // current becomes the current parent node;
                if (Current != null && prev != null) // update previous if current has changed and current was not null
                    Previous = prev;
                Parent = newparent;
            }
            catch { }
            return ret;
        }

        /// <summary>Sets the current node to the root node of the current document and returns it.</summary>
        /// <returns>The current node.</returns>
        public XmlNode GoToRoot()
        {
            XmlNode ret = null;
            try
            {
                XmlNode prev = Current;
                ret = Current = Root;  // current becomes the current parent node;
                if (Current != null && prev != null) // update previous if current has changed and current was not null
                    Previous = prev;
            }
            catch { }
            return ret;
        }

        /// <summary>Sets the current node to the root node of the current document and returns it.</summary>
        /// <returns>The current node.</returns>
        public XmlNode GoToDocument()
        {
            XmlNode ret = null;
            try
            {
                XmlNode prev = Current;
                ret = Current = Doc;  // current becomes the current parent node;
                if (Current != null && prev != null) // update previous if current has changed and current was not null
                    Previous = prev;
            }
            catch { }
            return ret;
        }

        /// <summary>Moves the current node to its first child node and returns the node.</summary>
        public XmlNode StepIn()
        {
            XmlNode ret = null;
            try
            {
                if (Current != null)
                {
                    _parent = Current;
                    XmlNode prev=Current;
                    ret = Current = Current.FirstChild;
                    if (Current != null && prev !=null) // update previous if current has changed and current was not null
                        Previous = prev;
                }
            }
            catch { }
            return ret;
        }

        /// <summary>Steps out of the current childnodes context and selects the next node of the parent as the current node.
        /// This also works if the Current node is null because the advancing fell out of range or because
        /// StepIn was executed but there were no child nodes of the current node.</summary>
        /// <returns></returns>
        public XmlNode StepOut()
        {
            XmlNode ret = null;
            try
            {
                XmlNode newparent = Parent.ParentNode;
                XmlNode prev = Current;
                ret = Current = Parent;  // current becomes the current parent node;
                if (Current != null && prev != null) // update previous if current has changed and current was not null
                    Previous = prev;
                Parent = newparent;
            }
            catch { }
            return ret;
        }


        /// <summary>Sets the current node to the next sibling node, or to null if the next sibling node does not
        /// exist, and returns that node.</summary>
        public XmlNode NextNode()
        {
            XmlNode ret = null;
            try
            {
                if (Current != null)
                {
                    XmlNode prev = Current;
                    ret = Current = Current.NextSibling;  // current becomes the next sibling node of current;
                    if (Current != null && prev != null) // update previous if current has changed and current was not null
                        Previous = prev;
                }
            }
            catch { }
            return ret;
        }

        /// <summary>Moves the current node to the first sibling node of the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        /// <param name="IncludeCurrent">If true then the new current node can also be the current node
        /// (if it satisfies the conditions), otherwise the search starts at the next sibling of the current node.</param>
        /// <returns></returns>
        public XmlNode NextNodeConditional(XmlNodeType NodeType, string NodeName, string NodeValue, bool IncludeCurrent)
        {

            XmlNode ret = null;
            try
            {
                if (Current != null)
                {
                    XmlNode prev = Current;
                    // Move current to the node that is found, or set it to null if the node is not found
                    // (meaning that the node falls out of the child nodes collection):
                    ret = Current = GetNextNode(Current /* StartingNode */ , 
                            NodeType, NodeName, NodeValue, IncludeCurrent);
                    if ( /* Current != null && */ Current!= prev && prev != null) // update previous if current has changed and current was not null
                        Previous = prev;
                }
            }
            catch { }
            return ret;

            //XmlNode ret = null;
            //try
            //{
            //    if (Current != null)
            //    {
            //        bool stop = false;
            //        if (!IncludeCurrent)
            //            NextNode();
            //        while (!stop)
            //        {
            //            if (Current == null)
            //                stop = true;
            //            else
            //            {
            //                stop = true;
            //                if (NodeType != XmlNodeType.None) // type should be considered
            //                    if (Current.NodeType != NodeType)
            //                        stop = false;
            //                if (NodeName != null)  // Node name should also be considered
            //                    if (Current.LocalName != NodeName)
            //                        stop = false;
            //                if (NodeValue != null)
            //                    if (Current.Value != NodeValue)
            //                        stop = false;
            //            }
            //            if (!stop)
            //                NextNode();
            //        }
            //        ret = Current;
            //    }
            //}
            //catch { }
            //return ret;
        }


        // Changing the current node to the next OR the current node that satisfies given contitions:

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode NextNode(XmlNodeType NodeType, string NodeName, string NodeValue)
        {
            return NextNodeConditional(NodeType, NodeName, NodeValue, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode NextNode(XmlNodeType NodeType, string NodeName)
        {
            return NextNodeConditional(NodeType, NodeName, null /* NodeValue */, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that 
        /// is of the specified type.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        public XmlNode NextNode(XmlNodeType NodeType)
        {
            return NextNodeConditional(NodeType, null /* NodeName */, null /* NodeValue */, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node. Node type is not important.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode NextNode(string NodeName, string NodeValue)
        {
            return NextNodeConditional(XmlNodeType.None /* NodeType */, NodeName, NodeValue, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node. Node type is not important.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode NextNode(string NodeName)
        {
            return NextNodeConditional(XmlNodeType.None /* NodeType */, NodeName, null /* NodeValue */, false /* IncludeCurrent */);
        }


        // Changing the current node to the next OR the current node that satisfies given contitions:

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode NextOrCurrentNode(XmlNodeType NodeType, string NodeName, string NodeValue)
        {
            return NextNodeConditional(NodeType, NodeName, NodeValue, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode NextOrCurrentNode(XmlNodeType NodeType, string NodeName)
        {
            return NextNodeConditional(NodeType, NodeName, null /* NodeValue */, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that 
        /// is of the specified type.</summary>
        /// <param name="NodeType">If not None then the current node must have this type.</param>
        public XmlNode NextOrCurrentNode(XmlNodeType NodeType)
        {
            return NextNodeConditional(NodeType, null /* NodeName */, null /* NodeValue */, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node. Node type is not important.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode NextOrCurrentNode(string NodeName, string NodeValue)
        {
            return NextNodeConditional(XmlNodeType.None /* NodeType */, NodeName, NodeValue, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node. Node type is not important.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode NextOrCurrentNode(string NodeName)
        {
            return NextNodeConditional(XmlNodeType.None /* NodeType */, NodeName, null /* NodeValue */, true /* IncludeCurrent */);
        }

        // These two groups of functions that only cosider element nodes:

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode NextElement(string NodeName, string NodeValue)
        {
            return NextNodeConditional(XmlNodeType.Element /* NodeType */, NodeName, NodeValue, false /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode NextElement(string NodeName)
        {
            return NextNodeConditional(XmlNodeType.Element /* NodeType */, NodeName, null /* NodeValue */, false /* IncludeCurrent */);
        }


        /// <summary>Moves the current node to the first sibling element after the current node.</summary>
        public XmlNode NextElement()
        {
            return NextNodeConditional(XmlNodeType.Element /* NodeType */, null /* NodeName */, null /* NodeValue */, false /* IncludeCurrent */);
        }


        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        /// <param name="NodeValue">If not null then the current node muust have this value.</param>
        public XmlNode NextOrCurrentElement(string NodeName, string NodeValue)
        {
            return NextNodeConditional(XmlNodeType.Element /* NodeType */, NodeName, NodeValue, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling node after the current node that satisfies the specified conditions, 
        /// and returns the current node.</summary>
        /// <param name="NodeName">If not null then the current node must have this name.</param>
        public XmlNode NextOrCurrentElement(string NodeName)
        {
            return NextNodeConditional(XmlNodeType.Element /* NodeType */, NodeName, null /* NodeValue */, true /* IncludeCurrent */);
        }

        /// <summary>Moves the current node to the current or the first sibling element after the current node.</summary>
        public XmlNode NextOrCurrentElement()
        {
            return NextNodeConditional(XmlNodeType.Element /* NodeType */, null /* NodeName */, null /* NodeValue */, true /* IncludeCurrent */);
        }


        #endregion  // Traversing

        // Stack<XmlNode> nextchild = new Stack<XmlNode>();

    }  // class XmlParser


    /// <summary>Class that enables custom parsing and building of an Xml document.</summary>
    public class XmlBuilder : XmlParser
    // $A Aug08 Oct08 Mar09;
    {


        #region DocumentCreation

        private XmlDocument NewXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            return doc;
        }

        private XmlDocument NewXmlDocument(string RootName)
        {
            XmlDocument doc = NewXmlDocument();
            doc.AppendChild(doc.CreateElement(RootName));
            return doc;
        }

        /// <summary>Creates a new Xml document.</summary>
        public void NewDocument()
        {
            Doc = NewXmlDocument();
        }

        /// <summary>Creates a new Xml document with a specified name of hte root element.</summary>
        /// <param name="OoRootName">Name of the root element.</param>
        public void NewDocument(string RootName)
        {
            Doc = NewXmlDocument(RootName);
        }

        /// <summary>Adds a processing instruction to the Xml document.</summary>
        /// <param name="target">Name of the processing instruction.</param>
        /// <param name="data">data for the processing instruction.</param>
        public void AddProcessingInstruction(string target, string data)
        {
            XmlDocument doc = Doc;
            if (doc == null)
                throw new XmlException("Document is not initialized, ca not add a processing instruction.");
            else
            {
                XmlProcessingInstruction instr;
                instr = doc.CreateProcessingInstruction(target, data);
                if (instr == null)
                    throw new XmlException("Could not create an XML processing instruction. Target: " + target
                        + ", data: " + data);
                else
                {
                    if (Root != null)
                        doc.InsertBefore(instr, Root);
                    else
                        doc.AppendChild(instr);
                }
            }
        }


        #endregion  // DocumentCreation


        #region TreatedNode

        // Definition of the node that is currently treated:

        protected XmlNode _new = null, _newest=null;

        /// <summary>Returns the Xml node that is currently treaded.
        /// All modifications apply to this node.
        /// If a newly created node exists that has not been inserted into the document yet then this node is returned.
        /// Otherwise, the current node is returned.</summary>
        public XmlNode Treated
        {
            get
            {
                if (_new != null)
                    return _new;
                else
                    return Current;
            }
        }

        /// <summary>Returns Treated if this node is Xml element, or null if it is not or if treated is null.</summary>
        public XmlElement TreatedElement
        {
            get { return Treated as XmlElement; }
        }

        /// <summary>Gets or sets the newly created node (if one exists).
        /// When the newly created node is inserted into the document, it is not accessible any more through
        /// New or Treated properties.
        /// If set to a node from another document, a deep copy is created and imported.
        /// If set to a node from the current document that is already included in the document, a deep copy is created and set.
        /// If set to a node from the current document that is not included in the document, a reference to the node is set.</summary>
        public XmlNode New
        { 
            get { return _new; }
            set
            {
                try
                {
                    if (value==null)
                        _new = value;
                    else
                    {
                        if (_new != null)
                            throw new Exception("There already exists a new node.");
                        if (value.OwnerDocument == Doc)
                        {
                            if (ContainedInDocument(value, Doc))
                                _new = value.CloneNode(true /* deep */);
                            else
                                _new = value;
                        } else
                        {
                            _new = Doc.ImportNode(value, true /* deep copy */ );
                        }
                    }
                }
                catch (Exception ex)
                {
                    R.ReportError(ex);
                }
            }
        }

        /// <summary>Returns New if it is an Xml Element, or null if it is not.</summary>
        public XmlElement NewElement
        { get { return New as XmlElement; } }




        /// <summary>Gets the last created node (if one exists).
        /// This is the last node that has been created anew and already inserted into existing structure.</summary>
        public XmlNode Newest
        { 
            get { return _newest; }
            protected set
            {
                _newest = value;
            }
        }


        /// <summary>Moves the current position to the specified node.</summary>
        public XmlNode MoveToNewest()
        {
            XmlNode ret = null;
            try
            {
                XmlNode which = Newest;
                if (which == null)
                    R.ReportError("XmlParser.MoveToNewest()", "Newest inserted node is not defined. "
                        + Environment.NewLine + "Either the node has been removed or there were no nodes created and inserted.");
                else
                    ret = MoveTo(which);
            }
            catch { }
            return ret;
        }
        



        /// <summary>Discards the newly created node and returns it if it exists.</summary>
        /// <returns>The discarded Xml node or null if there was no newly created node.</returns>
        public XmlNode Discard()
        {
            XmlNode ret = New;
            New = null;
            return ret;
        }



        /// <summary>Sets a named attribute of the currently treated node to a specific value.
        /// This only works if the currently treated node is an XML element.
        /// If an attribute with a specified name dose not yet exist then it is created,
        /// otherwise old value is overwritten.</summary>
        /// <param name="name">Name of the attribute to be set.</param>
        /// <param name="value">Value of the attribute.</param>
        /// <returns>The node for which the attribute has been set, or null if the attribute could not be set.</returns>
        public XmlNode SetAttribute(string name, string value)
        {
            XmlNode ret=null;
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("The attribute name is not specified properly.");
                else if (TreatedElement == null)
                    throw new XmlException("The currently treated node is null or is not an element.");
                else
                {
                    TreatedElement.SetAttribute(name,value);
                    ret = TreatedElement;
                }
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
            }
            return ret;
        }

        /// <summary>Removes the attribute with a specified name from the currently treated XML node, and 
        /// returns  this node or null if the operation was not successful (e.g. if the treated node is not
        /// defined or if the attribute with a specified name does not exist).
        /// This function does not report any errors.</summary>
        /// <param name="name">Name of the attribute to be removed.</param>
        /// <returns>The node on which atttribute is removed (which is the currently treated node), or 
        /// null if the operation could not be performed (this function does not report any errors).</returns>
        public XmlNode RemoveAttribute(string name)
        {
            XmlNode ret=null;
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("The attribute name is not specified properly.");
                else if (TreatedElement == null)
                    throw new XmlException("The currently treated node is not specified or is not an element.");
                else
                {
                    TreatedElement.RemoveAttribute(name);
                    ret = TreatedElement;
                }
            }
            catch { }
            return ret;
        }


        /// <summary>Sets value of the currently treated XML node.
        /// If the currently treated node is a Text node then value is set directly.
        /// If the currently treated node is an element then value is set on its first chilld Text node
        /// if such a node exists. If the element has no children, a Text child node is created.</summary>
        public XmlNode SetValue(string value)
        {
            XmlNode ret=null;
            try
            {
                ret = Treated;
                if (ret == null)
                    throw new XmlException("Could not set a value: the current node is null.");
                if (ret.NodeType == XmlNodeType.Element)
                {
                    XmlNode nd = null;
                    if (ret.HasChildNodes)
                    {
                        // try to find the first Text child node and set value to that node
                        nd = ret.FirstChild;
                        while (nd != null && nd.NodeType != XmlNodeType.Text)
                            nd = nd.NextSibling;
                        if (nd != null)
                            nd.Value = value;
                    } 
                    // no Text child node was found, ve therefore just create one:
                    if (nd==null)
                    {
                        nd = ret.PrependChild(Doc.CreateTextNode(value));
                    }

                } else
                    Treated.Value = value;
                
               //{
               //     Treated.Value = value;
               //     ret = Current;
               //     // Remark:
               //     // Check if this implementation is sufficient. Possibly there would be a need to 
               //     // treat different nodes differently. Current experience did not show any problems with
               //     // this implementation, however.
               // }
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                ret = null;
            }
            return ret;
        }

        /// <summary>Sets inner text of the currently treated xml node.</summary>
        public XmlNode SetInnerText(string value)
        {
            XmlNode ret = null;
            try
            {
                ret = Treated;
                if (Treated == null)
                    throw new XmlException("Could not set a value: the current node is null.");
                Treated.InnerText = value;
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
            }
            return ret;
        }

        /// <summary>Sets inner xml of the currently treated xml node.</summary>
        public XmlNode SetInnerXml(string value)
        {
            XmlNode ret = null;
            try
            {
                ret = Treated;
                if (Treated == null)
                    throw new XmlException("Could not set a value: the current node is null.");
                Treated.InnerXml = value;
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
            }
            return ret;
        }

        #endregion  // TreatedNode


        #region NodeRemoval
        
        /// <summary>Removes the specified node from the current XML document and returns that node.
        /// null is returned if the node can not be removed. 
        /// CAUTION: 
        /// Removing the node may destroy information about previous node and thus render the Back() operation impossible.
        /// It may also affect marks, which is not checked by any function but the BackMark() following removal may fail.</summary>
        /// <param name="node">XML node to be removed.</param>
        public XmlNode RemoveNode(XmlNode node)
        {
            XmlNode ret = null;
            try
            {
                if (node == null)
                    throw new ArgumentNullException("Can not remove an XML node: node is null.");
                else
                {
                    if (node==Current)
                    {
                        // The node to be removed is the current node, which nead special treatment.
                        // We move to the next node and take care of Previous, then remove the node:
                        XmlNode prev = Previous;
                        // Move to the next node
                        NextNode();
                        if (!ContainedInNode(prev, node))
                            Previous = prev;
                        else
                        {
                            Previous = Current; 
                            if (Previous == null && Current == null)
                            {
                                // It is possible here that both are null, in this case perform a step out.
                                MoveTo(Parent);
                            }
                        }
                        XmlNode parentnode = node.ParentNode;
                        parentnode.RemoveChild(node);
                        ret = node;
                    } else if (ContainedInNode(Current,node))
                    {
                        // The node to be removed is not the current node, but it contains the current node.
                        // To remove such a node, the current node must be first set to that node
                        // and then the node can be removed.
                        MoveTo(node);
                        ret = RemoveNode(node);
                    } else if (ContainedInNode(Previous,node))
                    {
                        // In this case the information on previous node willl be lost:
                        Previous = Current;
                        ret = RemoveNode(node);
                    } else if (node != Current && node != Previous)
                    {
                        // There are no conflicts, the node can be simply deleted:
                        XmlNode parentnode = node.ParentNode;
                        parentnode.RemoveChild(node);
                        ret = node;
                    } else
                    {
                        throw new Exception("The node could not be deleted due to an unknown reason. Node name: " + node.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
            }
            return ret;
        }

        /// <summary>Removes the current node and returns the removed node or null if the operation does not succeed.
        /// The current node is moved to the next node and may become null if the current node was 
        /// the last one in the given child list.</summary>
        public XmlNode RemoveCurrent()
        {
            return RemoveNode(Current);
        }

        #endregion  // NodeRemoval



        #region NodeCreation


        /// <summary>Creates an exact copy of the specified XML node and sets it as the new (currently treated) node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <param name="modelnode">The model node that is cloned.</param>
        /// <returns>The newly created clone of the model node.</returns>
        public XmlNode CreateClone(XmlNode modelnode)
        {
            XmlNode ret = null;
            try
            {
                if (New != null)
                    throw new Exception("There already exists the newly created node.");
                if (modelnode.OwnerDocument == Doc)
                {
                    ret = _new = modelnode.CloneNode(true /* deep */);
                } else
                {
                    ret = _new = Doc.ImportNode(modelnode, true /* deep copy */ );
                }

                //_new = Doc.CreateNode(type, name, namespaceURI);
                //ret = _new;
                //if (!string.IsNullOrEmpty(value))
                //    _new.InnerText = value;
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                ret = null;
            }
            return ret;
        }

        /// <summary>Creates an exact copy of the Current XML node and sets it as the new (currently treated) node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <returns>The newly created clone of the current node.</returns>
        public XmlNode CreateClone()
        {
            return CreateClone(Current);
        }



        /// <summary>Creates a new node and sets this node as treated node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <param name="type">Type of the node.</param>
        /// <param name="name">Name of the node.</param>
        /// <param name="value">Value of the node.</param>
        /// <param name="namespaceURI">Namespace URI of the node.</param>
        /// <returns>The newly created node.</returns>
        public XmlNode CreateNode(XmlNodeType type, string name, string value, string namespaceURI)
        {
            XmlNode ret = null;
            try
            {
                if (New != null)
                    throw new Exception("There already exists the newly created node.");
                _new = Doc.CreateNode(type, name, namespaceURI);
                ret = _new;
                if (!string.IsNullOrEmpty(value))
                    _new.InnerText = value;
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                ret = null;
            }
            return ret;
        }

        /// <summary>Creates a new node and sets this node as treated node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <param name="type">Type of the node.</param>
        /// <param name="name">Name of the node.</param>
        /// <param name="value">Value of the node.</param>
        /// <returns>The newly created node.</returns>
        public XmlNode CreateNode(XmlNodeType type, string name, string value)
        {
            return CreateNode(type, name, value, null /* namespaceURI */ );
        }

        /// <summary>Creates a new node and sets this node as treated node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <param name="type">Type of the node.</param>
        /// <param name="name">Name of the node.</param>
        /// <returns>The newly created node.</returns>
        public XmlNode CreateNode(XmlNodeType type, string name)
        {
            return CreateNode(type, name, null /* value */, null /* namespaceURI */ );
        }
        
        // Create a new element:


        /// <summary>Creates a new Xml element and sets this element as treated node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <param name="name">Name of the node.</param>
        /// <param name="value">Value of the node.</param>
        /// <param name="namespaceURI">Namespace URI of the node.</param>
        /// <returns>The newly created node.</returns>
        public XmlNode CreateElement(string name, string value, string namespaceURI)
        {
            return CreateNode(XmlNodeType.Element, name, value, namespaceURI);
        }

        /// <summary>Creates a new Xml element and sets this element as treated node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <param name="name">Name of the node.</param>
        /// <param name="value">Value of the node.</param>
        /// <returns>The newly created node.</returns>
        public XmlNode CreateElement(string name, string value)
        {
            return CreateNode(XmlNodeType.Element, name, value, null /* namespaceURI */);
        }

        /// <summary>Creates a new Xml element and sets this element as treated node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <param name="name">Name of the node.</param>
        /// <returns>The newly created node.</returns>
        public XmlNode CreateElement(string name)
        {
            return CreateNode(XmlNodeType.Element, name, null /* value */, null /* namespaceURI */);
        }

        // Create a new comment: 


        /// <summary>Creates a new Xml comment and sets this comment as treated node.
        /// If there already exists a newly created node then it must be unassigned first (e.g. by Discard()), 
        /// otherwise this method fails.
        /// The new node also gets unassigned when it is inserted into the document.</summary>
        /// <param name="name">Name of the node.</param>
        /// <param name="value">Value of the node.</param>
        /// <returns>The newly created node.</returns>
        public XmlNode CreateComment(string value)
        {
            return CreateNode(XmlNodeType.Comment, null /* name */, value, null /* namespaceURI */);
        }

        #endregion  // NodeCreation



        #region NodePlacement



        /// <summary>Inserts the newly created node before the current node.
        /// The new node is set to null if the operation is successful (otherwise it remains unchanged).</summary>
        /// <returns>The newly created node if operation is successful, null otherwise.</returns>
        public XmlNode InsertBefore()
        {
            XmlNode ret = null;
            try
            {
                if (New == null)
                    throw new Exception("There is no newly created Xml node." + Environment.NewLine +
                            "  Maybe the node has already been inserted into document or it has been discarded.");
                Newest = New;
                if (Current != null)
                {
                    ret = Current.ParentNode.InsertBefore(New, Current);
                    if (ret != null)
                        New = null;  // the new node has been inserted, set it to null
                }
                else if (Parent != null)
                {
                    // If Current is not defined but Parent is, append the node to the end of the parent's child
                    // nodes. This is because the current has obviously run out of range of the children table.
                    ret = Parent.AppendChild(New);
                    if (ret != null)
                        New = null;  // the new node has been inserted, set it to null
                }
                else
                    throw new Exception("Could not insert the newly created node before the current node or at the end of the current context.");
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                ret = null;
            }
            New = null;
            return ret;
        }


        /// <summary>Inserts the newly created node after the current node.
        /// The new node is set to null if the operation is successful (otherwise it remains unchanged).</summary>
        /// <returns>The newly created node if operation is successful, null otherwise.</returns>
        public XmlNode InsertAfter()
        {
            XmlNode ret = null;
            try
            {
                if (New == null)
                    throw new Exception("There is no newly created Xml node." + Environment.NewLine +
                            "  Maybe the node has already been inserted into document or it has been discarded.");
                Newest = New;
                if (Current != null)
                {
                    ret = Current.ParentNode.InsertAfter(New, Current);
                    if (ret != null)
                        New = null;  // the new node has been inserted, set it to null
                }
                else if (Parent != null)
                {
                    // If Current is not defined but Parent is, append the node to the end of the parent's child
                    // nodes. This is because the current has obviously run out of range of the children table.
                    ret = Parent.AppendChild(New);
                    if (ret != null)
                        New = null;  // the new node has been inserted, set it to null
                }
                else
                    throw new Exception("Could not insert the newly created node after the current node or at the end of the current context.");
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                ret = null;
            }
            New = null;
            return ret;
        }

        /// <summary>Appends the newly created node after the last child of the current node.
        /// The new node is set to null if the operation is successful (otherwise it remains unchanged).</summary>
        /// <returns>The newly created node if operation is successful, null otherwise.</returns>
        public XmlNode AppendChild()
        {
            XmlNode ret = null;
            try
            {
                if (New == null)
                    throw new Exception("There is no newly created Xml node." + Environment.NewLine +
                            "  Maybe the node has already been inserted into document or it has been discarded.");
                if (Current == null)
                    throw new Exception("The current node is not defined, can not insert the newly created node as its child.");
                Newest = New;
                if (Current != null)
                {
                    ret = Current.AppendChild(New);
                    if (ret != null)
                        New = null;  // the new node has been inserted, set it to null
                    else throw new Exception("Child insertion failed. Node name: " + Current.Name + ".");
                }
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                ret = null;
            }
            New = null;
            return ret;
        }

        /// <summary>Inserts the newly created node before the first child of the current node.
        /// The new node is set to null if the operation is successful (otherwise it remains unchanged).</summary>
        /// <returns>The newly created node if operation is successful, null otherwise.</returns>
        public XmlNode PrependChild()
        {
            XmlNode ret = null;
            try
            {
                if (New == null)
                    throw new Exception("There is no newly created Xml node." + Environment.NewLine +
                            "  Maybe the node has already been inserted into document or it has been discarded.");
                if (Current == null)
                    throw new Exception("The current node is not defined, can not insert the newly created node as its child.");
                Newest = New;
                if (Current != null)
                {
                    ret = Current.PrependChild(New);
                    if (ret != null)
                        New = null;  // the new node has been inserted, set it to null
                    else throw new Exception("Child insertion failed. Node name: " + Current.Name + ".");
                }
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                ret = null;
            }
            New = null;
            return ret;
        }

        #endregion  // NodePlacement



    }  // class XmlBuilder






    /// <summary>Base class for data classes that support writing data to / reading from custom XML files,
    /// writing from / to database objects, etc.</summary>
    public abstract class ParsableXmlObject 
    {

        public string
            XmlString = null;  // Complete msg as obtained form the web service




        IReporter _R = null;

        /// <summary>Reporter for this class.</summary>
        public virtual IReporter R
        {
            get
            {

                // TODO: Change this!

                if (_R == null)
                    _R = new ReporterConsole();
                return _R;
            }
        }

        protected XmlBuilder Data = new XmlBuilder();

        /// <summary>Returns the XML document representing the msg.</summary>
        public XmlDocument Doc
        {
            get
            {
                XmlDocument ret = null;
                if (Data == null)
                    ret = null;
                else
                    ret = Data.Doc;
                if (ret == null && !string.IsNullOrEmpty(XmlString))
                {
                    ret = new XmlDocument();
                    ret.LoadXml(XmlString);
                }
                return ret;
            }
        }

        /// <summary>Saves contents of the msg to an xml file.</summary>
        /// <param name="filename">Name of the file that the msg is saved to.</param>
        public virtual void Save(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("File to save message in is not specified (null reference).");
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("File to save message in is not specified (empty string).");
            XmlDocument doc = Doc;
            if (doc == null)
                throw new Exception("Message does not have any contents. Maybe CreateXml() should be executed before this call.");
            doc.Save(filename);
        }

        /// <summary>Creates msg Xml from the current msg data and stores it in this object.</summary>
        public virtual void CreateXml()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.CreateXml()", "Started...");
            try
            {
                if (Data == null)
                    Data = new XmlBuilder();
                XmlDocument doc = ToXml();
                Data.LoadXml(doc);
            }
            catch (Exception ex)
            {
                R.ReportError(ex, "MsgObervationOrder.CreateXml");
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.CreateXml()", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>Loads msg data from a file containing the XML msg.</summary>
        /// <param name="filename">Name of the file from which tata is read.</param>
        public virtual void Load(string filename)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Load()", "Started...");
            try
            {
                if (Data == null)
                    Data = new XmlBuilder();
                Data.Load(filename);
            }
            catch (Exception ex)
            {
                R.ReportError(ex, "MsgObervationOrder.Load");
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Load()", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>Loads msg data from a string containing the XML msg.</summary>
        /// <param name="doc">String containing the msg in XML format.</param>
        public virtual void LoadXml(string doc)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.LoadXml()", "Started...");
            try
            {
                if (Data == null)
                    Data = new XmlBuilder();
                Data.LoadXml(doc);
            }
            catch (Exception ex)
            {
                R.ReportError(ex, "MsgObervationOrder.LoadXml");
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.LoadXml()", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Loads msg data from an XmlDocument containing XML msg.</summary>
        /// <param name="filename">XMLDocument containing the msg.</param>
        public virtual void LoadXml(XmlDocument doc)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.LoadXml()", "Started...");
            try
            {
                if (Data == null)
                    Data = new XmlBuilder();
                Data.LoadXml(doc);
            }
            catch (Exception ex)
            {
                R.ReportError(ex, "MsgObervationOrder.LoadXml");
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.LoadXml()", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>Reads msg data from the internal XML document containing the msg.</summary>
        public abstract void Read();

        /// <summary>Read msg data from an XML document containing the msg.</summary>
        /// <param name="doc">Document containing the msg.</param>
        public abstract void Read(XmlParser data /* , PAT_bis_classes.clsMsgOrder order */ );



        /// <summary>Class used for accummulated error and warning reports for task composed of several steps.
        /// This is used when we want to perform the whole task even if some errors occur, and treat all errors
        /// and messages after the task is completed.</summary>
        public class AccummulatedReport
        {

            private AccummulatedReport()
            {
            }

            /// <summary>Constructor.</summary>
            /// <param name="rep">Reporter object used for launching reports.</param>
            /// <param name="throwExceptionOnErrors">If true then reporting methods will throw an exception when report contains errors.
            /// It is more common practice not to use tehse methods and launch reports & throw exceptions explicitly.</param>
            public AccummulatedReport(IReporter rep, string location, bool throwExceptionOnErrors)
            {
                R = rep;
                Location = location;
                ThrowExceptionOnErrors = throwExceptionOnErrors;
            }

            private List<string>
                _errorStrings = null,
                _warningStrings = null,
                _infoStrings = null;


            /// <summary>Get number of errors accummulated on the report.</summary>
            public int NumErrors { get { if (_errorStrings==null) return 0; else return _errorStrings.Count; } }

            /// <summary>Get number of warnings accummulated on the report.</summary>
            public int NumWarnings { get { if (_warningStrings == null) return 0; else return _warningStrings.Count; } }

            /// <summary>Get number of infos accummulated on the report.</summary>
            public int NumInfos { get { if (_infoStrings == null) return 0; else return _infoStrings.Count; } }



            private IReporter _rep = new ReporterConsole();

            /// <summary>If true then  exception is thrown in the case of errors when reporting is launched.</summary>
            public bool ThrowExceptionOnErrors = true;

            /// <summary>Gets or sets the reporter used for reporting (if any).</summary>
            public IReporter R
            {
                get { return _rep; }
                set { _rep = value; }
            }

            /// <summary>Description of location where the accummulated repord is generated.</summary>
            public string Location = null;

            /// <summary>Resets the report (remove all erorrs, warnings and info messages)</summary>
            public virtual void Reset()
            {
                _errorStrings = _warningStrings = _infoStrings = null;
            }


            /// <summary>Adds a new error message to the accummulated report.</summary>
            /// <param name="message">Error message.</param>
            public void AddError(string message)
            {
                if (_errorStrings==null)
                    _errorStrings = new List<string>();
                _errorStrings.Add(message);
            }

            /// <summary>Adds a new warning message to the accummulated report.</summary>
            /// <param name="message">Warning message.</param>
            public void AddWarning(string message)
            {
                if (_warningStrings==null)
                    _warningStrings = new List<string>();
                _warningStrings.Add(message);
            }

            /// <summary>Adds a new info message to the accummulated report.</summary>
            /// <param name="message">Info message.</param>
            public void AddInfo(string message)
            {
                if (_infoStrings==null)
                    _infoStrings = new List<string>();
                _infoStrings.Add(message);
            }


            /// <summary>Concatenated string containing all info strings that exist on this class.</summary>
            public virtual string ErrorString
            {
                get
                {
                    string ret = null;
                    if (_errorStrings!=null)
                        if (_errorStrings.Count > 0)
                        {
                            for (int i = 0; i < _errorStrings.Count; ++i)
                            {
                                ret += _errorStrings[i] + Environment.NewLine;
                            }
                        }
                    return ret;
                }
            }

            /// <summary>Concatenated string containing all warning strings that exist on this class.</summary>
            public virtual string WarningString
            {
                get
                {
                    string ret = null;
                    if (_warningStrings != null)
                        if (_warningStrings.Count > 0)
                        {
                            for (int i = 0; i < _warningStrings.Count; ++i)
                            {
                                ret += _warningStrings[i] + Environment.NewLine;
                            }
                        }
                    return ret;
                }
            }

            /// <summary>Concatenated string containing all error strings that exist on this class.</summary>
            public virtual string InfoString
            {
                get
                {
                    string ret = null;
                    if (_infoStrings!=null)
                        if (_infoStrings.Count > 0)
                        {
                            for (int i = 0; i < _infoStrings.Count; ++i)
                            {
                                ret += _infoStrings[i] + Environment.NewLine;
                            }
                        }
                    return ret;
                }
            }


            public virtual string ErrorAndWarningString
            {
                get
                {
                    string ret = null;
                    if (NumErrors > 0)
                    {
                        ret += "  Errors: " + Environment.NewLine + ErrorString;
                        if (NumWarnings > 0)
                            ret += Environment.NewLine;
                    }
                    if (NumWarnings > 0)
                    {
                        ret += "  Warnings: " + Environment.NewLine + WarningString;
                    }
                    return ret;
                }
            }

            /// <summary>Launches the accummulated report.</summary>
            /// <param name="place"></param>
            /// <param name="rep"></param>
            public virtual void Report()
            {
                if (R != null)
                {
                    if (NumInfos>0)
                    {
                        if (!string.IsNullOrEmpty(Location))
                            R.ReportInfo(Location, InfoString);
                        else
                            R.ReportInfo(InfoString);
                    }
                    if (NumWarnings>0)
                    {
                        if (!string.IsNullOrEmpty(Location))
                            R.ReportWarning(Location, WarningString);
                        else
                            R.ReportInfo(WarningString);
                    }
                    if (NumErrors>0)
                    {
                        if (!string.IsNullOrEmpty(Location))
                            R.ReportWarning(Location, ErrorString);
                        else
                            R.ReportInfo(WarningString);
                    }
                }
                if (ThrowExceptionOnErrors && NumErrors > 0)
                    throw new Exception(ErrorAndWarningString);
            }
        }  // class AccummulatedReport


        /// <summary>Empty data consistency check, intended to be overridden in derived classes.</summary>
        /// <param name="rep">Object where error, warning and info reports are accumulated.</param>
        public virtual void CheckConsistency(AccummulatedReport rep)
        {
            if (rep != null)
            {
                rep.AddInfo("Data consistency check has not been performed.");
            }
        }



        /// <summary>Writes contents of the msg in a human readable form to a textwriter.</summary>
        private void Write(TextWriter tw)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Write(TextWriter)", "Started...");
            try
            {
                if (tw == null)
                    throw new Exception("Output stream to write data to is not specified (null reference).");
                string str = this.ToString();
                tw.Write(str);
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Write(TextWriter)", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>Writes cntents of the msg in a human readable form to the system console.</summary>
        public virtual void WriteToConsole()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Write()", "Started...");
            try
            {
                using (TextWriter tw = new StreamWriter(Console.OpenStandardOutput()))
                {
                    Write(tw);
                }
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Write()", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Writes cntents of the msg in a human readable form to a file.</summary>
        /// <param name="filename">File to which msg contents is written.</param>
        /// <param name="append">If true then fle is appended, otherwise it is overwritten.</param>
        public virtual void Write(string filename, bool append)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Write()", "Started...");
            try
            {
                using (TextWriter tw = new StreamWriter(filename, append))
                {
                    Write(tw);
                }
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Write()", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>Converts a msg to Xml and returns it.</summary>
        public abstract XmlDocument ToXml();



    }  // class ParsableXmlObject



    /* OPOMBE na uporabo razreda MsgObervationOrder in ostalih razredov za sporo;ila
      Razred je namenjen branju podatkov iz sporočil v XML in prepisu v bazo. Zaenkrat sta ti dve operaciji
    precej ločeni, razred MsgObervationOrder pokriva interpretacijo XML, za zapis v bazo pa poskrbi
    razred clsMsgOrder, vendar MsgObervationOrder ne zapiše podatkov direktno v ta razred, ampak najprej v
    svoje lokalne spremenljivke. To pomeni eno prepisovanje več, je pa razvoj bolj obladljiv, ker se lahko
    implementira interpretacija XML, ne da bi bilo potrebno zaradi razreda clsMsgOrder poganjati strežnik.
    */




    /// <summary>Base class for all messages.</summary>
    public abstract class MsgBase : ParsableXmlObject
    // $A Igor May09;
    {

        /// <summary>Gets objects that contains all constants related to communication messages.</summary>
        static protected MsgConst Const
        {
            get { return MsgConst.Const; }
        }

        /// <summary>Message type.
        /// Helps with general methods that operate on any datatype of msg.</summary>
        public MessageType Type = MessageType.Unknown;


        //private string _messageId = null;

        /// <summary>Message ID, obtained form the web service.</summary>
        public string MessageId = null;  // Message ID, obtained form the web service!

        /// <summary>Complete msg as obtained from the web service.</summary>
        public string MessageXml
        {
            get { return XmlString; }
            set { XmlString = value; }
        }

        /// <summary>Specifies the file where message has been backed up.
        /// Warning: this file may be deleted at any time!</summary>
        public string MessageFile = null;
         



        #region CommonData

        public string
            MessageNumber = null, MessageNumberOid = null, // msg ID as written in the msg
            MessageReceiver = null, MessageReceiverOid = null, // msg receiver
            MessageResponder = null, MessageResponderOid = null,  // msg responder
            MessageSender = null, MessageSenderOid = null;  // msg sender

        /// <summary>Sets msg data such that sender is Labex and receiver is Bis.</summary>
        public void SetSenderLabex()
        {
            MessageSender = MessageResponder = MsgConst.Const.LabexId;
            MessageReceiver = MsgConst.Const.BisId;
        }

        /// <summary>Sets msg data such that sender is Bis and receiver is Labex.</summary>
        public void SetSenderBis()
        {
            MessageSender = MessageResponder = MsgConst.Const.BisId;
            MessageReceiver = MsgConst.Const.LabexId;
        }

        #endregion  // CommonData


        #region MessageReceiver

        /// <summary>Sets the receiver of the communication msg stored in msg to the specified receiver,
        /// and indicates through an output argument whether the receiver has been changed in msg.
        /// If the receiver stated in the msg already equals to the specified one then nothing
        /// changes in the msg and the output indicator becomes false.
        /// WARNING: searching for receiver element is done through an XPath expression, which may
        /// fail due to default namespaces that were not detected.</summary>
        /// <param name="msg">Message in which receiver should be set.</param>
        /// <param name="receiver">The new receiver of the msg.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// receiver has been written to the msg).</param>
        public static void SetMessageReceiverOld(ref XmlDocument msg, string receiver, out bool changed)
        {
            changed = false;
            if (msg == null)
                throw new ArgumentNullException("Message document is not specified.");
            if (string.IsNullOrEmpty(receiver))
                throw new ArgumentException("Receiver of the message is not specified.");
            XmlNode rootnode = msg.DocumentElement;
            if (rootnode == null)
                throw new XmlException("XML document does not contain a root node.");
            if (rootnode.Name == Const.OoRootName)
            {
                XmlElement receiverelement = null;
                receiverelement = rootnode.SelectSingleNode(Const.ReceiverPath) as XmlElement;
                if (receiverelement == null)
                    throw new XmlException("Message document does not contain receiver node.");
                string currentreceiver = receiverelement.GetAttribute(Const.SenderReceiverAttribute);
                if (currentreceiver != receiver)
                {
                    changed = true;
                    receiverelement.SetAttribute(Const.SenderReceiverAttribute, receiver);
                }
            }
        }

        /// <summary>Sets the receiver of the communication msg stored in msg to the specified receiver,
        /// and indicates through an output argument whether the receiver has been changed in msg.
        /// If the receiver stated in the msg already equals to the specified one then nothing
        /// changes in the msg and the output indicator becomes false.</summary>
        /// <param name="msg">Message document in which receiver should be set.</param>
        /// <param name="receiver">The new receiver of the msg.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// receiver has been written to the msg).</param>
        public static void SetMessageReceiver(ref string message, string receiver, out bool changed)
        {
            changed = false;
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message string is not specified.");
            if (string.IsNullOrEmpty(receiver))
                throw new ArgumentException("Receiver of the message is not specified.");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(message);
            SetMessageReceiver(ref doc, receiver, out changed);
            if (changed)
            {
                message = doc.OuterXml;
            }
        }


        /// <summary>Sets the receiver of the communication msg stored in msg to the specified receiver,
        /// and indicates through an output argument whether the receiver has been changed in msg.
        /// If the receiver stated in the msg already equals to the specified one then nothing
        /// changes in the msg and the output indicator becomes false.</summary>
        /// <param name="msg">Message in which receiver should be set.</param>
        /// <param name="receiver">The new receiver of the msg.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// receiver has been written to the msg).</param>
        public static void SetMessageReceiver(ref XmlDocument msg, string receiver, out bool changed)
        {
            changed = false;
            if (msg == null)
                throw new ArgumentNullException("Message document is not specified.");
            XmlBuilder data = new XmlBuilder();
            data.LoadXml(msg);
            SetMessageReceiver(ref data, receiver, out changed);
        }


        // Auxiliary variables holding involved application IDs in lower case for easier comparision:
        protected static string
            LabexIdLowercase = null,
            BisIdLowercase = null;

        /// <summary>Sets the receiver of the communication msg stored in an Xml builder to the specified receiver,
        /// and indicates through an output argument whether the receiver has been changed in msg.
        /// If the receiver stated in the msg already equals to the specified one then nothing
        /// changes in the msg and the output indicator becomes false.</summary>
        /// <param name="data">XML builder containing the msg where receiver is to be set.</param>
        /// <param name="receiver">The new receiver of the msg.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// receiver has been written to the msg).</param>
        public static void SetMessageReceiver(ref XmlBuilder data, string receiver, out bool changed)
        {
            changed = false;
            if (data == null)
                throw new ArgumentException("Message builder is not specified.");
            if (string.IsNullOrEmpty(receiver))
                throw new ArgumentException("Receiver of the message is not specified.");
            data.GoToRoot();
            if (data.Current == null)
                throw new XmlException("Message XML does not contain a root element.");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Root element is empty.");
            data.NextOrCurrentElement(Const.ReceiverContainer);
            if (data.Current == null)
                throw new XmlException("Message XML does not contain message receiver's container element named "
                    + Const.ReceiverContainer + ".");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Message receiver's container element is empty. Element name: "
                    + Const.ReceiverContainer + ".");
            data.NextOrCurrentElement(Const.ReceiverSubContainer);
            if (data.Current == null)
                throw new XmlException("Message XML does not contain message receiver's subcontainer element named "
                    + Const.ReceiverSubContainer + ".");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Message receiver's subcontainer element is empty. Element name: "
                    + Const.ReceiverSubContainer + ".");
            data.NextOrCurrentElement(Const.IdElement);
            if (data.Current == null)
                throw new XmlException("Data about message receiver is badly formed. Container element: "
                    + Const.ReceiverContainer + ".");
            string MessageReceiver = data.Attribute(Const.IdIdAttribute);
            string MessageReceiverOid = data.Attribute(Const.IdOidAttribute);
            if (MessageReceiver != receiver)
            {
                string receiverlowercase = receiver.ToLower();
                if (LabexIdLowercase == null)
                    LabexIdLowercase = Const.LabexId.ToLower();
                if (BisIdLowercase == null)
                    BisIdLowercase = Const.BisId.ToLower();
                if (receiverlowercase == LabexIdLowercase)
                {
                    changed = true;
                    data.SetAttribute(Const.IdIdAttribute, receiver);
                    data.SetAttribute(Const.IdOidAttribute, Const.LabexIdOid);
                } else if (receiverlowercase == BisIdLowercase)
                {
                    changed = true;
                    data.SetAttribute(Const.IdIdAttribute, receiver);
                    data.SetAttribute(Const.IdOidAttribute, Const.BisIdOid);
                } else
                {
                    throw new ArgumentException("Unknown receiver ID: " + receiver + ".");
                }
            }
        }  // SetMessageReceiver




        /// <summary>Sets the sender of the communication msg stored in an Xml builder to the specified sender,
        /// and indicates through an output argument whether the sender has been changed in msg.
        /// If the Sender stated in the msg already equals to the specified one then nothing
        /// changes in the msg and the output indicator becomes false.</summary>
        /// <param name="data">XML builder containing the msg where Sender is to be set.</param>
        /// <param name="sender">The new Sender of the msg.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// Sender has been written to the msg).</param>
        public static void SetMessageSender(ref XmlBuilder data, string sender, out bool changed)
        {
            changed = false;
            if (data == null)
                throw new ArgumentException("Message builder is not specified.");
            if (string.IsNullOrEmpty(sender))
                throw new ArgumentException("Sender of the message is not specified.");
            data.GoToRoot();
            if (data.Current == null)
                throw new XmlException("Message XML does not contain a root element.");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Root element is empty.");
            data.NextOrCurrentElement(Const.SenderContainer);
            if (data.Current == null)
                throw new XmlException("Message XML does not contain message sender's container element named "
                    + Const.SenderContainer + ".");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Message sender's container element is empty. Element name: "
                    + Const.SenderContainer + ".");
            data.NextOrCurrentElement(Const.SenderSubContainer);
            if (data.Current == null)
                throw new XmlException("Message XML does not contain message sender's subcontainer element named "
                    + Const.SenderSubContainer + ".");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Message sender's subcontainer element is empty. Element name: "
                    + Const.SenderSubContainer + ".");
            data.NextOrCurrentElement(Const.IdElement);
            if (data.Current == null)
                throw new XmlException("Data about message sender is badly formed. Container element: "
                    + Const.SenderContainer + ".");
            string MessageSender = data.Attribute(Const.IdIdAttribute);
            string MessageSenderOid = data.Attribute(Const.IdOidAttribute);
            if (MessageSender != sender)
            {
                string senderlowercase = sender.ToLower();
                if (LabexIdLowercase == null)
                    LabexIdLowercase = Const.LabexId.ToLower();
                if (BisIdLowercase == null)
                    BisIdLowercase = Const.BisId.ToLower();
                if (senderlowercase == LabexIdLowercase)
                {
                    changed = true;
                    data.SetAttribute(Const.IdIdAttribute, sender);
                    data.SetAttribute(Const.IdOidAttribute, Const.LabexIdOid);
                }
                else if (senderlowercase == BisIdLowercase)
                {
                    changed = true;
                    data.SetAttribute(Const.IdIdAttribute, sender);
                    data.SetAttribute(Const.IdOidAttribute, Const.BisIdOid);
                }
                else
                {
                    throw new ArgumentException("Unknown sender ID: " + sender + ".");
                }
            }
        }  // SetMessageSender




        /// <summary>Sets the responder of the communication msg stored in an Xml builder to the specified responder,
        /// and indicates through an output argument whether the responder has been changed in msg.
        /// If the responder stated in the msg already equals to the specified one then nothing
        /// changes in the msg and the output indicator becomes false.</summary>
        /// <param name="data">XML builder containing the msg where responder is to be set.</param>
        /// <param name="responder">The new responder of the msg.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// responder has been written to the msg).</param>
        public static void SetMessageResponder(ref XmlBuilder data, string responder, out bool changed)
        {
            changed = false;
            if (data == null)
                throw new ArgumentException("Message builder is not specified.");
            if (string.IsNullOrEmpty(responder))
                throw new ArgumentException("Responder of the message is not specified.");
            data.GoToRoot();
            if (data.Current == null)
                throw new XmlException("Message XML does not contain a root element.");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Root element is empty.");
            data.NextOrCurrentElement(Const.ResponderContainer);
            if (data.Current == null)
                throw new XmlException("Message XML does not contain message responder's container element named "
                    + Const.ResponderContainer + ".");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Message responder's container element is empty. Element name: "
                    + Const.ResponderContainer + ".");
            data.NextOrCurrentElement(Const.ResponderSubContainer);
            if (data.Current == null)
                throw new XmlException("Message XML does not contain message responder's subcontainer element named "
                    + Const.ResponderSubContainer + ".");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Message responder's subcontainer element is empty. Element name: "
                    + Const.ResponderSubContainer + ".");
            data.NextOrCurrentElement(Const.IdElement);
            if (data.Current == null)
                throw new XmlException("Data about message responder is badly formed. Container element: "
                    + Const.ResponderContainer + ".");
            string MessageResponder = data.Attribute(Const.IdIdAttribute);
            string MessageResponderOid = data.Attribute(Const.IdOidAttribute);
            if (MessageResponder != responder)
            {
                string responderlowercase = responder.ToLower();
                if (LabexIdLowercase == null)
                    LabexIdLowercase = Const.LabexId.ToLower();
                if (BisIdLowercase == null)
                    BisIdLowercase = Const.BisId.ToLower();
                if (responderlowercase == LabexIdLowercase)
                {
                    changed = true;
                    data.SetAttribute(Const.IdIdAttribute, responder);
                    data.SetAttribute(Const.IdOidAttribute, Const.LabexIdOid);
                }
                else if (responderlowercase == BisIdLowercase)
                {
                    changed = true;
                    data.SetAttribute(Const.IdIdAttribute, responder);
                    data.SetAttribute(Const.IdOidAttribute, Const.BisIdOid);
                }
                else
                {
                    throw new ArgumentException("Unknown responder ID: " + responder + ".");
                }
            }
        }  // SetMessageResponder




        #endregion  // MessageReceiver


        #region MessageType

        /// <summary>Returns true if data contains a msg of type MessageType.SpecimenObservationOrder.</summary>
        /// <param name="data">XmlParser containing the msg.</param>
        private static bool IsSpecimenObservationOrder(XmlParser data)
        {
            data.GoToRoot();
            if (data.Name != Const.OoRootName)
                return false;
            if (data.StepIn() == null)
                return false;
            data.NextOrCurrentElement(Const.OoMessageContainer);
            if (data.Current == null)
                return false;
            data.StepIn();
            if (data.Current == null)
                return false;
            data.NextOrCurrentElement(Const.MessageSubContainer);
            if (data.Current == null)
                return false;
            data.StepIn();
            if (data.Current == null)
                return false;
            data.NextOrCurrentElement(Const.OoMessageSubContainer2);
            if (data.Current == null)
                return false;
            else
                return true;
        }


        /// <summary>Returns true if data contains a msg of type MessageType.SpecimenObservationEvent.</summary>
        /// <param name="data">XmlParser containing the msg.</param>
        private static bool IsSpecimenObservationEvent(XmlParser data)
        {
            data.GoToRoot();
            if (data.Name != Const.OeRootName)
                return false;
            if (data.StepIn() == null)
                return false;
            data.NextOrCurrentElement(Const.OeMessageContainer);
            if (data.Current == null)
                return false;
            data.StepIn();
            if (data.Current == null)
                return false;
            data.NextOrCurrentElement(Const.MessageSubContainer);
            if (data.Current == null)
                return false;
            data.StepIn();
            if (data.Current == null)
                return false;
            data.NextOrCurrentElement(Const.OeMessageSubContainer2);
            if (data.Current == null)
                return false;
            else
                return true;
        }


        /// <summary>Returns true if data contains a msg of type MessageType.DetailedFinancialTransaction.</summary>
        /// <param name="data">XmlParser containing the msg.</param>
        private static bool IsDetailedFinancialTransaction(XmlParser data)
        {
            data.GoToRoot();
            if (data.Name != Const.FtRootName)
                return false;
            if (data.StepIn() == null)
                return false;
            data.NextOrCurrentElement(Const.FtMessageContainer);
            if (data.Current == null)
                return false;
            data.StepIn();
            if (data.Current == null)
                return false;
            data.NextOrCurrentElement(Const.MessageSubContainer);
            if (data.Current == null)
                return false;
            data.StepIn();
            if (data.Current == null)
                return false;
            data.NextOrCurrentElement(Const.FtMessageSubContainer2);
            if (data.Current == null)
                return false;
            else
                return true;
        }

        /// <summary>Infers the msg type from the XML string containing the msg, and returns it.</summary>
        /// <param name="msgstring">Message string.</param>
        /// <returns>The type of the msg.</returns>
        public static MessageType GetMessageType(string msgstring)
        {
            MessageType ret = MessageType.Unknown;
            if (string.IsNullOrEmpty(msgstring))
                throw new ArgumentException("The message string is not specified (null or empty string).");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msgstring);
            ret = GetMessageType(doc);
            return ret;
        }

        /// <summary>Infers the msg type from the XML document containing the msg, and returns it.</summary>
        /// <param name="msg">Xml document containing the msg.</param>
        /// <returns>Message type (null if the type could not be inferred).</returns>
        public static MessageType GetMessageType(XmlDocument msg)
        {
            if (msg==null)
                throw new ArgumentNullException("Message document is not specified (null reference).");
            XmlParser data = new XmlParser();
            data.LoadXml(msg);
            return GetMessageType(data);
        }

        /// <summary>Infers the msg type from the XML parser containing the msg, and returns it.</summary>
        /// <param name="msg">Xml parser containing the msg.</param>
        /// <returns>Message type (null if the type could not be inferred).</returns>
        public static MessageType GetMessageType(XmlParser data)
        {
            MessageType ret = MessageType.Unknown;
            if (data != null)
            {
                if (IsSpecimenObservationOrder(data))
                    ret = MessageType.SpecimenObservationOrder;
                else if (IsSpecimenObservationEvent(data))
                    ret = MessageType.SpecimenObservationEvent;
                else if (IsDetailedFinancialTransaction(data))
                    ret = MessageType.DetailedFinancialTransaction;
            }
            return ret;
        }

        #endregion  // MessageType

    }  // class MsgBase


    /// <summary>Base class messages, includes possibility of model document for
    /// conversion of the object to XML msg according to specification.</summary>
    public abstract class MsgBaseWithModel : MsgBase
    {

        private XmlDocument _modelDocument = null;

        /// <summary>Gets or seta the XML document that is used as model for creation of XML.
        /// Warning: Each call to execution of get() makes a clone of an XmlDocument.</summary>
        public virtual XmlDocument ModelDocument
        {
            get 
            {
                if (_modelDocument != null)
                {
                    return (XmlDocument) _modelDocument.Clone();
                } else
                    return _modelDocument; 
            }
            set { _modelDocument = value; }
        }

        /// <summary>Sets the model document for creation of the msg to the contents of the specified file.</summary>
        /// <param name="FilePath">Path of the file containing the model msg.</param>
        public void SetModelFile(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                throw new ArgumentNullException("Name of the output file is not specified.");
            if (!File.Exists(FilePath))
                throw new ArgumentException("The specified model messagee file does not not exist. "
                    + Environment.NewLine + "  File path: \"" + FilePath + "\".");
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);
            ModelDocument = doc;
        }

        /// <summary>Sets the model document for creation of the msg to the contents of the specified string.</summary>
        /// <param name="FilePath">String containing the model msg.</param>
        public void SetModelString(string XmlString)
        {
            if (string.IsNullOrEmpty(XmlString))
                throw new ArgumentNullException("String containing model message is not specified.");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XmlString);
            ModelDocument = doc;
        }

        /// <summary>Sets the model document for creation of the msg to the contents of the specified string.</summary>
        /// <param name="FilePath">String containing the model msg.</param>
        public void SetModelDocument(XmlDocument doc)
        {
            ModelDocument = doc;
            if (doc == null)
                throw new ArgumentNullException("Model document is not specified (null reference). Document has been set anyway.");
        }

    }



}
