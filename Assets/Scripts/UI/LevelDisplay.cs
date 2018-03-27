using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RatStudios.UI
{
    public class LevelDisplay : DisplayBase<LevelDisplay>
    {
        public GameObject player;

        private Image backdrop;
        private Text levelText;
        private RectTransform trans;

        private int roomNumber = 0;
        private int difficulty = 0;

        public void Start()
        {
            trans = GetComponent<RectTransform>();

            bool noError = true;
            if (singleton == null)
            {
                backdrop = GetComponent<Image>();
                noError = noError && (backdrop != null);

                levelText = GetComponentInChildren<Text>();
                noError = noError && (levelText != null);

                if (noError)
                {
                    singleton = this;
                    updateText();
                }
                else
                {
                    print(backdrop);
                    print(levelText);
                    throw new WrongUICompositionException();
                }
            }
            else
            {
                throw new DuplicateUIException();
            }

        }

        public static void setRoomNumber(int number)
        {
            singleton.roomNumber = number;
            singleton.updateText();
        }

        public static void setDifficulty(int diff)
        {
            singleton.difficulty = diff;
            singleton.updateText();
        }

        //TODO: Positioning of all these UI thingiemadohickers.
        private void updateText()
        {
            levelText.text = "Room: " + roomNumber + "\n"
                + "Difficulty: " + difficulty;
            trans.position = new Vector3(/*(Camera.main.pixelWidth / 2) - (trans.rect.width / 2)*/ 0, Camera.main.pixelHeight - 20, 0);
        }
    }
}


