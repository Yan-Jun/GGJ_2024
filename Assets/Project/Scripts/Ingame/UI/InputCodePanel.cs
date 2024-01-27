using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace GGJ.Ingame.UI
{
    public interface IInputCodePanel
    {
        void ClearAll();
        UniTaskVoid SetCompletedIndex(int index, string arrowCode);
        UniTaskVoid SetCompletedIndex(int index, char code);
        UniTask SetInputArrowCodes(string[] arrowCodes);
        UniTask SetInputCodes(string codes);
    }

    public class InputCodePanel : MonoBehaviour, IInputCodePanel
    {
        [SerializeField] private InputCodeSetting _inputCodeSetting;

        private readonly string _keyUpAssetKey = "KeyUp/{0}";
        private readonly string _keyDownAssetKey = "KeyDown/{0}";

        public async UniTask SetInputCodes(string codes)
        {
            for (var i = 0; i < codes.Length; i++)
            {
                var codeImage = _inputCodeSetting.CodeObjects[i].GetComponent<Image>();
                if (codeImage.sprite)
                {
                    Addressables.Release(codeImage.sprite);
                    codeImage.sprite = null;
                }

                var key = string.Format(_keyUpAssetKey, codes[i]);
                var sprite = await Addressables.LoadAssetAsync<Sprite>(key);
                codeImage.sprite = sprite;
            }

            for (var i = 0; i < codes.Length; i++)
            {
                _inputCodeSetting.CodeObjects[i].SetActive(true);
            }
        }

        public async UniTask SetInputArrowCodes(string[] arrowCodes)
        {
            for (var i = 0; i < arrowCodes.Length; i++)
            {
                var codeImage = _inputCodeSetting.CodeObjects[i].GetComponent<Image>();
                if (codeImage.sprite)
                {
                    Addressables.Release(codeImage.sprite);
                    codeImage.sprite = null;
                }

                var key = string.Format(_keyUpAssetKey, arrowCodes[i]);
                var sprite = await Addressables.LoadAssetAsync<Sprite>(key);
                codeImage.sprite = sprite;
            }

            for (var i = 0; i < arrowCodes.Length; i++)
            {
                _inputCodeSetting.CodeObjects[i].SetActive(true);
            }
        }


        public async UniTaskVoid SetCompletedIndex(int index, char code)
        {
            var key = string.Format(_keyDownAssetKey, code);
            var sprite = await Addressables.LoadAssetAsync<Sprite>(key);

            var codeImage = _inputCodeSetting.CodeObjects[index].GetComponent<Image>();
            if (codeImage.sprite)
            {
                Addressables.Release(codeImage.sprite);
                codeImage.sprite = null;
            }
            codeImage.GetComponent<Image>().sprite = sprite;
            codeImage.gameObject.SetActive(true);
        }

        public async UniTaskVoid SetCompletedIndex(int index, string arrowCode)
        {
            var key = string.Format(_keyDownAssetKey, arrowCode);
            var sprite = await Addressables.LoadAssetAsync<Sprite>(key);

            var codeImage = _inputCodeSetting.CodeObjects[index].GetComponent<Image>();
            if (codeImage.sprite)
            {
                Addressables.Release(codeImage.sprite);
                codeImage.sprite = null;
            }
            codeImage.GetComponent<Image>().sprite = sprite;
            codeImage.gameObject.SetActive(true);
        }


        public void ClearAll()
        {
            for (var i = 0; i < _inputCodeSetting.CodeObjects.Length; i++)
            {
                _inputCodeSetting.CodeObjects[i].gameObject.SetActive(false);
                var codeImage = _inputCodeSetting.CodeObjects[i].GetComponent<Image>();
                if (codeImage.sprite)
                {
                    Addressables.Release(codeImage.sprite);
                    codeImage.sprite = null;
                }
            }
        }

        private void OnDestroy()
        {
            ClearAll();
        }
    }

    [System.Serializable]
    public struct InputCodeSetting
    {
        public GameObject[] CodeObjects;
    }
}