using System.Collections.Generic;

public class EconomicState : IEconomicState
{
    private float money = 120;
    private IPolluterOwner polluterOwner = null;

    public EconomicState(float startMoney, IPolluterOwner polluterOwner)
    {
        money = startMoney;
        this.polluterOwner = polluterOwner;
    }

    public float GetMoney() { return money; }
    public void SetMoney(float val) { money = val; }
    public void AddMoney(float delta) { SetMoney(money + delta); }

    public float GetAssetValue()
    {
        float sum = 0;
        foreach (var polluter in polluterOwner.GetPolluters())
        {
            if (polluter.IsAlive())
            {
                sum += polluter.GetAttrib().economicAttrib.price;
            }
        }
        return sum;
    }

    public float GetTurnIncome()
    {
        float income = 0;
        foreach (var polluter in polluterOwner.GetPolluters())
        {
            income += polluter.GetProfit();
        }
        return income;
    }

    public void AccumulateMoney()
    {
        AddMoney(GetTurnIncome());
    }
}
