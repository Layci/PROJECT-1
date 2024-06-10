using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public interface ISelectable
    {
        public GameObject Selection { get; }
        public void Select();
        public void Deselect();
    }
}
