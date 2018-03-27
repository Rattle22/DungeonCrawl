using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RatStudios.UI
{
    //TODO: Is the Service/Werkzeug/Material Pattern good for scores?
    public class Scoreboard : Dialog<Scoreboard>
    {
        private Button exitButton;
        private Button restartButton;
        private CanvasGroup backdrop;
        private ScoreList scoreList;

        public void Start()
        {
            bool noError = true;
            if (singleton == null)
            {
                backdrop = GetComponent<CanvasGroup>();
                noError = noError && (backdrop != null);

                exitButton = transform.Find("EndGame").GetComponent<Button>();
                noError = noError && (exitButton != null);

                restartButton = transform.Find("Restart").GetComponent<Button>();
                noError = noError && (restartButton != null);

                scoreList = GetComponentInChildren<ScoreList>();
                noError = noError && (scoreList != null);

                if (noError)
                {
                    singleton = this;
                    setEnabled(false);
                    exitButton.onClick.AddListener(() => {
                        Application.Quit();
                    });
                    restartButton.onClick.AddListener(() => {
                        SceneManager.LoadScene("Testing");
                        Time.timeScale = 1;
                    });
                }
                else
                {
                    print(backdrop);
                    print(exitButton);
                    throw new WrongUICompositionException();
                }
            }
            else
            {
                throw new DuplicateUIException();
            }

        }

        public static void show() {
            singleton.setEnabled(true);
        }

        public static void addEntry(DateTime time, int score) {
            singleton.scoreList.addEntry(time, score);
        }

        private void setEnabled(bool enabled)
        {
            backdrop.alpha = enabled ? 1 : 0;
            exitButton.enabled = enabled;
            restartButton.enabled = enabled;

            if (enabled)
            {
                scoreList.loadList();
                scoreList.showList();
                scoreList.saveList();
                addInterrupt(this);
            }
            else
            {
                removeInterrupt(this);
            }

        }
    }
}