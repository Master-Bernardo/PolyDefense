using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* todo not yet imlemented*/
public class Ab_UnitAI : Ability
{
    //switches between different behaviours 

    //public Behaviour[] behaviours;
    protected Behaviour currentBehaviour;
 

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        /*for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].SetUp(this);
        }*/
    }

    public override void UpdateAbility()
    {
        //1check if we need to change the current Bahaviour
        CheckCurrentBehaviour();
        //2. update bahaviour
        currentBehaviour.UpdateBehaviour();
    }

    public virtual void CheckCurrentBehaviour()
    {

    }

    protected void SetCurrentBehaviour(Behaviour newBehaviour)
    {
        if (currentBehaviour != newBehaviour)
        {
            if(currentBehaviour!=null)currentBehaviour.OnBehaviourExit();
            currentBehaviour = newBehaviour;
            currentBehaviour.OnBehaviourEnter();
        }
    }
}
