using GGJ.Ingame.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GGJ.Ingame.UI
{
    public class InputCodeController : MonoBehaviour
    {
        private string _nextArrangeCode;
        private int _nextCodeIndex;

        private IInputCodeSystem _inputCodeSystem;

        private void Start ()
        {
            Initialize(
                FindFirstObjectByType<InputCodeSystem>());
        }

        private void Initialize(IInputCodeSystem inputCodeSystem)
        {
            _inputCodeSystem = inputCodeSystem;

            Assert.IsNotNull(_inputCodeSystem);

            _nextArrangeCode = GenerateTestCode();
            Debug.Log($"Next code: {_nextArrangeCode}");
        }

        private void Update()
        {
            UpdateInputCode();
        }

        public void UpdateInputCode()
        {
            var inputCode = _inputCodeSystem.GetCurrentInputCode();
            if (inputCode.HasValue && _nextArrangeCode[_nextCodeIndex] == inputCode)
            {
                _nextCodeIndex++;
                var isCompleted = CheckInputComplete(_nextCodeIndex, _nextArrangeCode);
                if (isCompleted)
                {
                    CompleteCode();
                    return;
                }
                Debug.Log($"Next type code: {_nextArrangeCode[_nextCodeIndex]}");
            }
        }

        public bool CheckInputComplete(int currentIndex, string arrangeCode)
        {
            return _nextCodeIndex == arrangeCode.Length;
        }

        public void CompleteCode()
        {
            Debug.Log($"Completed code: {_nextArrangeCode}");
            _nextArrangeCode = GenerateTestCode();
            _nextCodeIndex = 0;
            Debug.Log($"Next code: {_nextArrangeCode}");
        }

        private string GenerateTestCode()
        {
            var simpleCodes = new string[]
            {
                "aaabbb",
                "abcdef",
                "123abc",
            };
            return simpleCodes[Random.Range(0, simpleCodes.Length - 1)];
        }

    }
}