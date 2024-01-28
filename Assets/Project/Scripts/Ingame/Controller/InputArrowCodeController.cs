using Cysharp.Threading.Tasks;
using GGJ.Ingame.Common;
using GGJ.Ingame.UI;
using System;
using UnityEngine.Assertions;
using UnityEngine;

namespace GGJ.Ingame.Controller
{
    public class InputArrowCodeController : MonoBehaviour
    {
        [SerializeField] private AudioSetting _audioSetting;
        [SerializeField] private InputCodePanel _targetInputCodePanel;

        private string[] _nextArrangeArrowCode;
        private int _nextCodeIndex;
        private bool _isWaitComplete;

        private IInputCodeSystem _inputCodeSystem;
        private IInputCodePanel _inputCodePanel;
        private IGenerateCodeSystem _generateCodeSystem;

        private AudioSource _audioSource;

        private void Start()
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

            _nextArrangeArrowCode = _generateCodeSystem.GetGenerateArrowCode(3, 5);

            _inputCodePanel.ClearAll();
            _inputCodePanel.SetInputArrowCodes(_nextArrangeArrowCode);

        }

        private void Update()
        {
            if (_isWaitComplete)
                return;

            UpdateCheckKeyCode();
        }

        public void UpdateCheckKeyCode()
        {
            var inputCode = _inputCodeSystem.GetCurrentKeyCode();
            if (inputCode != null && _nextArrangeArrowCode[_nextCodeIndex] == inputCode)
            {
                _inputCodePanel.SetCompletedIndex(_nextCodeIndex, inputCode);
                _audioSource.PlayOneShot(_audioSetting.SuccessClip);
                _nextCodeIndex++;

                var isCompleted = CheckInputComplete(_nextCodeIndex, _nextArrangeArrowCode);
                if (isCompleted)
                {
                    CompleteCode().Forget();
                    PlayerStat.instance.AddWallPoint("Wall_1", 1000);
                    _audioSource.PlayOneShot(_audioSetting.CompleteClip);
                    return;
                }
                return;
            }

            if (inputCode != null)
            {
                _audioSource.PlayOneShot(_audioSetting.FailClip);
            }
        }

        public bool CheckInputComplete(int currentIndex, string[] arrangeArrowCode)
        {
            return currentIndex == arrangeArrowCode.Length;
        }


        public async UniTask CompleteCode()
        {
            _isWaitComplete = true;
            _nextArrangeArrowCode = _generateCodeSystem.GetGenerateArrowCode(3, 5);
            _nextCodeIndex = 0;

            await UniTask.Delay(TimeSpan.FromSeconds(.5));

            _inputCodePanel.ClearAll();
            await _inputCodePanel.SetInputArrowCodes(_nextArrangeArrowCode);

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