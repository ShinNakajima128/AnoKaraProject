using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject activateObject;

    [SerializeField]
    GameObject inactivateObject;

    [SerializeField]
    Text activeText;

    [SerializeField]
    Text inactiveText;
    public void ObjectActive()
    {
        activateObject.SetActive(true);
    }

    public void ObjectInactivate()
    {
        inactivateObject.SetActive(false);
    }

    public void TextActivate()
    {
        activeText.gameObject.SetActive(true);
    }

    public void Textinactivate()
    {
        inactiveText.gameObject.SetActive(false);
    }
}
