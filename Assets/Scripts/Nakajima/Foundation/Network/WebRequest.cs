using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public class WebRequest
    {
        public delegate void GetDynamic(dynamic result);
        public delegate void GetString(string result);

        public enum ResultType
        {
            String,
            Json
        }

        public enum RequestMethod
        {
            GET,
            POST
        }

        static WebRequest Instance= new WebRequest();

        HTTPRequest Worker = null;
        
        static public void Request<T>(string uri, ResultType type, T dlg)
        {
            if(Instance.Worker == null)
            {
                Instance.Worker = GameObject.FindObjectOfType<HTTPRequest>();
                if (Instance.Worker == null) return;
            }

            Instance.Worker.Request<T>(uri, type, dlg);
        }
    }
}
