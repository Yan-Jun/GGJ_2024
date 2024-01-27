using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Ingame.UI
{
    public interface IInputCodePanel
    {
        void ClearAll();
        void Initialize();
        void SetCompletedIndex(int index);
        void SetInputCodes(string codes);
    }

    public class InputCodePanel : MonoBehaviour, IInputCodePanel
    {
        [SerializeField] private InputCodeSetting _inputCodeSetting;

        public void Initialize()
        {
            ClearAll();
        }

        public void SetInputCodes(string codes)
        {
            for (var i = 0; i < codes.Length; i++)
            {
                _inputCodeSetting.CodeObjects[i].GetComponentInChildren<TMP_Text>().SetText(codes[i].ToString());
                _inputCodeSetting.CodeObjects[i].GetComponent<Image>().color = Color.white;
                _inputCodeSetting.CodeObjects[i].gameObject.SetActive(true);
            }
        }

        public void SetCompletedIndex(int index)
        {
            for (var i = 0; i < _inputCodeSetting.CodeObjects.Length; i++)
            {
                _inputCodeSetting.CodeObjects[i].GetComponent<Image>().color =
                    i <= index ?
                        Color.green :
                        Color.white;
            }
        }

        public void ClearAll()
        {
            for (var i = 0; i < _inputCodeSetting.CodeObjects.Length; i++)
            {
                _inputCodeSetting.CodeObjects[i].gameObject.SetActive(false);
            }
        }
    }

    [System.Serializable]
    public struct InputCodeSetting
    {
        public GameObject[] CodeObjects;
    }
}