using System.Collections.Generic;
using System.Linq;

public class GoalState : IGoalState
{
    private Goal[] goals = null;
    private List<Goal> achievedGoals = new List<Goal> { };
    private IResourceState resourceState;

    public GoalState(IResourceState resourceState)
    {
        this.resourceState = resourceState;
    }

    public void AddToAchievedGoals(Goal goal) { achievedGoals.Add(goal); }
    public Goal[] GetAchievedGoals() { return achievedGoals.ToArray(); }
    public float GetGoalBounusScore()
    {
        float sum = 0;
        foreach (var goal in achievedGoals)
        {
            sum += goal.reward;
        }
        return sum;
    }

    public void SetGoals(Goal[] goals) { this.goals = goals; }
    public Goal[] GetGoals() { return goals; }
    public bool HasMetGoal(Goal goal)
    {
        return GetProgress(goal) == 1;
    }

    public float GetProgress(Goal goal)
    {
        var resourceMap = resourceState.GetAccumulatedResourceMap();
        float val = 0;
        resourceMap.TryGetValue(goal.resourceName, out val);
        return val / goal.value;
    }

    private void UpdateAchievedGoals()
    {
        foreach (var goal in goals)
        {
            if (HasMetGoal(goal) && !achievedGoals.Contains(goal))
            {
                achievedGoals.Add(goal);
            }
        }
    }
}