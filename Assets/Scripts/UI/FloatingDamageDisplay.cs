using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RatStudios.UI {
    public class FloatingDamageDisplay : DisplayBase<FloatingDamageDisplay> {

        private class FloatingDamageNumber {

            private float height;
            private Vector3 originalPosition;
            private Text text;
            private float maxHeight;
            private bool exists = true;
            public bool Exists { get { return exists; } }

            public FloatingDamageNumber(Vector3 originalPosition, Text textObject, float maxHeight) {
                this.originalPosition = originalPosition;
                text = textObject;
                this.maxHeight = maxHeight;

                @float(0);
            }

            public void @float(float amount) {
                if (height < maxHeight && exists)
                {
                    height += amount;
                    Vector3 position = originalPosition;
                    position.y += height;
                    text.transform.position = position;
                }
                else if (exists) {
                    Destroy(text);
                    exists = false;
                }
            }

            /// <summary>
            /// Resets the number and increases it by the given amount.
            /// </summary>
            /// <param name="amount"></param>
            public void increaseNumber(int amount) {
                if (exists) {
                    int currentAmount = int.Parse(text.text);
                    amount += currentAmount;
                    text.text = "" + amount;

                    @float(-height);
                }
            }

            public void setNewPosition(Vector3 position) {
                originalPosition = position;
            }
        }

        public GameObject damageTextPrefab;
        public float maxFloat;
        public float factorPerFrame;
        private Dictionary<GameObject, FloatingDamageNumber> numbers = new Dictionary<GameObject, FloatingDamageNumber>();

        private void Start()
        {
            if (singleton == null)
            {
                singleton = this;
            }
            else {
                throw new DuplicateUIException();
            }
        }

        private void Update()
        {
            List<GameObject> numbersToDelete = new List<GameObject>();

            foreach (GameObject g in numbers.Keys)
            {
                FloatingDamageNumber n;
                if (numbers.TryGetValue(g, out n))
                {
                    if (n.Exists)
                    {
                        n.@float(maxFloat * factorPerFrame);
                    }
                    else
                    {
                        numbersToDelete.Add(g);
                    }
                }
            }

            foreach (GameObject g in numbersToDelete)
            {
                numbers.Remove(g);
            }
        }

        public static void ShowDamage(int amount, GameObject owner, float verticalOffset) {
            singleton.showDamage(amount, owner, verticalOffset);
        }

        private void showDamage(int amount, GameObject owner, float verticalOffset) {
            if (!numbers.ContainsKey(owner))
            {
                GameObject textObject = Instantiate(damageTextPrefab);
                textObject.transform.SetParent(transform);
                Text text = textObject.GetComponent<Text>();
                text.text = "" + 0;

                Vector3 position = owner.transform.position;
                position.y += verticalOffset;

                FloatingDamageNumber number = new FloatingDamageNumber(position, text, maxFloat);
                number.increaseNumber(amount);
                numbers.Add(owner, number);
            }
            else {
                FloatingDamageNumber number;
                if (numbers.TryGetValue(owner, out number)) {
                    number.increaseNumber(amount);
                    number.setNewPosition(owner.transform.position);
                }
            }

        }
    }
}