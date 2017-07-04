using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {

        public abstract void Use(AbilityUseParams useParams);
    }
}