using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class VillainyFunction : ICharacterFunction
    {
        public VillainyFunction(TaleKind taleKind, string place, Characters characters)
        {
            Place = place;
            ActingCharacter = characters.Antagonist1;
            if (taleKind != TaleKind.BoyEscapesFromBabaYaga)
            {
                AdditionalCharacter1 = characters.Victim;
            }
            else
            {
                AdditionalCharacter1 = characters.Hero;
            }
        }

        public string Name { get; } = "Вредительство";

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
