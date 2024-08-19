using System.Collections.Generic;

public enum FlowerEnum
{
    Idle,
    Attack
}

public class FlowerStateMachine
{
    public FlowerState CurrentState { get; private set; }
    
    public Dictionary<FlowerEnum, FlowerState> stateDictionary = new Dictionary<FlowerEnum, FlowerState>();
    
    public Flower _flower;
    
    public void Initialize(FlowerEnum startState, Flower flower)
    {
        _flower = flower;
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(FlowerEnum newState)
    {
        CurrentState.Exit();
        CurrentState = stateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(FlowerEnum stateEnum, FlowerState flowerState)
    {
        stateDictionary.Add(stateEnum, flowerState);
    }
}
