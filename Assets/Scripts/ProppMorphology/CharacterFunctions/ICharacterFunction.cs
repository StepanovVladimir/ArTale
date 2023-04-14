using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    interface ICharacterFunction
    {
        public string Name { get; }

        public string StorylineString { get; }

        public int SceneIndex { get; }

        public string Place { get; }
        public string ActingCharacter { get; }
        public string AdditionalCharacter1 { get; }
        public string AdditionalCharacter2 { get; }
    }
}
