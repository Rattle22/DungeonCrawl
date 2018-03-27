using UnityEngine;
using UnityEngine.UI;

namespace RatStudios.UI
{
    public class HealthBar : MonoBehaviour {

        private static HealthBar singleton;

        private Image backdrop;
        private Text healthText;

        public void Start()
        {
            bool noError = true;
            if (singleton == null)
            {
                backdrop = GetComponent<Image>();
                noError = noError && (backdrop != null);

                healthText = GetComponentInChildren<Text>();
                noError = noError && (healthText != null);

                if (noError)
                {
                    Vector3 position = new Vector3();
                    RectTransform transform = GetComponent<RectTransform>();
                    position.y += Camera.main.pixelHeight;
                    transform.position = position;

                    singleton = this;
                }
                else
                {
                    print(backdrop);
                    print(healthText);
                    throw new WrongUICompositionException();
                }
            }
            else
            {
                throw new DuplicateUIException();
            }

        }

        public static void showHealth(int max, int current)
        {
            singleton.actShowHealth(max, current);
        }

        private void actShowHealth(int max, int current)
        {
            healthText.text = current + "/" + max;
        }
    }
}

