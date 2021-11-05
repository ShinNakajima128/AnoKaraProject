using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 各クイズのデータを読み込む際に使用するクラス
/// </summary>
namespace MasterData
{
    /// <summary> 時代の種類 </summary>
    public enum PeriodTypes
    {
        None,
        Jomon_Yayoi,
        Asuka,
        Heian,
        Kamakura,
        Sengoku,
        Edo
    }

    public class QuizMasterDataClass<T>
    {
        public T[] Data;
    }

    /// <summary>
    /// 四択クイズのクラス
    /// </summary>
    [Serializable]
    public class FourChoicesQuiz
    {
        public int Id;
        [TextArea(0, 10)]
        public string Question;
        public string Choices1;
        public string Choices2;
        public string Choices3;
        public string Choices4;
        public string Answer;
    }

    /// <summary>
    /// 穴埋めクイズのクラス
    /// </summary>
    [Serializable]
    public class AnaumeQuiz
    {
        public int Id;
        [TextArea(0, 10)]
        public string Question;
        public string Dragtext;
        public string Answer;
        public string[] m_dragTexts;

        public void ConvartToArray()
        {
            m_dragTexts = Dragtext.Split(',');
        }
    }

    //別のパターンを用意する場合はこの下にクラスを追加する
}
