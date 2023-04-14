using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class PassingTestFunction : ICharacterFunction
    {
        public string Name { get; } = "Прохождение испытания";

        public string StorylineString { get; }

        public int SceneIndex { get; }

        public string Place { get; set; }

        public string ActingCharacter { get; set; }

        public string AdditionalCharacter1 { get; set; }

        public string AdditionalCharacter2 { get; set; }
    }
}
