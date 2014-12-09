using UnityEngine;
using System.Collections;

public class BullSeye : LivingEntity {

    private static int BullSeyeCount = 0;

	// Use this for initialization
	public override void Start () {
        base.Start();
        BullSeyeCount += 1;
	}

    public override void Die()
    {
        base.Die();
        BullSeyeCount -= 1;

        if (BullSeyeCount == 0)
        {
            // Finish The Game
            GlobalScript globalEntity = GameObject.FindGameObjectWithTag("GlobalEntity").GetComponent<GlobalScript>();
            globalEntity.FinishGame();
        }
    }
}
