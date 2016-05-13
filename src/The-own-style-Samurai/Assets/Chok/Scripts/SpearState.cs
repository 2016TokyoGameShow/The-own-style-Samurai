using UnityEngine;
using System.Collections;

public class SpearState : IMeleeState {

    protected override void SpecialMove()
    {
        StartCoroutine(StepBack());
    }

    private IEnumerator StepBack()
    {
        for (float i = 0; i < 0.1f; i += Time.deltaTime)
        {
            transform.position -= transform.forward / 2;
            yield return 0;
        }
        StartCoroutine(StepForward());
    }

    private IEnumerator StepForward()
    {
        yield return new WaitForSeconds(0.5f);
        for (float i = 0; i < 0.1f; i += Time.deltaTime)
        {
            transform.position += transform.forward / 2;
            yield return 0;
        }
        enemy.StartAttack();
    }
}
