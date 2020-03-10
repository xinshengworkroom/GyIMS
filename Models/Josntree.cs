using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Models
{
    public class JsonTree
    {
        private string _id;
        private string _text;
        private string _state = "closed";
        private string _url;
        private Dictionary<string, string> _attributes = new Dictionary<string, string>();
        private object _children;

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }

        public string url
        {
            get { return _url; }
            set { _url = value; }
        }
        public Dictionary<string, string> attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }
        public object children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}