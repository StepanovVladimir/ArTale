using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class InitialSituationFunction : ICharacterFunction
    {
        public InitialSituationFunction(TaleKind taleKind, string place, Characters characters)
        {
            Place = place;
            ActingCharacter = characters.Parent;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                AdditionalCharacter1 = characters.Victim;
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                AdditionalCharacter1 = characters.Hero;
                AdditionalCharacter2 = characters.Victim;
            }
            else
            {
                AdditionalCharacter1 = characters.Hero;
            }
        }

        public string Name { get; } = "Начальная ситуация";

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
