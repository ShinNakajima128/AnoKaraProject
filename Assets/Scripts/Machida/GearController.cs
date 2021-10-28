using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearController : MonoBehaviour
{
    /// <summary>回すカウント<summary>
    [SerializeField]
    int m_count = default;

    /// <summary>現在のカウント<summary>
    [SerializeField]
    int m_currentCount = default;

    /// <summary>表示するパネル<summary>
    [SerializeField]
    GameObject m_panel = default;

    // 画像を回す
    /// <summary>クリック時のマウス位置<summary>
    Vector2 m_pos;

    /// <summary>クリックしたときのターゲットの角度<summary>
    Quaternion m_rotation;

    /// <summary>ターゲットの中心からposへのベクトル<summary>
    Vector2 m_vecA;

    /// <summary>ターゲットの中心から現マウス位置へのベクトル<summary>
    Vector2 m_vecB;

    /// <summary>vecAとvecBが成す角度<summary>
    float m_angle;

    /// <summary>vecAとvecBの外積<summary>
    Vector3 m_crossProduct;

    /// <summary>ドラッグ中のフラグ<summary>
    bool m_flag;

    // 回った角度を求める
    /// <summary>マウスのポジション<summary>
    Vector2 m_mousePos;

    /// <summary>画面の半分のサイズ<summary>
    Vector3 screenSizeHalf;

    /// <summary>現在の角度<summary>
    float m_currentAngle;

    /// <summary>どのくらい動いたか<summary>
    float m_tan = 0f;

    void Start()
    {
        m_count = Random.Range(2, 5);
        m_currentCount = 0;

        // 画面の縦横の半分 
        screenSizeHalf.x = Screen.width / 2f;
        screenSizeHalf.y = Screen.height / 2f;
        screenSizeHalf.z = 0f;

        // マウスの初期位置
        m_mousePos = Input.mousePosition - screenSizeHalf;
        m_currentAngle = Mathf.Atan2(m_mousePos.x, m_mousePos.y);
    }

    void Update()
    {
        if (m_count <= m_currentCount)
            m_panel.gameObject.SetActive(true);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            // 真ん中が(0,0,0)になるようにマウスの位置を取得
            m_mousePos = Input.mousePosition - screenSizeHalf;

            float pos = Mathf.Atan2(m_mousePos.x, m_mousePos.y); // 上向きとマウス位置のなす角
            float currentPos = pos - m_currentAngle; // 前のフレームの角度との差

            m_tan += Mathf.Tan(currentPos);

            if (m_tan < -1)
                m_tan = 0;

            if (m_tan > 7)
            {
                m_currentCount++;
                m_tan = 0;
            }

            m_currentAngle = pos; // 今のフレームの角度を保存// ターゲットの現角度を取得
        }

        if (m_flag)  // マウスのクリック状態からメソッドを選択
        {
            Rotate();
        }
        else
        {
            SetPos();
        }
    }

    /// <summary>
    /// マウスをクリックした時の処理
    /// </summary>
    void SetPos()
    {
        //クリック時のマウスの初期位置とターゲットの現角度を取得
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // マウス位置をワールド座標で取得
            m_rotation = this.transform.rotation;

            m_flag = true;
        }
    }

    /// <summary>
    /// ドラッグ中かを判断して処理を行う
    /// </summary>
    void Rotate()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_flag = false;
            return;
        }

        //マウス初期位置のベクトルを求める
        m_vecA = m_pos - (Vector2)this.transform.position;

        //現マウス位置のベクトルを求める
        m_vecB = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;

        // Vector2にしているのはz座標が悪さをしないようにするため

        //マウスの初期位置と現マウス位置から角度と外積を求める
        m_angle = Vector2.Angle(m_vecA, m_vecB);  //角度を計算
        m_crossProduct = Vector3.Cross(m_vecA, m_vecB);    //外積を計算

        // 外積の z 成分の正負で回転方向を決める
        if (m_crossProduct.z > 0)
        {
            this.transform.localRotation = m_rotation * Quaternion.Euler(0, 0, m_angle); // 初期値との掛け算で相対的に回転させる
        }
        else
        {
            this.transform.localRotation = m_rotation * Quaternion.Euler(0, 0, -m_angle); // 初期値との掛け算で相対的に回転させる
        }
    }
}
