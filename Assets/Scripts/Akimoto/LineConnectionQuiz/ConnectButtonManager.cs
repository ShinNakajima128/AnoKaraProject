using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectButtonManager : MonoBehaviour
{
    private List<LineConnectButton> m_connectButtons = new List<LineConnectButton>();

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            LineConnectButton button = transform.GetChild(i).GetComponent<LineConnectButton>();
            if (button == null) continue;
            button.Parent = this;
            m_connectButtons.Add(button);
        }
    }
}
