using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class TeleporterManager : MonoBehaviour
    {
        public static TeleporterManager Instance;

        public List<Teleporter> teleporters = new();

        private void Awake()
        {
            Instance = this;
        }

        public void Register(Teleporter tp)
        {
            teleporters.Add(tp);
        }
    }
}
