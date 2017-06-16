using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationUtils : MonoBehaviour {
    [Header("Battle Animations (IBattleAnimation)")]
    //Given GameObjects must have a component implementing the IBattleAnimation interfesse
    public GameObject baseAttackAnim;
    public GameObject fireAnim;
    public GameObject powerOfLoveAnim;
    public GameObject powerOfLoveBuffAnim;
    public GameObject healingAnim;
}
