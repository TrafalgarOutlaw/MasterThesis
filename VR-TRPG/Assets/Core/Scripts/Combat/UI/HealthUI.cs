using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VRTRPG.Combat
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro healthText;
        [SerializeField] Transform textTransform;
        ACombatable combatUnit;

        // Start is called before the first frame update
        void Start()
        {
            combatUnit = transform.parent.GetComponent<ACombatable>();
            healthText.text = combatUnit.GetHealth().ToString();

            combatUnit.OnHealthChanged.AddListener(UpdateHealthText);
        }
        void Update()
        {
            textTransform.rotation = Quaternion.LookRotation(textTransform.position - Camera.main.transform.position);
        }

        void UpdateHealthText()
        {
            healthText.text = combatUnit.GetHealth().ToString();
        }
    }
}
