using Cysharp.Threading.Tasks;
using GGJ.Ingame.Common;
using GGJ.Ingame.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace GGJ.Ingame.Controller
{
    public class InputCodeController : MonoBehaviour 
    {
        [SerializeField] private AudioSetting _audioSetting;
        [SerializeField] private InputCodePanel _targetInputCodePanel;
        private string _nextArrangeCode;
        private int _nextCodeIndex;
        private bool _isWaitComplete;

        private IInputCodeSystem _inputCodeSystem;
        private IInputCodePanel _inputCodePanel;
        private IGenerateCodeSystem _generateCodeSystem;

        private AudioSource _audioSource;

        private void Start ()
        {
            Initialize(
                FindFirstObjectByType<InputCodeSystem>(),
                _targetInputCodePanel,
                new GenerateCodeSystem(),
                GetComponent<AudioSource>()
                );
        }


        private void Initialize(IInputCodeSystem inputCodeSystem, IInputCodePanel inputCodePanel, IGenerateCodeSystem generateCodeSystem, AudioSource audioSource)
        {
            _inputCodeSystem = inputCodeSystem;
            _inputCodePanel = inputCodePanel;
            _generateCodeSystem = generateCodeSystem;
            _audioSource = audioSource;

            Assert.IsNotNull(_inputCodeSystem);
            Assert.IsNotNull(_inputCodePanel);
            Assert.IsNotNull(_audioSource);

            _nextArrangeCode = _generateCodeSystem.GetGenerateCode(6, 10);
            //Debug.Log($"Next code: {_nextArrangeCode}");

            _inputCodePanel.ClearAll();
            _inputCodePanel.SetInputCodes(_nextArrangeCode);

        }

        private void Update()
        {
            if (_isWaitComplete)
                return;

            UpdateCheckCode();
        }

        public void UpdateCheckCode()
        {
            var inputCode = _inputCodeSystem.GetCurrentInputCode();
            if (inputCode.HasValue && _nextArrangeCode[_nextCodeIndex] == char.ToLower(inputCode.Value))
            {
                _inputCodePanel.SetCompletedIndex(_nextCodeIndex, char.ToLower(inputCode.Value));
                _audioSource.PlayOneShot(_audioSetting.SuccessClip);
                _nextCodeIndex++;

                var isCompleted = CheckInputComplete(_nextCodeIndex, _nextArrangeCode);
                if (isCompleted)
                {
                    CompleteCode().Forget();
                    PlayerStat.instance.AddWallPoint("Wall_2",1000);
                    _audioSource.PlayOneShot(_audioSetting.CompleteClip);
                    return;
                }
                return;
                //Debug.Log($"Next type code: {_nextArrangeCode[_nextCodeIndex]}");
            }

            if (inputCode.HasValue)
            {
                _audioSource.PlayOneShot(_audioSetting.FailClip);
            }
        }

        public bool CheckInputComplete(int currentIndex, string arrangeCode)
        {
            return currentIndex == arrangeCode.Length;
        }

        public async UniTask CompleteCode()
        {
            _isWaitComplete = true;
            _nextArrangeCode = _generateCodeSystem.GetGenerateCode(6, 10);
            _nextCodeIndex = 0;

            await UniTask.Delay(TimeSpan.FromSeconds(.5));

            _inputCodePanel.ClearAll();
            await _inputCodePanel.SetInputCodes(_nextArrangeCode);

            _isWaitComplete = false;
        }

        [System.Serializable]
        public struct AudioSetting
        {
            public AudioClip SuccessClip;
            public AudioClip FailClip;
            public AudioClip CompleteClip;
        } 
    }
}