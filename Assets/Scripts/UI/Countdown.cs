using UnityEngine;
using UnityEngine.UI;

namespace RatStudios.UI
{
    public class Countdown : DisplayBase<Countdown>
    {
        public GameObject player;

        private Image backdrop;
        private Text timeText;
        private float time;
        private bool running;

        public void Start()
        {
            GetComponent<RectTransform>().position = new Vector3(1.777777777777778f * Camera.main.pixelWidth - 100, Camera.main.pixelHeight, 0);

            bool noError = true;
            if (singleton == null)
            {
                backdrop = GetComponent<Image>();
                noError = noError && (backdrop != null);

                timeText = GetComponentInChildren<Text>();
                noError = noError && (timeText != null);

                if (noError)
                {
                    singleton = this;
                }
                else
                {
                    print(backdrop);
                    print(timeText);
                    throw new WrongUICompositionException();
                }
            }
            else
            {
                throw new DuplicateUIException();
            }

        }

        public static void setRunning(bool running)
        {
            singleton.running = running;
        }

        public void Update()
        {
            if (running)
            {
                time -= Time.deltaTime;
                showTime(time);

                if (time < 0)
                {
                    setColor(Color.red);
                    player.GetComponent<Entity_Life>().dealDamage(-(int)time);
                }
                else {
                    setColor(Color.black);
                }
            }
        }

        public static void addTime(float time)
        {
            //If the time has run out, reset to the given time instead of adding.
            singleton.time = time < 0 ? time : singleton.time + time;
        }

        private void showTime(float time)
        {
            if (time < 0)
            {
                timeText.text = "00:00";
                return;
            }

            float minutes = Mathf.Floor(time / 60);
            float seconds = time % 60;
            timeText.text = minutes + ":" + seconds;
        }

        private void setColor(Color color) {
            timeText.color = color;
        }
    }
}

