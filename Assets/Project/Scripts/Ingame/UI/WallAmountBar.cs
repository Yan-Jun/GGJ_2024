using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Ingame.UI
{
    public class WallAmountBar : MonoBehaviour
    {
        [SerializeField] private WallAmountSetting _wallAmountSetting;
        [SerializeField] private AmountBarSetting _amountBarSetting;

        private int _maxAmount;
        private int _currentAmount;

        private void Start()
        {
            Initialize(5000, 5000);
        }

        public void UpdateWallAmount(int currentAmount)
        {
            _currentAmount = currentAmount;
            var barCount = _currentAmount / _amountBarSetting.OneBarAmount;
            var lastBarAmount = _currentAmount % _amountBarSetting.OneBarAmount;
            for (var i = 0; i < _amountBarSetting.Images.Length; i++)
            {
                if (i < barCount)
                {
                    // Full
                    _amountBarSetting.Images[i].sprite = 
                        _wallAmountSetting.Walls.First((wall) => wall.Amount <= _amountBarSetting.OneBarAmount).WallSprite;
                }
                else if (i == barCount)
                {
                    // Half
                    _amountBarSetting.Images[i].sprite = 
                        _wallAmountSetting.Walls.First((wall) => wall.Amount <= lastBarAmount).WallSprite;
                }
                else
                {
                    // Empty
                    _amountBarSetting.Images[i].sprite = 
                        _wallAmountSetting.Walls.First((wall) => wall.Amount == 0).WallSprite;
                }
            }
        }

        public void Initialize(int currentAmount, int maxAmount)
        {
            _currentAmount = currentAmount;
            _maxAmount = maxAmount;
        }

        [System.Serializable]
        public struct AmountBarSetting
        {
            public int OneBarAmount;
            public Image[] Images;
        }

        [System.Serializable]
        public struct WallAmountSetting
        {
            public Wall[] Walls;
        }

        [System.Serializable]
        public struct Wall
        {
            public float Amount;
            public Sprite WallSprite;
        }
    }
}