using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionDataBaseTest : MonoBehaviour
{
    [SerializeField]
    CollectionData m_collectionDatabase;

    void Start()
    {
        CollectionDataBase database = m_collectionDatabase.m_dataBases[0];
        Debug.Log($"name:{database.Name}, tooltip:{database.Tooltip}, flag:{database.IsGet}");
    }
}
