using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�N���X����</summary>
public class Samaple : MonoBehaviour
{
    public static Samaple Instance { get; private set; }

    /// <summary>�T�}���[�͈�s�ŋL�ڂ���</summary>
    [SerializeField]                //SerializeField�ŉ��s
    GameObject m_obj = default;     //�����o�[�ϐ��ɂ�"m_"
                                    //���̕ϐ��͉��s����
    /// <summary>�T���v��</summary>
    GameObject m_obj2 = default;

    /// <summary>
    /// �֐���
    /// </summary>
    void SampleVoid()
    {

    }

    /// <summary>
    /// �֐���
    /// </summary>
    /// <param name="value">�����̏ڍ�</param>
    /// <returns>�Ԃ�l</returns>
    int Sample(int value)
    {
        int a = value;
        return a;
    }
}


