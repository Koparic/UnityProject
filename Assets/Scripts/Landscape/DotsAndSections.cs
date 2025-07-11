using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotsAndSections : MonoBehaviour
{
    public class Dot
    {
        public Dot prev;
        public Vector2 coords;
        public bool isCross;
        public Dot next;
    }

    public class Section
    {
        public Dot begin;
        public Dot end;
        public bool isCrossing;
        public Section cross;
    }

    public class Land
    {
        public int index;
        public Section[] sections;

       
    }
    
}
