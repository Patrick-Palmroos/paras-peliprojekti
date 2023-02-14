using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class StoryNode
    {
        public int Id { get; set; }
        public string Prompt { get; set; }
        public string OptionLeft { get; set; }
        public string OptionRight { get; set; }
        public int ChildLeft { get; set; }
        public int ChildRight { get; set; }
    }
}
