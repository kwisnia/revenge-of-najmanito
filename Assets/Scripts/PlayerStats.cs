using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerStats
{
    public static int collectedCherries { get; private set; }
        public static int totalCherries { get; private set; }
        public static readonly int[] MAXCherryCount = new int[3];
        public static readonly int[] CollectedCherriesPerLevel = new int[3];
        public static int deaths { get; private set; }
        public static bool crossbowCollected { get; set; }
        private static int _currentSceneIndex;
        public static bool gameCompleted { get; set; }
        public static void LookForCherries()
        {
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
            CollectedCherriesPerLevel[_currentSceneIndex] = 0;
            MAXCherryCount[_currentSceneIndex] =
                GameObject.FindGameObjectsWithTag("Cherry").Length;
        }

        public static void AddCherry()
        {
            collectedCherries++;
            CollectedCherriesPerLevel[_currentSceneIndex]++;
            Debug.Log(collectedCherries);
            Debug.Log(CollectedCherriesPerLevel[_currentSceneIndex]);
        }

        public static void AddDeath()
        {
            deaths++;
        }

        public static void CalculateTotal()
        {
            totalCherries = MAXCherryCount.Sum();
        }

        public static void ResetStats()
        {
            deaths = 0;
            collectedCherries = 0;
        }
    }
