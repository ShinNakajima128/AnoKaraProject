using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using static Network.WebRequest;

public class HTTPRequest : MonoBehaviour
{
    struct Packet<T>
    {
        public string Uri;
        public RequestMethod Method;
        public ResultType Type;
        public T Delegate;
    }
    
    public void Request<T>(string uri, ResultType type, T dlg)
    {
        Packet<T> p = new Packet<T>();
        p.Uri = uri;
        p.Type = type;
        p.Delegate = dlg;
        p.Method = RequestMethod.GET;
        StartCoroutine(Send(p));
    }

    IEnumerator Send<T>(Packet<T> p)
    {
        UnityWebRequest req = UnityWebRequest.Get(p.Uri);
        yield return req.SendWebRequest();

        if (req.error != null)
        {
            Debug.Log(req.error);
        }
        else
        {
            DataParse(p, req);
        }
    }

    void DataParse<T>(Packet<T> p, UnityWebRequest req)
    {
        string str = req.downloadHandler.text;
        switch (p.Type)
        {
            case ResultType.String:
                {
                    GetString Delegate = p.Delegate as GetString;
                    if(Delegate != null)
                    {
                        Delegate(req.downloadHandler.text);
                    }
                }
                break;
        }
    }    
}