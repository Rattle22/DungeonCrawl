using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RatStudios.UI
{
    //TODO: Saving and Loading should probably not be here, but instead in another class.
    public class ScoreList : MonoBehaviour
    {
        private class DateComparer<T> : Comparer<T> where T : IComparable
        {
            public override int Compare(T x, T y)
            {
                return y.CompareTo(x);
            }
        }

        public GameObject scoreEntryPrefab;
        private SortedList<int, List<DateTime>> scores = new SortedList<int, List<DateTime>>(new DateComparer<int>());
        private int totalScores = 0;

        public void addEntry(DateTime time, int score)
        {
            List<DateTime> timeList;
            //If the score exists, add the given time to its list
            if (scores.TryGetValue(score, out timeList))
            {
                timeList.Add(time);
            }
            else
            {
                //Else, create a new List with the new time as the first entry
                scores.Add(score, new List<DateTime>(new DateTime[] { time }));
            }

            totalScores++;
        }

        public void saveList()
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite("SaveData.dat")))
            {
                writer.Write(scores.Count);
                foreach (int score in scores.Keys)
                {
                    List<DateTime> times;
                    if (scores.TryGetValue(score, out times))
                    {
                        writer.Write(score);
                        writer.Write(times.Count);
                        foreach (DateTime time in times)
                        {
                            writer.Write(time.ToBinary());
                        }
                    }
                }
            }
        }

        public void loadList()
        {
            if (!File.Exists("SaveData.dat"))
            {
                return;
            }

            using (BinaryReader reader = new BinaryReader(File.OpenRead("SaveData.dat")))
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int score = reader.ReadInt32();
                    int timeCount = reader.ReadInt32();
                    for (int j = 0; j < timeCount; j++)
                    {
                        DateTime time = DateTime.FromBinary(reader.ReadInt64());
                        addEntry(time, score);
                    }
                }
            }
        }

        public void showList()
        {
            int place = 0;

            foreach (int score in scores.Keys)
            {
                List<DateTime> times;
                if (scores.TryGetValue(score, out times))
                {
                    foreach (DateTime time in times)
                    {
                        if (place < 9) {
                            place++;
                            GameObject entryObject = Instantiate(scoreEntryPrefab, transform);
                            ScoreEntry entry = entryObject.GetComponent<ScoreEntry>();
                            entry.SetEntry(time, score, place);
                        }
                    }
                }
            }
        }
    }
}