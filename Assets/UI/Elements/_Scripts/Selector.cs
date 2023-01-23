using System.Collections.Generic;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Elements {
    public class Selector : VisualElement {
        public new class UxmlFactory : UxmlFactory<Selector, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits {
            private readonly UxmlStringAttributeDescription _unselectedSpriteAddress = new() {
                name = "unselected-sprite-address",
                defaultValue = string.Empty
            };

            private readonly UxmlStringAttributeDescription _selectedSpriteAddress = new() {
                name = "selected-sprite-address",
                defaultValue = string.Empty
            };

            private readonly UxmlFloatAttributeDescription _unselectedSpriteSize = new() {
                name = "unselected-sprite-size",
                defaultValue = 100
            };

            private readonly UxmlFloatAttributeDescription _selectedSpriteSize = new() {
                name = "selected-sprite-size",
                defaultValue = 200
            };

            private readonly UxmlEnumAttributeDescription<EasingMode> _spriteSizeEasing = new() {
                name = "sprite-size-easing",
                defaultValue = EasingMode.Ease
            };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                var ate = (Selector)ve;

                ate.UnselectedSpriteAddress = _unselectedSpriteAddress.GetValueFromBag(bag, cc);
                ate.SelectedSpriteAddress = _selectedSpriteAddress.GetValueFromBag(bag, cc);
                ate.UnselectedSpriteSize = _unselectedSpriteSize.GetValueFromBag(bag, cc);
                ate.SelectedSpriteSize = _selectedSpriteSize.GetValueFromBag(bag, cc);
                ate.SpriteSizeEasing = _spriteSizeEasing.GetValueFromBag(bag, cc);
            }
        }

        public string UnselectedSpriteAddress {
            get => _unselectedSpriteAddress;
            set {
                if (_unselectedSpriteAddress == value) {
                    return;
                }

                _unselectedSpriteAddress = value;
                UnselectedSprite = string.IsNullOrEmpty(_unselectedSpriteAddress)
                    ? null
                    : Addressables.LoadAssetAsync<Sprite>(_unselectedSpriteAddress).WaitForCompletion();
            }
        }

        public string SelectedSpriteAddress {
            get => _selectedSpriteAddress;
            set {
                if (_selectedSpriteAddress == value) {
                    return;
                }

                _selectedSpriteAddress = value;
                SelectedSprite = string.IsNullOrEmpty(_selectedSpriteAddress)
                    ? null
                    : Addressables.LoadAssetAsync<Sprite>(_selectedSpriteAddress).WaitForCompletion();
            }
        }

        public Sprite UnselectedSprite {
            get => _unselectedSprite;
            set {
                if (_unselectedSprite == value) {
                    return;
                }

                _unselectedSprite = value;
                UpdateSprite();
            }
        }

        public Sprite SelectedSprite {
            get => _selectedSprite;
            set {
                if (_selectedSprite == value) {
                    return;
                }

                _selectedSprite = value;
                UpdateSprite();
            }
        }

        public float UnselectedSpriteSize {
            get => _unselectedSpriteSize;
            set {
                _unselectedSpriteSize = value;
                UpdateSprite();
            }
        }

        public float SelectedSpriteSize {
            get => _selectedSpriteSize;
            set {
                _selectedSpriteSize = value;
                UpdateSprite();
            }
        }

        public EasingMode SpriteSizeEasing {
            get => _spriteSizeEasing;
            set {
                if (_spriteSizeEasing == value) {
                    return;
                }

                _spriteSizeEasing = value;
                UpdateTransitions();
            }
        }

        public bool IsSelected {
            get => _isSelected;
            set {
                if (_isSelected == value) {
                    return;
                }

                _isSelected = value;
                UpdateSprite();
            }
        }

        private string _unselectedSpriteAddress;
        private string _selectedSpriteAddress;
        private Sprite _unselectedSprite;
        private Sprite _selectedSprite;
        private float _unselectedSpriteSize;
        private float _selectedSpriteSize;
        private EasingMode _spriteSizeEasing;
        private bool _isSelected;
        private readonly VisualElement _sprite;

        public Selector() {
            style.position = Position.Absolute;
            style.width = 0;
            style.height = 0;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.Center;

            _sprite = new VisualElement {
                name = "sprite",
                style = {
                    position = Position.Absolute
                }
            };
            Add(_sprite);
        }

        public Selector(Sprite unselectedSprite, Sprite selectedSprite, float unselectedSpriteSize,
            float selectedSpriteSize, EasingMode spriteSizeEasing) : this() {
            _unselectedSprite = unselectedSprite;
            _selectedSprite = selectedSprite;
            _unselectedSpriteSize = unselectedSpriteSize;
            _selectedSpriteSize = selectedSpriteSize;
            _spriteSizeEasing = spriteSizeEasing;
            UpdateTransitions();
            UpdateSprite();
        }

        private void UpdateSprite() {
            if (_isSelected) {
                _sprite.style.backgroundImage = new StyleBackground(_selectedSprite);
                _sprite.style.width = _selectedSpriteSize;
                _sprite.style.height = _selectedSpriteSize;
            }
            else {
                _sprite.style.backgroundImage = new StyleBackground(_unselectedSprite);
                _sprite.style.width = _unselectedSpriteSize;
                _sprite.style.height = _unselectedSpriteSize;
            }
        }

        private void UpdateTransitions() {
            _sprite.style.AddTransition("width", 0, 0.5f, _spriteSizeEasing);
            _sprite.style.AddTransition("height", 0, 0.5f, _spriteSizeEasing);
        }
    }
}