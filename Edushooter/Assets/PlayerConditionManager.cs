using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class PlayerConditionManager : MonoBehaviour
{
    [System.Serializable]
    private class ActiveCondition
    {
        public PowerupCondition Condition;
        public IEnumerator RepeatableEffect = null;
        public float currentDuration = 0f;
    }

    public GameObject Owner;

    [SerializeField] private List<ActiveCondition> ActiveConditions = new List<ActiveCondition>();
    [SerializeField] private List<ActiveCondition> ConditionsToRemove = new List<ActiveCondition>();

    public void AddCondition(PowerupCondition condition)
    {
        float conditionDuration = Random.Range(condition.MinDuration, condition.MaxDuration);

        ActiveCondition newActiveCondition = new ActiveCondition
        {
            Condition = condition,
            currentDuration = conditionDuration,
        };

        // Add the new active condition to the list
        ActiveConditions.Add(newActiveCondition);
        newActiveCondition.Condition.StartCondition(Owner);

        ConditionUIManager.TryGetInstance()?.TrackNewCondition(condition, conditionDuration);

        // If the condition has a repeatable effect
        if (condition.ConditionHasRepeatableEffect)
        {
            // Start the repeatable effect coroutine
            newActiveCondition.RepeatableEffect = RepeatEffect(condition);
            StartCoroutine(newActiveCondition.RepeatableEffect);
        }
    }

    private void Update()
    {
        // If there are active conditions...
        if (ActiveConditions.Count > 0)
        {
            foreach (var activeCondition in ActiveConditions)
            {
                activeCondition.currentDuration -= Time.deltaTime;

                // If the condition has run out of time...
                if (activeCondition.currentDuration <= 0f)
                {
                    ConditionsToRemove.Add(activeCondition);
                    activeCondition.Condition.EndCondition(); // End the condition

                    // Stop the repeatable effect if it exists
                    if (activeCondition.RepeatableEffect != null)
                    {
                        StopCoroutine(activeCondition.RepeatableEffect);
                    }
                }
            }

            // Remove conditions after iteration
            foreach (var condition in ConditionsToRemove)
            {
                ActiveConditions.Remove(condition);
            }

            ConditionsToRemove.Clear(); // Clear the list for the next frame
        }
    }

    // Coroutine to repeat the effect at the defined frequency
    private IEnumerator RepeatEffect(PowerupCondition condition)
    {
        while (true)
        {
            condition.RepeatableConditionEffect(); // Execute the repeatable effect
            yield return new WaitForSeconds(condition.EffectFrequency); // Wait for the effect frequency interval
        }
    }
}
