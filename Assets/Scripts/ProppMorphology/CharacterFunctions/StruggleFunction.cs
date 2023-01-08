using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class StruggleFunction : ICharacterFunction
    {
        public StruggleFunction(TaleKind taleKind, string place, Characters characters)
        {
            Place = place;
            AdditionalCharacter1 = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                ActingCharacter = characters.Antagonist1;
                AdditionalCharacter2 = characters.Victim;
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                ActingCharacter = characters.Antagonist2;
                AdditionalCharacter2 = characters.Victim;
            }
            else
            {
                ActingCharacter = characters.Antagonist2;
            }
        }

        public string Name { get; } = "Борьба";

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
