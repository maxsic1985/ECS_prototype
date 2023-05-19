using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StaticData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MSuhininTestovoe.B2B
{
    public class PatternService : IPatternService
    {
        //Addressables spawn pattern label
        private const string ADDRESSABLE_LABEL = "SpawnPattern";

        // Define the maximum and minimum probabilities for the easyList and mediumList
        private const float MAX_EASY_PROBABILITY = 96f;
        private const float MIN_EASY_PROBABILITY = 4f;

        private const float MIN_MEDIUM_PROBABILITY = 4f;

        // Define the number of iterations to run
        private const int ITERATIONS = 20;

        private List<SpawnPatternStaticData> _patterns;
        private List<SpawnPatternStaticData> _easyList = new();
        private List<SpawnPatternStaticData> _mediumList = new();
        private List<SpawnPatternStaticData> _hardList = new();

        // Store the current iteration count and maximum medium probability
        private int iterationCount;
        private float maxMediumProbability;

        // Store the current probabilities for selecting from each list
        private float probEasy = 96f;
        private float probMedium = 3f;


        public async Task Initialize()
        {
            Task result = LoadPatterns(ADDRESSABLE_LABEL);
            await result;

            _easyList = GetPatternsGroup(PatternGroups.Ease).ToList();
            _mediumList = GetPatternsGroup(PatternGroups.Medium).ToList();
            _hardList = GetPatternsGroup(PatternGroups.Hard).ToList();
        }

        public IEnumerable<SpawnPatternStaticData> GetPatternsGroup(PatternGroups patternGroup)
        {
            return _patterns.Where(p => p.PatternGroup == patternGroup);
        }
        public SpawnPatternStaticData GetRandomPattern()
        {
            iterationCount++;

            // Update the probabilities based on the current iteration and previous selections
            float[] updatedProbabilities = UpdateProbabilities(probEasy, probMedium, maxMediumProbability);
            probEasy = updatedProbabilities[0];
            probMedium = updatedProbabilities[1];
            float probHard = updatedProbabilities[2];
            maxMediumProbability = updatedProbabilities[3];

            // Generate a random number based on the updated probabilities
            List<SpawnPatternStaticData> chosenList = GetRandomList(probEasy, probMedium, probHard, _easyList, _mediumList, _hardList);
            int count = chosenList.Count;
            int randomIndex = Random.Range(0, count);
            return chosenList[randomIndex];
        }

        private float[] UpdateProbabilities(float probEasy, float probMedium, float maxMediumProbability)
        {
            float maxEasyProbability = MAX_EASY_PROBABILITY;

            if (maxMediumProbability < probMedium)
            {
                maxMediumProbability = probMedium;
            }

            if (Mathf.Round(probEasy) > MIN_EASY_PROBABILITY)
            {
                probEasy -= (maxEasyProbability - MIN_EASY_PROBABILITY) / ITERATIONS;
                probMedium = (100f - probEasy) * 3f / 4f;
            }
            else if (Mathf.Round(probEasy) <= MIN_EASY_PROBABILITY && Mathf.Round(probMedium) > MIN_MEDIUM_PROBABILITY)
            {
                probEasy = MIN_EASY_PROBABILITY;
                probMedium -= (maxMediumProbability - MIN_MEDIUM_PROBABILITY) / ITERATIONS;
            }
            else
            {
                probEasy = MIN_EASY_PROBABILITY;
                probMedium = MIN_MEDIUM_PROBABILITY;
            }

            float probHard = 100f - probMedium - probEasy;

            return new[] { probEasy, probMedium, probHard, maxMediumProbability };
        }

        private List<SpawnPatternStaticData> GetRandomList(float probEasy, float probMedium, float probHard, List<SpawnPatternStaticData> easyList,
            List<SpawnPatternStaticData> mediumList, List<SpawnPatternStaticData> hardList)
        {
            // Generate a random number between 0 and 100
            float rand = Random.Range(0f, 100f);

            // Determine which list to select from based on the probabilities
            if (rand < probEasy)
                return easyList;
            
            return rand < probEasy + probMedium ? mediumList : hardList;
        }


        private async Task LoadPatterns(string label)
        {
            AsyncOperationHandle<IList<SpawnPatternStaticData>> soResult =
                Addressables.LoadAssetsAsync<SpawnPatternStaticData>(label, obj =>
                {
                    //Gets called for every loaded asset
                    Debug.Log($"Name: {obj.name} difficult: {obj.PatternGroup.ToString()}");
                });
            await soResult.Task;

            _patterns = soResult.Result.ToList();
        }
    }
}