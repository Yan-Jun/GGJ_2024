using GGJ.Ingame.Common;
using GGJ.Ingame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GGJ.Ingame.Controller
{
    public class InputCodeController : MonoBehaviour
    {
        private string _nextArrangeCode;
        private int _nextCodeIndex;

        private IInputCodeSystem _inputCodeSystem;
        private IInputCodePanel _inputCodePanel;

        private void Start ()
        {
            Initialize(
                FindFirstObjectByType<InputCodeSystem>(),
                FindFirstObjectByType<InputCodePanel>());
        }

        private void Initialize(IInputCodeSystem inputCodeSystem, IInputCodePanel inputCodePanel)
        {
            _inputCodeSystem = inputCodeSystem;
            _inputCodePanel = inputCodePanel;

            Assert.IsNotNull(_inputCodeSystem);
            Assert.IsNotNull(_inputCodePanel);

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
                _nextCodeIndex++;

                var isCompleted = CheckInputComplete(_nextCodeIndex, _nextArrangeCode);
                if (isCompleted)
                {
                    CompleteCode();
                    //жие\
                    PlayerStat.i.AddWallPoint("Wall_1",1000);
                    return;
                }
                //Debug.Log($"Next type code: {_nextArrangeCode[_nextCodeIndex]}");
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
    }
}