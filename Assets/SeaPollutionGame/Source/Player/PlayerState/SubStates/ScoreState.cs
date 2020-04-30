public class ScoreState : IScoreState
{
    private ScoreWeight scoreWeight = null;
    private IPollutionState pollutionState;
    private IEconomicState economicState;
    private IGoalState goalState;

    public ScoreState(IPollutionState pollutionState, IEconomicState economicState, IGoalState goalState)
    {
        this.pollutionState = pollutionState;
        this.economicState = economicState;
        this.goalState = goalState;
    }

    public void SetScoreWeight(ScoreWeight weight) { scoreWeight = weight; }

    public float GetEfficiency()
    {
        float emission = Util.SumMap(pollutionState.GetAccumulatedPollutionMap(PollutionMapType.NET));
        if (emission == 0) { return 0; }
        return economicState.GetMoney() / emission;
    }

    public float GetScore()
    {
        float score = scoreWeight.money * (economicState.GetMoney() + economicState.GetAssetValue())
            + scoreWeight.filtered * (-Util.SumMap(pollutionState.GetAccumulatedPollutionMap(PollutionMapType.FILTERED)))
            + scoreWeight.efficiency * GetEfficiency()
            + goalState.GetGoalBounusScore();
        return score;
    }
}
