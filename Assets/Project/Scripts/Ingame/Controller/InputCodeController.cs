using GGJ.Ingame.Common;
using GGJ.Ingame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace GGJ.Ingame.Controller
{
    public class InputCodeController : MonoBehaviour 
    {
        [SerializeField] private AudioSetting _audioSetting;
        private string _nextArrangeCode;
        private int _nextCodeIndex;

        private IInputCodeSystem _inputCodeSystem;
        private IInputCodePanel _inputCodePanel;
        private AudioSource _audioSource;

        private void Start ()
        {
            Initialize(
                FindFirstObjectByType<InputCodeSystem>(),
                FindFirstObjectByType<InputCodePanel>(),
                GetComponent<AudioSource>());
        }

        public void Initialize(IInputCodeSystem inputCodeSystem, IInputCodePanel inputCodePanel, AudioSource audioSource)
        {
            _inputCodeSystem = inputCodeSystem;
            _inputCodePanel = inputCodePanel;
            _audioSource = audioSource;

            Assert.IsNotNull(_inputCodeSystem);
            Assert.IsNotNull(_inputCodePanel);
            Assert.IsNotNull(_audioSource);

            _nextArrangeCode = GenerateTestCode();
            //Debug.Log($"Next code: {_nextArrangeCode}");

            _inputCodePanel.ClearAll();
            _inputCodePanel.SetInputCodes(_nextArrangeCode);
        }

        private void Update()
        {
            UpdateCheckCode();
        }

        public void UpdateCheckCode()
        {
            var inputCode = _inputCodeSystem.GetCurrentInputCode();
            if (inputCode.HasValue && _nextArrangeCode[_nextCodeIndex] == inputCode)
            {
                _inputCodePanel.SetCompletedIndex(_nextCodeIndex);
                _audioSource.PlayOneShot(_audioSetting.SuccessClip);
                _nextCodeIndex++;

                var isCompleted = CheckInputComplete(_nextCodeIndex, _nextArrangeCode);
                if (isCompleted)
                {
                    CompleteCode();
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
            return _nextCodeIndex == arrangeCode.Length;
        }

        public void CompleteCode()
        {
            //Debug.Log($"Completed code: {_nextArrangeCode}");
            _nextArrangeCode = GenerateTestCode();
            _nextCodeIndex = 0;

            _inputCodePanel.ClearAll();
            _inputCodePanel.SetInputCodes(_nextArrangeCode);
            //Debug.Log($"Next code: {_nextArrangeCode}");
        }

        private string GenerateTestCode()
        {
            var simpleCodes = new string[]
            {
                "aaabbb",
                "1111111",
                "12345678",
                "123abc",
            };
            return simpleCodes[Random.Range(0, simpleCodes.Length)];
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