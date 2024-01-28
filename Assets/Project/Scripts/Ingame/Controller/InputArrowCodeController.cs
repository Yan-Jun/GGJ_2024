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
        [SerializeField] private InputCodePanel _targetInputCodePanel;

        private string[] _nextArrangeArrowCode;
        private int _nextCodeIndex;
        private bool _isWaitComplete;

        private IInputCodeSystem _inputCodeSystem;
        private IInputCodePanel _inputCodePanel;
        private IGenerateCodeSystem _generateCodeSystem;


        private void Start()
        {
            Initialize(
                FindFirstObjectByType<InputCodeSystem>(),
                _targetInputCodePanel,
                new GenerateCodeSystem());
        }

        private void Initialize(IInputCodeSystem inputCodeSystem, IInputCodePanel inputCodePanel, IGenerateCodeSystem generateCodeSystem)
        {
            _inputCodeSystem = inputCodeSystem;
            _inputCodePanel = inputCodePanel;
            _generateCodeSystem = generateCodeSystem;

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
                _nextCodeIndex++;

                var isCompleted = CheckInputComplete(_nextCodeIndex, _nextArrangeArrowCode);
                if (isCompleted)
                {
                    CompleteCode().Forget();
                    return;
                }
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
    }
}