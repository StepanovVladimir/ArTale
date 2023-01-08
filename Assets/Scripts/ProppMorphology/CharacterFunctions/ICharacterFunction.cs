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
        public string Place { get; set; }
        public string ActingCharacter { get; }
        public string AdditionalCharacter1 { get; }
        public string AdditionalCharacter2 { get; }
    }
}
